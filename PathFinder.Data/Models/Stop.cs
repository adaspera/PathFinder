using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration.Attributes;
using Microsoft.EntityFrameworkCore;

namespace PathFinder.Server.Models;

public class Stop
{
    [Key, Column(Order = 0)]
    [Name("stop_id")]
    public string StopId { get; set; } 

    [Key, Column(Order = 1)]
    public string FeedId { get; set; }

    [Name("stop_name")]
    public string StopName { get; set; }

    [Name("stop_lat")]
    public double StopLat { get; set; }

    [Name("stop_lon")]
    public double StopLon { get; set; }
}