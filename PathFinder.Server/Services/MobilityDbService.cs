using System.Globalization;
using System.IO.Compression;
using System.Text.Json.Serialization;
using CsvHelper;
using CsvHelper.Configuration;
using PathFinder.Data.Schema;
using PathFinder.Server.Models;
using PathFinder.Server.Repositories;
using PathFinder.Server.Repositories.Interfaces;
using Serilog;
using Route = Microsoft.AspNetCore.Routing.Route;

namespace PathFinder.Server.Services;


public class MobilityDbService
{
    private readonly HttpClient _httpClient;
    private readonly IStopRepository _stopRepository;
    private readonly IRouteRepository _routeRepository;
    private readonly IFeedInfoRepository _feedInfoRepository;
    private readonly IHttpClientFactory _httpClientFactory;

    public MobilityDbService(HttpClient httpClient, IStopRepository stopRepository,
        IRouteRepository routeRepository,
        IFeedInfoRepository feedInfoRepository,
        IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClient;
        _stopRepository = stopRepository;
        _routeRepository = routeRepository;
        _httpClientFactory = httpClientFactory;
        _feedInfoRepository = feedInfoRepository;
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
        
        await ProcessGtfsFileAsync<FeedInfo>(archive, "feed_info.txt", feedId, 
            (feed, fid) => { feed.Id = fid; return feed; }, 
            _feedInfoRepository);
        // TODO pasiduodu, pasalinsim reference i FeedInfo is kitu entities
        await ProcessGtfsFileAsync<Stop>(archive, "stops.txt", feedId, 
            (stop, fid) => { stop.FeedId = fid; stop.FeedInfo = _feedInfoRepository.GetById(fid); return stop; }, 
            _stopRepository);
        
        await ProcessGtfsFileAsync<Models.Route>(archive, "routes.txt", feedId, 
            (route, fid) => { route.FeedId = fid; route.FeedInfo = _feedInfoRepository.GetById(fid); return route; }, 
            _routeRepository);
    }
    
    private async Task ProcessGtfsFileAsync<T>(
        ZipArchive archive,
        string fileName,
        string feedId,
        Func<T, string, T> setFeedId,
        IBaseRepository<T> repository) where T : class
    {
        var entry = archive.GetEntry(fileName);
        if (entry == null)
        {
            Log.Warning("File {FileName} not found in GTFS feed", fileName);
            return;
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
        
        await repository.AddRangeAsync(records);
        await repository.SaveChangesAsync();

        Log.Information("Imported {Count} of type {Type} for feed {FeedId}", 
            records.Count, typeof(T).Name, feedId);
    }

}