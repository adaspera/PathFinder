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
        var res = await _mobilityDbService.GetAllFeedsAsync();
        return Ok(res);
    } 
    
    [HttpGet("GetMetadata")]
    public async Task<IActionResult> TestGetMetadata()
    {
        var res = await _mobilityDbService.GetMetadata();
        return Ok(res);
    }

    [HttpGet("GetFeedInfoById")]
    public async Task<IActionResult> TestGetFeedInfoById(string feedId)
    {
        try
        {
            var var = await _mobilityDbService.GetGtfsFeedDownloadUrlAsync(feedId);
            return Ok(var);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpGet("GetFeedById")]
    public async Task<IActionResult> TestGetFeedById(string feedId)
    {
        try
        {
            await _mobilityDbService.DownloadGtfsFeedAsync(feedId);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}