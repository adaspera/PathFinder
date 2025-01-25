using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PathFinder.Server.Services;

namespace PathFinder.Server.Controllers;

[ApiController]
[Route("/api/test")]
public class TestController : ControllerBase
{
    private MobilityDbService _mobilityDbService;

    public TestController(MobilityDbService mobilityDbService)
    {
        _mobilityDbService = mobilityDbService;
    }
    
    [HttpGet]
    public async Task<IActionResult> TestGet()
    {
        var res = await _mobilityDbService.GetFeedInfoAsync("1");
        return Ok(res);
    } 
    
    [HttpGet("GetMetadata")]
    public async Task<IActionResult> TestGetMetadata()
    {
        var res = await _mobilityDbService.GetMetadata();
        return Ok(res);
    } 
}