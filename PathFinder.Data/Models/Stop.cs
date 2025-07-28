using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration.Attributes;
using Microsoft.EntityFrameworkCore;

namespace PathFinder.Server.Models;

public class Stop
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public string FeedId { get; set; }
    
    [ForeignKey(nameof(FeedId))]
    public FeedInfo? FeedInfo { get; set; }

    [Name("stop_name")]
    public string? Name { get; set; }

    [Name("stop_lat")]
    public double? Latitude { get; set; }

    [Name("stop_lon")]
    public double? Longitude { get; set; }

    [Name("zone_id")]
    public string? ZoneId { get; set; }
}