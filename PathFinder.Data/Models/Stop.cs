using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration.Attributes;
using Microsoft.EntityFrameworkCore;

namespace PathFinder.Server.Models;

[PrimaryKey(nameof(StopId), nameof(FeedId))]
public class Stop
{
    [Name("stop_id")]
    public string StopId { get; set; } 

    public string FeedId { get; set; }

    [Name("stop_name")]
    public string StopName { get; set; }

    [Name("stop_lat")]
    public double StopLat { get; set; }

    [Name("stop_lon")]
    public double StopLon { get; set; }
}