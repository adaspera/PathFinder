using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration.Attributes;
using Microsoft.EntityFrameworkCore;

namespace PathFinder.Server.Models;

[PrimaryKey(nameof(Id), nameof(FeedId))]
public class Stop
{
    [Name("stop_id")]
    public string Id { get; set; }

    [Required]
    public string FeedId { get; set; }

    [Name("stop_name")]
    public string? Name { get; set; }

    [Name("stop_lat")]
    public double? Latitude { get; set; }

    [Name("stop_lon")]
    public double? Longitude { get; set; }

    [Name("zone_id")]
    public string? ZoneId { get; set; }
}