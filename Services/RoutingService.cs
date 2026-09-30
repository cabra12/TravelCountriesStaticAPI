using TravelLogisticsApi.Data;

namespace TravelLogisticsApi.Services;

public interface IRoutingService
{
    string[]? GetPath(string start, string DestinationCountry);
}

public class RoutingService : IRoutingService
{
    private readonly IBorderRepository _borderRepository;

    public RoutingService(IBorderRepository borderRepository)
    {
        _borderRepository = borderRepository;
    }

    public string[]? GetPath(string start, string DestinationCountry)
    {
        if (start.Equals(DestinationCountry, StringComparison.OrdinalIgnoreCase))
        {
            return [start];
        }

        var queue = new Queue<string>();
        var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var MapOfRoutes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        queue.Enqueue(start);
        visited.Add(start);

        bool destinationFound = false;

        while (queue.Count > 0)
        {
            string current = queue.Dequeue();

            if (current.Equals(DestinationCountry, StringComparison.OrdinalIgnoreCase))
            {
                destinationFound = true;
                break;
            }

            foreach (string neighborCountry in _borderRepository.GetNeighborCountries(current))
            {
                if (visited.Add(neighborCountry))
                {
                    MapOfRoutes[neighborCountry] = current;
                    queue.Enqueue(neighborCountry);
                }
            }
        }

        if (!destinationFound) return null;

        return [.. ReconstructPath(MapOfRoutes, start, DestinationCountry)];
    }

    private static List<string> ReconstructPath(Dictionary<string, string> MapOfRoutes, string start, string DestinationCountry)
    {
        var path = new List<string>();
        string current = DestinationCountry;

        while (current != start)
        {
            path.Add(current);
            current = MapOfRoutes[current];
        }

        path.Add(start);
        path.Reverse();

        return path;
    }
}