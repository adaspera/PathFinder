using System.Globalization;
using System.IO.Compression;
using System.Text.Json.Serialization;
using CsvHelper;
using PathFinder.Server.Models;
using PathFinder.Server.Repositories;
using PathFinder.Server.Repositories.Interfaces;
using Serilog;

namespace PathFinder.Server.Services;


public class MobilityDbService
{
    private readonly HttpClient _httpClient;
    private readonly IStopRepository _stopRepository;
    private readonly IHttpClientFactory _httpClientFactory;

    public MobilityDbService(HttpClient httpClient, IStopRepository stopRepository, IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClient;
        _stopRepository = stopRepository;
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<object?> GetAllFeedsAsync()
    {
        return await _httpClient.GetFromJsonAsync<object>("v1/feeds?limit=10");
    }

    public async Task<object?> GetFeedInfoAsync(string feedId)
    {
        return await _httpClient.GetFromJsonAsync<object>($"feeds/{feedId}");
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
        
        var zipBytes = await response.Content.ReadAsByteArrayAsync();
    
        using (var zipStream = new MemoryStream(zipBytes))
        using (var archive = new ZipArchive(zipStream))
        {
            var stopsEntry = archive.GetEntry("stops.txt");
            using (var reader = new StreamReader(stopsEntry.Open()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var stops = csv.GetRecords<Stop>().ToList();
                
                foreach (var stop in stops)
                {
                    stop.FeedId = feedId;
                }
                
                await _stopRepository.AddRangeAsync(stops);
                await _stopRepository.SaveChangesAsync();
            }
        }
    }

}


//TODO move
public class GtfsFeedResponse
{
    public string Id { get; set; }
    [JsonPropertyName("latest_dataset")]
    public LatestDataset LatestDataset { get; set; }
}

public class LatestDataset
{
    [JsonPropertyName("hosted_url")]
    public string HostedUrl { get; set; } 
}