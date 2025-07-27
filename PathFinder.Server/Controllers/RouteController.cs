using Microsoft.AspNetCore.Mvc;
using PathFinder.Server.Services.Interfaces;
using Serilog;

namespace PathFinder.Server.Controllers;

[ApiController]
[Route("/api/route")]
public class RouteController : ControllerBase
{
    
    private readonly IRouteService _routeService;

    public RouteController(IRouteService routeService)
    {
        _routeService = routeService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetRoutesByFeedId(string feedId)
    {
        try
        {
            var routes = await _routeService.GetAllAsync(feedId);
            return Ok(routes);
        }
        catch (Exception e)
        {
            Log.Warning("Error while fetching Routes in GetFeedRoutesById: {error}",e.Message);
            return BadRequest(e.Message);
        }
    }
}