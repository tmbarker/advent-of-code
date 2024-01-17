using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2021.D19;

public static class Report
{
    public static IList<Reporting> Parse(IEnumerable<string> lines)
    {
        var reportings = new List<Reporting>();
        var activeScanner = -1;
        var activeBeacons = new List<Vec3D>();

        foreach (var line in lines)
        {
            if (activeScanner < 0)
            {
                activeScanner = line.ParseInt();
                continue;
            }

            if (string.IsNullOrWhiteSpace(line))
            {
                reportings.Add(new Reporting(activeScanner, activeBeacons));
                activeScanner = -1;
                activeBeacons = new List<Vec3D>();
                continue;
            }
            
            activeBeacons.Add(item: Vec3D.Parse(line));
        }
        
        reportings.Add(new Reporting(activeScanner, activeBeacons));
        return reportings;
    }
}