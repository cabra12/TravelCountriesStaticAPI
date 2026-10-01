namespace TravelLogisticsApi.Models;

public record RouteResponse(
    string Start, 
    string Destination, 
    string[] Route
);