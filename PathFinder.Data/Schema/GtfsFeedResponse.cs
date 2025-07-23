using System.Text.Json.Serialization;

namespace PathFinder.Data.Schema;

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