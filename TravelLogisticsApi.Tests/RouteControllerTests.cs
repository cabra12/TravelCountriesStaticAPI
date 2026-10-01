using Microsoft.AspNetCore.Mvc;
using Xunit;
using TravelLogisticsApi.Controllers; 
using TravelLogisticsApi.Services;
using TravelLogisticsApi.Data;

namespace TravelLogisticsApi.Tests;

public class RouteControllerTests
{
    private readonly RouteController _controller;

    public RouteControllerTests()
    {
        var data = new BorderRepository();
        var service = new RoutingService(data);
        _controller = new RouteController(service);
    }

    [Fact]
    public void GetRoute_ValidDestination_ReturnsOk()
    {
        var result = _controller.GetRoute("USA", "BLZ");
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void GetRoute_UnknownDestination_ReturnsError()
    {
        var result = _controller.GetRoute("USA", "UNKNOWN");
        Assert.IsType<NotFoundObjectResult>(result);
    }
    
    [Fact]
    public void GetRoute_EmptyDestination_ReturnsBadRequest()
    {
        var result = _controller.GetRoute(string.Empty, "USA"); 
        Assert.IsType<BadRequestObjectResult>(result);
    }
}