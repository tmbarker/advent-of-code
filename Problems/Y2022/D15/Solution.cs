using System.Text.RegularExpressions;
using Problems.Common;
using Problems.Y2022.Common;
using Utilities.DataStructures.Cartesian;
using Utilities.Extensions;

namespace Problems.Y2022.D15;

/// <summary>
/// Beacon Exclusion Zone: https://adventofcode.com/2022/day/15
/// </summary>
public class Solution : SolutionBase2022
{
    private const string SensorRegex = @".*x=(-?\d+).*y=(-?\d+).*x=(-?\d+).*y=(-?\d+)";
    private const int Row = 2000000;
    private const int SearchAreaDimension = 4000000;
    private const long TuningFrequencyMultiplier = SearchAreaDimension;
    
    public override int Day => 15;
    
    public override object Run(int part)
    {
        var reportings = ParseReportings(GetInputLines());
        return part switch
        {
            0 => CountBeaconExcludedPositions(reportings),
            1 => CalculateTuningFrequency(FindDistressBeacon(reportings)),
            _ => ProblemNotSolvedString,
        };
    }

    private static int CountBeaconExcludedPositions(IList<Reporting> reportings)
    {
        var beaconExcludedPositions = new HashSet<Vector2D>();
        var occupiedPositions = new HashSet<Vector2D>();
        
        foreach (var reporting in reportings)
        {
            occupiedPositions.EnsureContains(reporting.SensorPos);
            occupiedPositions.EnsureContains(reporting.BeaconPos);
        }
        
        foreach (var reporting in reportings)
        {
            var beaconPos = reporting.BeaconPos;
            var sensorPos = reporting.SensorPos;
            var sensorRange = Vector2D.Distance(sensorPos, beaconPos, DistanceMetric.Taxicab);
            
            if (Math.Abs(sensorPos.Y - Row) > sensorRange)
            {
                continue;
            }

            var dy = Math.Abs(sensorPos.Y - Row);
            var maxDxInRange = sensorRange - dy;

            // Only sweep the minimum number of positions
            var sweepStart = new Vector2D(sensorPos.X - maxDxInRange, Row);
            var sweepEnd = new Vector2D(sensorPos.X + maxDxInRange, Row);
            var sweepPos = sweepStart;

            while (sweepPos.X <= sweepEnd.X)
            {
                if (!occupiedPositions.Contains(sweepPos))
                {
                    beaconExcludedPositions.Add(sweepPos);
                }
                sweepPos += Vector2D.Right;
            }
        }
        
        return beaconExcludedPositions.Count;
    }

    private static long CalculateTuningFrequency(Vector2D beaconPos)
    {
        return beaconPos.X * TuningFrequencyMultiplier + beaconPos.Y;
    }
    
    private static Vector2D FindDistressBeacon(IEnumerable<Reporting> reportings)
    {
        var sensorRangeTuples = reportings.Select(r => 
        {
            var sensorPos = r.SensorPos;
            var range = Vector2D.Distance(sensorPos, r.BeaconPos, DistanceMetric.Taxicab);
            return (sensorPos, range);
        }).ToList();
        
        for (var i = 0; i < sensorRangeTuples.Count; i++)
        {
            var sensorPos = sensorRangeTuples[i].sensorPos;
            var sensorRange = sensorRangeTuples[i].range;
            var outOfRangePerimeterPositions = GetPerimeterPositionsInSearchArea(sensorPos, sensorRange);
            
            foreach (var searchPos in outOfRangePerimeterPositions)
            {
                var searchPosInRangeOfSensor = false;
                for (var j = 0; j < sensorRangeTuples.Count; j++)
                {
                    if (Vector2D.Distance(sensorRangeTuples[j].sensorPos, searchPos, DistanceMetric.Taxicab) > sensorRangeTuples[j].range)
                    {
                        continue;
                    }
                    
                    searchPosInRangeOfSensor = true;
                    break;
                }

                if (!searchPosInRangeOfSensor)
                {
                    return searchPos;
                }
            }
        }

        throw new NoSolutionException();
    }

    private static HashSet<Vector2D> GetPerimeterPositionsInSearchArea(Vector2D sensorPos, int range)
    {
        var positionSet = new HashSet<Vector2D>();
        var distance = range + 1;
        
        var vertices = new List<Vector2D>
        {
            sensorPos + distance * Vector2D.Up,
            sensorPos + distance * Vector2D.Right,
            sensorPos + distance * Vector2D.Down,
            sensorPos + distance * Vector2D.Left,
        };

        for (var i = 0; i < vertices.Count - 1; i++)
        {
            var fromVertex = vertices[i];
            var toVertex = vertices[i + 1];
            var step = Vector2D.Normalize(toVertex - fromVertex);

            var current = fromVertex;
            while (current != toVertex + step)
            {
                if (IsPositionInSearchArea(current))
                {
                    positionSet.EnsureContains(current);   
                }
                current += step;
            }
        }

        return positionSet;
    }

    private static bool IsPositionInSearchArea(Vector2D pos)
    {
        return pos is { X: >= 0, Y: >= 0 } and { X: <= SearchAreaDimension, Y: <= SearchAreaDimension };
    }

    private static IList<Reporting> ParseReportings(IEnumerable<string> lines)
    {
        return lines.Select(ParseReporting).ToList();
    }

    private static Reporting ParseReporting(string reporting)
    {
        var matches = Regex.Match(reporting, SensorRegex);
        
        var sensorX = int.Parse(matches.Groups[1].Value);
        var sensorY = int.Parse(matches.Groups[2].Value);
        var beaconX = int.Parse(matches.Groups[3].Value);
        var beaconY = int.Parse(matches.Groups[4].Value);

        return new Reporting(new Vector2D(sensorX, sensorY), new Vector2D(beaconX, beaconY));
    }
}