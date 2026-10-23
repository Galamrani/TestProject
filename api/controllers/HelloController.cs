using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/hello")]
public class HelloController : ControllerBase
{
    [HttpGet]
    public IActionResult Get([FromQuery] string? name)
    {
        return Ok(new { message = $"Hello, {name ?? "world"}!", time = DateTime.Now });
    }
}
