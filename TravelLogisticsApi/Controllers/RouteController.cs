using Microsoft.AspNetCore.Mvc;
using TravelLogisticsApi.Services;

namespace TravelLogisticsApi.Controllers;

[ApiController]
[Route("")]

public class RouteController : ControllerBase
{
    private readonly IRoutingService _routingService;

    public RouteController(IRoutingService routingService)
    {
        _routingService = routingService;
    }

    [HttpGet("{destination}")]
    public IActionResult GetRoute(string destination, [FromQuery] string start = "USA")
    {
        if (string.IsNullOrWhiteSpace(destination) || string.IsNullOrWhiteSpace(start))
        {
            return BadRequest(new { message = "Start and destination parameters cannot be empty." });
        }

        var route = _routingService.GetPath(start, destination);

        if (route == null)
        {
            return NotFound(new { message = $"No route found from {start} to {destination}" });
        }

        return Ok(new { start = start, destination = destination, route = route });
    }
}