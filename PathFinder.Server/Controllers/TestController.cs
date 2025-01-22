using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace PathFinder.Server.Controllers;

[ApiController]
[Route("/api/test")]
public class TestController : ControllerBase
{
    
    [HttpGet]
    public async Task<IActionResult> TestGet()
    {
        
        
        return Ok(new[] { "a", "b" });
    } 
}