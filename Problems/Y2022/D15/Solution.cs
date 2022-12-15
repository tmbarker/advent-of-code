using System.Text.RegularExpressions;
using Problems.Y2022.Common;
using Utilities.DataStructures.Grid;
using Utilities.Extensions;

namespace Problems.Y2022.D15;

/// <summary>
/// Beacon Exclusion Zone: https://adventofcode.com/2022/day/15
/// </summary>
public class Solution : SolutionBase2022
{
    private const string SensorRegex = @".*x=(-?\d+).*y=(-?\d+).*x=(-?\d+).*y=(-?\d+)";
    private const int Row = 2000000;
    
    public override int Day => 15;
    
    public override string Run(int part)
    {
        var reportings = ParseReportings(GetInput());
        return part switch
        {
            0 => CountBeaconExcludedPositions(reportings, Row).ToString(),
            _ => ProblemNotSolvedString,
        };
    }

    private static int CountBeaconExcludedPositions(IEnumerable<(Vector2D sensorPos, Vector2D beaconPos)> reportings, int row)
    {
        var enumeratedReportings = reportings.ToList();
        var excludedBeaconPositions = new HashSet<Vector2D>();
        var sensorPositions = new HashSet<Vector2D>();
        var beaconPositions = new HashSet<Vector2D>();

        foreach (var (s, b) in enumeratedReportings)
        {
            sensorPositions.EnsureContains(s);
            beaconPositions.EnsureContains(b);
        }

        foreach (var (sensorPos, beaconPos) in enumeratedReportings)
        {
            var sensorRange = sensorPos.TaxicabDistance(beaconPos);
            if (Math.Abs(sensorPos.Y - row) > sensorRange)
            {
                continue;
            }

            var dy = Math.Abs(sensorPos.Y - row);
            var dx = sensorRange - dy;
            
            // Only sweep the minimum number of positions
            var sweepStart = new Vector2D(sensorPos.X - dx - 1, row);
            var sweepEnd = new Vector2D(sensorPos.X + dx + 1, row);
            
            var sweepPos = sweepStart;
            var delta = Vector2D.Normalize(sweepEnd - sweepStart);

            while (sweepPos.X <= sweepEnd.X)
            {
                var distanceFromSensor = sensorPos.TaxicabDistance(sweepPos);
                if (distanceFromSensor <= sensorRange && !sensorPositions.Contains(sweepPos) && !beaconPositions.Contains(sweepPos))
                {
                    excludedBeaconPositions.EnsureContains(sweepPos);
                }
                sweepPos += delta;
            }
        }
        
        return excludedBeaconPositions.Count;
    }

    private static IEnumerable<(Vector2D sensorPos, Vector2D beaconPos)> ParseReportings(IEnumerable<string> lines)
    {
        return lines.Select(ParseReporting);
    }

    private static (Vector2D sensorPos, Vector2D beaconPos) ParseReporting(string reporting)
    {
        var matches = Regex.Match(reporting, SensorRegex);
        
        var sensorX = int.Parse(matches.Groups[1].Value);
        var sensorY = int.Parse(matches.Groups[2].Value);
        var beaconX = int.Parse(matches.Groups[3].Value);
        var beaconY = int.Parse(matches.Groups[4].Value);

        return (new Vector2D(sensorX, sensorY), new Vector2D(beaconX, beaconY));
    }
}