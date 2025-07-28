using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration.Attributes;
using Microsoft.EntityFrameworkCore;

namespace PathFinder.Server.Models;

public class Route
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    public string FeedId { get; set; }
    
    [ForeignKey(nameof(FeedId))]
    public FeedInfo? FeedInfo { get; set; }

    [Name("agency_id")]
    public string? AgencyId { get; set; } //TODO problema nes AgencyId is generated

    [ForeignKey(nameof(AgencyId))]
    public Agency? Agency { get; set; }

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