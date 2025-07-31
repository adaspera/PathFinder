using System.Text.Json.Serialization;

namespace PathFinder.Data.Models.DTOs;

public class GtfsFeedResponseDto
{
    public string Id { get; set; }
    
    public string Provider {get; set;}
    
    [JsonPropertyName("latest_dataset")]
    public LatestDataset LatestDataset { get; set; }
    
    public IEnumerable<Location> Locations { get; set; }
}

public class LatestDataset
{
    [JsonPropertyName("hosted_url")]
    public string HostedUrl { get; set; } 
}

public class Location
{
    [JsonPropertyName("country_code")]
    public string CoutryCode { get; set; }
    
    public string Country { get; set; }
    
    [JsonPropertyName("subdivision_name")]
    public string SubdivisionName { get; set; }
    
    public string Municipality { get; set; }
}