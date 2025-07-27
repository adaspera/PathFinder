using Microsoft.AspNetCore.Mvc;
using PathFinder.Server.Repositories.Interfaces;
using PathFinder.Server.Services;
using PathFinder.Server.Services.Interfaces;
using Serilog;

namespace PathFinder.Server.Controllers;

[ApiController]
[Route("/api/stop")]
public class StopController : ControllerBase
{
    private readonly IStopService _stopService;
    
    public StopController(IStopService stopService)
    {
        _stopService = stopService;
    }
    
    [HttpGet("{feedId}")]
    public async Task<IActionResult> GetStopsByFeedId(string feedId)
    {
        try
        {
            var stops = await _stopService.GetAllAsync(feedId);
            return Ok(stops);
        }
        catch (Exception e)
        {
            Log.Warning("Error while fetching Stops in GetFeedStopsById: {error}",e.Message);
            return BadRequest(e.Message);
        }
    }
}