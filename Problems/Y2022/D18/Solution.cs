using Problems.Y2022.Common;
using Utilities.Cartesian;

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
        var aabb = new Aabb3D(elementsSet, false);

        queue.Enqueue(aabb.GetMinVertex());
        boundingSet.Add(aabb.GetMinVertex());
        
        while (queue.Count > 0)
        {
            foreach (var adj in queue.Dequeue().GetAdjacentSet(DistanceMetric.Taxicab))
            {
                if (elementsSet.Contains(adj) || boundingSet.Contains(adj) || !aabb.Contains(adj, true))
                {
                    continue;
                }
                
                boundingSet.Add(adj);
                queue.Enqueue(adj);
            }
        }

        return (int)(ComputeTotalSurfaceArea(boundingSet) - aabb.GetSurfaceArea());
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