using Problems.Attributes;
using Problems.Common;
using Problems.Y2022.Common;
using System.Text.RegularExpressions;
using Utilities.Cartesian;

namespace Problems.Y2022.D15;

/// <summary>
/// Beacon Exclusion Zone: https://adventofcode.com/2022/day/15
/// </summary>
[Favourite("Beacon Exclusion Zone", Topics.Vectors, Difficulty.Medium)]
public class Solution : SolutionBase2022
{
    private const int Row = 2000000;
    private const int SearchAreaDimension = 4000000;
    private const long TuningFrequencyMultiplier = SearchAreaDimension;
    
    public override int Day => 15;
    
    public override object Run(int part)
    {
        var reportings = ParseReportings(GetInputLines());
        return part switch
        {
            1 =>  CountBeaconExcludedPositions(reportings),
            2 =>  CalculateTuningFrequency(FindDistressBeacon(reportings)),
            _ => ProblemNotSolvedString
        };
    }

    private static int CountBeaconExcludedPositions(IList<Reporting> reportings)
    {
        var beaconExcludedPositions = new HashSet<Vector2D>();
        var occupiedPositions = new HashSet<Vector2D>();
        
        foreach (var reporting in reportings)
        {
            occupiedPositions.Add(reporting.SensorPos);
            occupiedPositions.Add(reporting.BeaconPos);
        }
        
        foreach (var reporting in reportings)
        {
            var dy = Math.Abs(reporting.SensorPos.Y - Row);   
            if (dy > reporting.Range)
            {
                continue;
            }
            
            // Only sweep the minimum number of positions
            var maxDxInRange = reporting.Range - dy;
            var sweepStart = new Vector2D(reporting.SensorPos.X - maxDxInRange, Row);
            var sweepEnd = new Vector2D(reporting.SensorPos.X + maxDxInRange, Row);
            
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
    
    private static Vector2D FindDistressBeacon(IList<Reporting> reportings)
    {
        foreach (var r1 in reportings)
        {
            foreach (var pos in GetBoundaryPositionsInSearchArea(r1.SensorPos, r1.Range))
            {
                var posInRangeOfSensor = false;
                foreach (var r2 in reportings)
                {
                    if (Vector2D.Distance(r2.SensorPos, pos, DistanceMetric.Taxicab) > r2.Range)
                    {
                        continue;
                    }
                    
                    posInRangeOfSensor = true;
                    break;
                }

                if (!posInRangeOfSensor)
                {
                    return pos;
                }
            }
        }

        throw new NoSolutionException();
    }

    private static HashSet<Vector2D> GetBoundaryPositionsInSearchArea(Vector2D sensorPos, int range)
    {
        var positionSet = new HashSet<Vector2D>();
        var vertices = new List<Vector2D>
        {
            sensorPos + (range + 1) * Vector2D.Up,
            sensorPos + (range + 1) * Vector2D.Right,
            sensorPos + (range + 1) * Vector2D.Down,
            sensorPos + (range + 1) * Vector2D.Left,
        };

        // Trace a square immediately outside of the sensor range by lerping between the vertices of the smallest
        // bounding box (vertices at range + 1)
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
                    positionSet.Add(current);   
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
        var matches = Regex.Matches(reporting, @"-?\d+");
        var sensorX = int.Parse(matches[0].Value);
        var sensorY = int.Parse(matches[1].Value);
        var beaconX = int.Parse(matches[2].Value);
        var beaconY = int.Parse(matches[3].Value);

        return new Reporting(new Vector2D(sensorX, sensorY), new Vector2D(beaconX, beaconY));
    }
}