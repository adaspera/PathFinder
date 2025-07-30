using System.Globalization;
using System.IO.Compression;
using System.Text.Json.Serialization;
using CsvHelper;
using CsvHelper.Configuration;
using PathFinder.Data.Schema;
using PathFinder.Server.Data;
using PathFinder.Server.Models;
using PathFinder.Server.Repositories;
using PathFinder.Server.Repositories.Interfaces;
using Serilog;
using Route = Microsoft.AspNetCore.Routing.Route;

namespace PathFinder.Server.Services;


public class MobilityDbService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AppDbContext _dbContext;

    public MobilityDbService(HttpClient httpClient, IStopRepository stopRepository,
        AppDbContext dbContext,
        IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClient;
        _httpClientFactory = httpClientFactory;
        _dbContext = dbContext;
    }
    
    public async Task<object?> GetAllFeedsAsync(int limit)
    {
        return await _httpClient.GetFromJsonAsync<object>($"v1/feeds?limit={limit}");
    }

    public async Task<object?> GetFeedInfoAsync(string feedId)
    {
        return await _httpClient.GetFromJsonAsync<object>($"v1/feeds/{feedId}");
    }
    
    public async Task<object?> GetMetadata()
    {
        return await _httpClient.GetFromJsonAsync<object>("v1/metadata");
    }

    public async Task<string?> GetGtfsFeedDownloadUrlAsync(string feedId)
    {
        var feed = await _httpClient.GetFromJsonAsync<GtfsFeedResponse>($"v1/gtfs_feeds/{feedId}");
        Log.Information("fetched feed info: {@GtfsFeedResponse}", feed);
        return feed?.LatestDataset?.HostedUrl;
    }
    
    public async Task DownloadGtfsFeedAsync(string feedId)
    {
        var gtfsUrl= await GetGtfsFeedDownloadUrlAsync(feedId);
        if (gtfsUrl == null)
        {
            throw new Exception("GTFS feed URL not found");
        }

        var downloadClient = _httpClientFactory.CreateClient();
        var response = await downloadClient.GetAsync(gtfsUrl);
        response.EnsureSuccessStatusCode();
        
        Log.Information("Feed with id: {feedId} downloaded successfully", feedId);
        
        var zipBytes = await response.Content.ReadAsByteArrayAsync();

        using var zipStream = new MemoryStream(zipBytes);
        using var archive = new ZipArchive(zipStream);
        
        try
        {
            await ProcessGtfsFileAsync<FeedInfo>(archive, "feed_info.txt", feedId, 
                (feed, fid) => { feed.Id = fid; return feed; });
        
            await ProcessGtfsFileAsync<Stop>(archive, "stops.txt", feedId, 
                (stop, fid) => { stop.FeedId = fid; return stop; });
            
            var agencies = await ProcessGtfsFileAsync<Agency>(archive, "agency.txt", feedId, 
                (agency, fid) => { agency.FeedId = fid; return agency; });
        
            await ProcessGtfsFileAsync<Models.Route>(archive, "routes.txt", feedId, 
                (route, fid) => { route.FeedId = fid;
                    route.Agency = agencies.Find(a => a.Id == route.AgencyId);  
                    return route; });
        
            await _dbContext.SaveChangesAsync();
        
            Log.Information("Successfully imported feed {FeedId}", feedId);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to import feed {FeedId}", feedId);
            throw;
        }
        
        // await ProcessGtfsFileAsync<FeedInfo>(archive, "feed_info.txt", feedId, 
        //     (feed, fid) => { feed.Id = fid; return feed; }, 
        //     _feedInfoRepository);
        //
        // await ProcessGtfsFileAsync<Stop>(archive, "stops.txt", feedId, 
        //     (stop, fid) => { stop.FeedId = fid; return stop; }, 
        //     _stopRepository);
        //
        // await ProcessGtfsFileAsync<Models.Route>(archive, "routes.txt", feedId, 
        //     (route, fid) => { route.FeedId = fid; return route; }, 
        //     _routeRepository);
    }
    
    private async Task<List<T>?> ProcessGtfsFileAsync<T>(
        ZipArchive archive,
        string fileName,
        string feedId,
        Func<T, string, T> setFeedId) where T : class
    {
        var entry = archive.GetEntry(fileName);
        if (entry == null)
        {
            Log.Warning("File {FileName} not found in GTFS feed", fileName);
            return null;
        }

        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null
        };

        using var reader = new StreamReader(entry.Open());
        using var csv = new CsvReader(reader, csvConfig);

        var records = csv.GetRecords<T>()
            .Select(record => setFeedId(record, feedId))
            .ToList();
        
        await _dbContext.Set<T>().AddRangeAsync(records);

        Log.Information("Imported {Count} of type {Type} for feed {FeedId}", 
            records.Count, typeof(T).Name, feedId);
        
        return records;
    }

}