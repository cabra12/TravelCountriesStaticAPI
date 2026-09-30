using System.Collections.Frozen;

namespace TravelLogisticsApi.Data;

public interface IBorderRepository
{
    string[] GetNeighborCountries(string countryCode);
}

public class BorderRepository : IBorderRepository
{
    private static readonly FrozenDictionary<string, string[]> CountryMap= new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
    {
            ["CAN"] = ["USA"],
            ["USA"] = ["CAN", "MEX"],
            ["MEX"] = ["USA", "GTM", "BLZ"],
            ["BLZ"] = ["MEX", "GTM"],
            ["GTM"] = ["MEX", "BLZ", "SLV", "HND"],
            ["SLV"] = ["GTM", "HND"],
            ["HND"] = ["GTM", "SLV", "NIC"],
            ["NIC"] = ["HND", "CRI"],
            ["CRI"] = ["NIC", "PAN"],
            ["PAN"] = ["CRI"]
    }.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);


    public string[] GetNeighborCountries(string countryCode)
    {
        if (CountryMap.TryGetValue(countryCode, out string[]? countries))
        {
            return countries;
        }

        return [];
    }
}