using Problems.Y2022.Common;
using Utilities.DataStructures.Cartesian;

namespace Problems.Y2022.D18;

/// <summary>
/// Boiling Boulders: https://adventofcode.com/2022/day/18
/// </summary>
public class Solution : SolutionBase2022
{
    private const char Delimiter = ',';
    private const int CubeFaces = 6;

    public override int Day => 18;
    
    public override object Run(int part)
    {
        var surfaceVectors = ParseSurfaceVectors(GetInputLines());
        return part switch
        {
            0 => ComputeTotalSurfaceArea(surfaceVectors),
            1 => ComputeExteriorSurfaceArea(surfaceVectors),
            _ => ProblemNotSolvedString,
        };
    }

    private static int ComputeTotalSurfaceArea(IEnumerable<Vector3D> elements)
    {
        var totalSurfaceArea = 0;
        var elementsSet = new HashSet<Vector3D>(elements);
        
        foreach (var element in elementsSet)
        {
            var faceAdjacentPositions = element.GetAdjacentSet(DistanceMetric.Taxicab);
            totalSurfaceArea += CubeFaces - faceAdjacentPositions.Count(p => elementsSet.Contains(p));
        }

        return totalSurfaceArea;
    }

    private static int ComputeExteriorSurfaceArea(IEnumerable<Vector3D> elements)
    {
        var elementsSet = new HashSet<Vector3D>(elements);
        var boundingSet = new HashSet<Vector3D>();
        var queue = new Queue<Vector3D>();
        
        var lBound = new Vector3D(
            x: elementsSet.Min(v => v.X) - 1,
            y: elementsSet.Min(v => v.Y) - 1,
            z: elementsSet.Min(v => v.Z) - 1);
        var uBound = new Vector3D(
            x: elementsSet.Max(v => v.X) + 1,
            y: elementsSet.Max(v => v.Y) + 1,
            z: elementsSet.Max(v => v.Z) + 1);

        queue.Enqueue(uBound);
        boundingSet.Add(uBound);
        
        while (queue.Count > 0)
        {
            foreach (var adj in queue.Dequeue().GetAdjacentSet(DistanceMetric.Taxicab))
            {
                if (elementsSet.Contains(adj) || boundingSet.Contains(adj) || !IsBoundedInclusive(adj, lBound, uBound))
                {
                    continue;
                }
                
                boundingSet.Add(adj);
                queue.Enqueue(adj);
            }
        }

        var totalBoundingSurfaceArea = ComputeTotalSurfaceArea(boundingSet);
        var exteriorBoundingSurfaceArea = GetRectPrismSurfaceArea(lBound, uBound);
        var exteriorSurfaceArea = totalBoundingSurfaceArea - exteriorBoundingSurfaceArea;

        return exteriorSurfaceArea;
    }

    private static int GetRectPrismSurfaceArea(Vector3D lBound, Vector3D uBound)
    {
        var l = Math.Abs(uBound.X - lBound.X) + 1;
        var h = Math.Abs(uBound.Y - lBound.Y) + 1;
        var w = Math.Abs(uBound.Z - lBound.Z) + 1;

        return 2 * (w * l + h * l + h * w);
    }

    private static bool IsBoundedInclusive(Vector3D pos, Vector3D lBound, Vector3D uBound)
    {
        return
            pos.X >= lBound.X && pos.X <= uBound.X &&
            pos.Y >= lBound.Y && pos.Y <= uBound.Y &&
            pos.Z >= lBound.Z && pos.Z <= uBound.Z;
    }

    private static IEnumerable<Vector3D> ParseSurfaceVectors(IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            var elements = line.Split(Delimiter);
            var x = int.Parse(elements[0]);
            var y = int.Parse(elements[1]);
            var z = int.Parse(elements[2]);
            
            yield return new Vector3D(x, y, z);
        }
    }
}