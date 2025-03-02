using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2022.D15;

[PuzzleInfo("Beacon Exclusion Zone", Topics.Vectors, Difficulty.Medium, favourite: true)]
public sealed class Solution : SolutionBase
{
    private const int Row = 2000000;
    private const int SearchAreaDimension = 4000000;
    private const long TuningFrequencyMultiplier = SearchAreaDimension;

    public override object Run(int part)
    {
        var input = GetInputLines();
        var reportings = input.Select(ParseReporting).ToList();
        
        return part switch
        {
            1 => CountBeaconExcludedPositions(reportings),
            2 => CalculateTuningFrequency(FindDistressBeacon(reportings)),
            _ => PuzzleNotSolvedString
        };
    }

    private static int CountBeaconExcludedPositions(IList<Reporting> reportings)
    {
        var beaconExcludedPositions = new HashSet<Vec2D>();
        var occupiedPositions = new HashSet<Vec2D>();
        
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
            
            //  Only sweep the minimum number of positions
            //
            var maxDxInRange = reporting.Range - dy;
            var sweepStart = new Vec2D(X: reporting.SensorPos.X - maxDxInRange, Y: Row);
            var sweepEnd = new Vec2D(X: reporting.SensorPos.X + maxDxInRange, Y: Row);
            
            var sweepPos = sweepStart;
            while (sweepPos.X <= sweepEnd.X)
            {
                if (!occupiedPositions.Contains(sweepPos))
                {
                    beaconExcludedPositions.Add(sweepPos);
                }
                sweepPos += Vec2D.Right;
            }
        }
        
        return beaconExcludedPositions.Count;
    }

    private static long CalculateTuningFrequency(Vec2D beaconPos)
    {
        return beaconPos.X * TuningFrequencyMultiplier + beaconPos.Y;
    }
    
    private static Vec2D FindDistressBeacon(IList<Reporting> reportings)
    {
        foreach (var r1 in reportings)
        foreach (var pos in GetBoundaryPositionsInSearchArea(r1.SensorPos, r1.Range))
        {
            var posInRangeOfSensor = false;
            foreach (var r2 in reportings)
            {
                if (Vec2D.Distance(r2.SensorPos, pos, Metric.Taxicab) <= r2.Range)
                {
                    posInRangeOfSensor = true;
                    break;
                }
            }

            if (!posInRangeOfSensor)
            {
                return pos;
            }
        }

        throw new NoSolutionException();
    }

    private static HashSet<Vec2D> GetBoundaryPositionsInSearchArea(Vec2D sensorPos, int range)
    {
        var positionSet = new HashSet<Vec2D>();
        var vertices = new List<Vec2D>
        {
            sensorPos + (range + 1) * Vec2D.Up,
            sensorPos + (range + 1) * Vec2D.Right,
            sensorPos + (range + 1) * Vec2D.Down,
            sensorPos + (range + 1) * Vec2D.Left
        };

        //  Trace a square immediately outside of the sensor range by lerping between the vertices of the smallest
        //  bounding box (vertices at range + 1)
        //
        for (var i = 0; i < vertices.Count - 1; i++)
        {
            var fromVertex = vertices[i];
            var toVertex = vertices[i + 1];
            var step = Vec2D.Normalize(toVertex - fromVertex);
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

    private static bool IsPositionInSearchArea(Vec2D pos)
    {
        return pos is { X: >= 0, Y: >= 0 } and { X: <= SearchAreaDimension, Y: <= SearchAreaDimension };
    }

    private static Reporting ParseReporting(string reporting)
    {
        var numbers = reporting.ParseInts();
        var sensorX = numbers[0];
        var sensorY = numbers[1];
        var beaconX = numbers[2];
        var beaconY = numbers[3];

        return new Reporting(
            sensorPos: new Vec2D(sensorX, sensorY),
            beaconPos: new Vec2D(beaconX, beaconY));
    }
}