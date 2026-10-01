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

    [HttpGet("")]
    public IActionResult GetHome()
    {
        string name = "Travel Logistics API";
        string usage = "GET /{countryCode}";
        string instructions = "Use the following country codes. Starting country is the USA. Country codes: CAN, USA, MEX, BLZ, GTM, SLV, HND, NIC, CRI, PAN";
        string example = "Get /PAN";

        var response = new { name, usage, instructions, example };
        return Ok(response);
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