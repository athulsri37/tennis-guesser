namespace TennisGuessr.Api.Geo;

// Fixed geographic reference data (approximate country centroids), used only
// to judge "closeness" between a guessed and the actual country for the
// closeness-tier clue feature. This is static geographic fact, not
// app-specific mutable data like player stats, so it's embedded directly in
// code rather than the database. Covers every country present in the
// Players seed data as of this writing -- add new entries here if a future
// player batch introduces a country not yet listed.
public static class CountryProximity
{
    private static readonly Dictionary<string, (double Lat, double Lon)> Coordinates = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Argentina"] = (-38.4161, -63.6167),
        ["Australia"] = (-25.2744, 133.7751),
        ["Austria"] = (47.5162, 14.5501),
        ["Belgium"] = (50.5039, 4.4699),
        ["Bosnia and Herzegovina"] = (43.9159, 17.6791),
        ["Brazil"] = (-14.2350, -51.9253),
        ["Bulgaria"] = (42.7339, 25.4858),
        ["Canada"] = (56.1304, -106.3468),
        ["Chile"] = (-35.6751, -71.5430),
        ["China"] = (35.8617, 104.1954),
        ["Croatia"] = (45.1000, 15.2000),
        ["Cyprus"] = (35.1264, 33.4299),
        ["Czech Republic"] = (49.8175, 15.4730),
        ["Denmark"] = (56.2639, 9.5018),
        ["Finland"] = (61.9241, 25.7482),
        ["France"] = (46.2276, 2.2137),
        ["Germany"] = (51.1657, 10.4515),
        ["Greece"] = (39.0742, 21.8243),
        ["Hungary"] = (47.1625, 19.5033),
        ["Italy"] = (41.8719, 12.5674),
        ["Japan"] = (36.2048, 138.2529),
        ["Kazakhstan"] = (48.0196, 66.9237),
        ["Latvia"] = (56.8796, 24.6032),
        ["Monaco"] = (43.7384, 7.4246),
        ["Morocco"] = (31.7917, -7.0926),
        ["Netherlands"] = (52.1326, 5.2913),
        ["Norway"] = (60.4720, 8.4689),
        ["Peru"] = (-9.1900, -75.0152),
        ["Poland"] = (51.9194, 19.1451),
        ["Portugal"] = (39.3999, -8.2245),
        ["Russia"] = (61.5240, 105.3188),
        ["Serbia"] = (44.0165, 21.0059),
        ["South Africa"] = (-30.5595, 22.9375),
        ["Spain"] = (40.4637, -3.7492),
        ["Sweden"] = (60.1282, 18.6435),
        ["Switzerland"] = (46.8182, 8.2275),
        ["USA"] = (37.0902, -95.7129),
        ["Ukraine"] = (48.3794, 31.1656),
        ["United Kingdom"] = (55.3781, -3.4360),
        ["Uruguay"] = (-32.5228, -55.7658),
        ["Uzbekistan"] = (41.3775, 64.5853),
    };

    // True if both countries are known and their centroids are within
    // thresholdKm of each other (haversine great-circle distance). Unknown
    // countries never count as close.
    public static bool IsWithin(string countryA, string countryB, double thresholdKm)
    {
        if (!Coordinates.TryGetValue(countryA, out var a) || !Coordinates.TryGetValue(countryB, out var b))
            return false;

        return HaversineDistanceKm(a.Lat, a.Lon, b.Lat, b.Lon) < thresholdKm;
    }

    private static double HaversineDistanceKm(double lat1, double lon1, double lat2, double lon2)
    {
        const double earthRadiusKm = 6371.0;
        var dLat = ToRadians(lat2 - lat1);
        var dLon = ToRadians(lon2 - lon1);

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return earthRadiusKm * c;
    }

    private static double ToRadians(double degrees) => degrees * Math.PI / 180.0;
}