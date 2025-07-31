using Microsoft.AspNetCore.Mvc;
using PathFinder.Server.Services;

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
    public IActionResult Search([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest();
        }
        
        var results = _searchService.SearchCities(query);
        return Ok(results);
    }
    
    [HttpPost]
    public async Task<IActionResult> TestLoadSearchIndexes()
    {
        var cities = await _mobilityDbService.GetTestGtfsFeedInfoAsync();
        _searchService.IndexCities(cities);
        return Ok();
    }
}