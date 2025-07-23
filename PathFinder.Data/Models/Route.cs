using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;
using Microsoft.EntityFrameworkCore;

namespace PathFinder.Server.Models;

[PrimaryKey(nameof(Id), nameof(FeedId))]
public class Route
{
    [Name("route_id"), Required]
    public string Id { get; set; }
    
    [Required]
    public string FeedId { get; set; }

    [Name("agency_id")]
    public string? AgencyId { get; set; }

    [Name("route_short_name")]
    public string? ShortName { get; set; }

    [Name("route_long_name")]
    public string? LongName { get; set; }

    [Name("route_type"), Required]
    public RouteType Type { get; set; }
}

public enum RouteType
{
    Tram = 0,
    Subway = 1,
    Bus = 2
}