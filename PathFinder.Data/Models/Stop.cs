using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PathFinder.Server.Models;

public class Stop
{
    [Key]
    public string StopId { get; set; } 
    public string StopName { get; set; }
    public double StopLat { get; set; }
    public double StopLon { get; set; }
    public string CityId { get; set; } 
}