using Xunit;
using TravelLogisticsApi.Services;
using TravelLogisticsApi.Data;

namespace TravelLogisticsApi.Tests;

public class RoutingServiceTests
{
    private readonly RoutingService _routingService;
    public RoutingServiceTests()
    {
        var data = new BorderRepository();
        _routingService = new RoutingService(data);
    }

    [Fact]
    public void GetPath_ValidStartAndDestination_ReturnsCorrectRoute()
    {
        var result = _routingService.GetPath("USA", "BLZ");
        var expected = new string[] { "USA", "MEX", "BLZ" };
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetPath_ValidStartAndDestination_ReturnsCorrectRouteUSDestination()
    {
        var result = _routingService.GetPath("NIC", "USA");
        var expected = new string[] { "NIC", "HND", "GTM", "MEX", "USA" };
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetPath_UnknownDestination_ReturnsNull()
    {
        var result = _routingService.GetPath("USA", "UNKNOWN");
        Assert.Null(result);
    }

    [Fact]
    public void GetPath_EmptyDestination_ReturnsNull()
    {
        var result = _routingService.GetPath("USA", "");
        Assert.Null(result);
    }

    [Fact]
    public void GetPath_SameStartAndDestination_ReturnsStartOnly()
    {
        var result = _routingService.GetPath("USA", "USA");
        var expected = new string[] { "USA" };
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public void GetPath_SameStartAndDestination_ReturnsStartOnlyNonUS()
    {
        var result = _routingService.GetPath("BLZ", "BLZ");
        var expected = new string[] { "BLZ" };
        Assert.Equal(expected, result);
    }
}
