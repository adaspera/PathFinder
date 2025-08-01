using Microsoft.AspNetCore.Mvc;
using PathFinder.Data.Models.DTOs;
using PathFinder.Server.Services;
using Serilog;

namespace PathFinder.Server.Controllers;

[ApiController]
[Route("/api/citysearch")]
public class CitySearchController : ControllerBase
{
    private readonly CitySearchService _searchService;
    private readonly MobilityDbService _mobilityDbService;
    
    public CitySearchController(CitySearchService searchService, MobilityDbService mobilityDbService)
    {
        _searchService = searchService;
        _mobilityDbService = mobilityDbService;
    }
    
    [HttpGet]
    public IActionResult QuickSearch([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest();
        }
        
        var results = _searchService.SearchCities(query);
        return Ok(results);
    }
    
    [HttpGet("advanced")]
    public async Task<IActionResult> AdvancedSearch([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest();
        }

        var results = await _mobilityDbService.AdvancedSearchAsync(query);
        
        Log.Information("Advanced search found results: {n}", results?.Count);
        
        return Ok(results);
    }
    
    [HttpPost]
    public async Task<IActionResult> TestLoadSearchIndexes()
    {
        var cities = await _mobilityDbService.GetTestGtfsFeedInfoAsync();

        foreach (var city in cities)
        {
            Log.Information("Testing {city}", city.Provider);
        }
        
        if (cities == null) return BadRequest();
        
        _searchService.IndexCities(cities);
        return Ok();
    }
}