using System.Text.RegularExpressions;
using Utilities.Cartesian;

namespace Problems.Y2021.D19;

public static class Report
{
    private const string IntRegex = @"-?\d+";
    
    public static IList<Reporting> Parse(IEnumerable<string> lines)
    {
        var reportings = new List<Reporting>();
        var activeScanner = -1;
        var activeBeacons = new List<Vector3D>();

        foreach (var line in lines)
        {
            if (activeScanner < 0)
            {
                activeScanner = ParseScannerId(line);
                continue;
            }

            if (string.IsNullOrWhiteSpace(line))
            {
                reportings.Add(new Reporting(activeScanner, activeBeacons));
                activeScanner = -1;
                activeBeacons = new List<Vector3D>();
                continue;
            }
            
            activeBeacons.Add(ParseVector(line));
        }
        
        reportings.Add(new Reporting(activeScanner, activeBeacons));
        return reportings;
    }
    
    private static int ParseScannerId(string line)
    {
        return int.Parse(Regex.Match(line, IntRegex).Value);
    }

    private static Vector3D ParseVector(string line)
    {
        var matches = Regex.Matches(line, IntRegex);
        return new Vector3D(
            x: int.Parse(matches[0].Value),
            y: int.Parse(matches[1].Value),
            z: int.Parse(matches[2].Value));
    }
}