using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration.Attributes;
using Microsoft.EntityFrameworkCore;

namespace PathFinder.Server.Models;

[PrimaryKey(nameof(Id), nameof(FeedId))]
public class Agency
{
    [Name("agency_id"), Required]
    public string Id { get; set; }
    
    [Required]
    public string FeedId { get; set; } = null!;

    [Name("agency_name"), Required]
    public string Name { get; set; } = null!;

    [Name("agency_url"), Required]
    public string Url { get; set; } = null!;

    [Name("agency_timezone"), Required]
    public string Timezone { get; set; } = null!;

    [Name("agency_lang")]
    public string? LanguageCode { get; set; }

    [Name("agency_phone")]
    public string? Phone { get; set; }
    
    public ICollection<Route>? Routes { get; set; }
}