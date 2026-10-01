using Microsoft.AspNetCore.Mvc;
using TravelLogisticsApi.Models;
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
            return BadRequest(new { message = "Start and destination countries are required." });
        }

        var route = _routingService.GetPath(start, destination);

        if (route == null)
        {
            return NotFound(new { message = $"No route found from {start} to {destination}" });
        }

        var response = new RouteResponse(start, destination, route);
        return Ok(response);
    }
}