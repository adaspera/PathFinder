using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;
using Microsoft.EntityFrameworkCore;

namespace PathFinder.Server.Models;

public class FeedInfo
{
    [Key]
    public string Id { get; set; }

    [Name("feed_publisher_name"), Required]
    public string FeedPublisherName { get; set; }
    
    [Name("feed_publisher_url"), Required]
    public string FeedPublisherUrl { get; set; }
    
    [Name("feed_lang"), Required]
    public string FeedLanguage { get; set; }
    
    [Name("feed_start_date")]
    public string? FeedStartDate { get; set; }
    
    [Name("feed_end_date")]
    public string? FeedEndDate { get; set; }
}