using Problems.Common;
using Utilities.Geometry.Euclidean;

namespace Problems.Y2022.D18;

/// <summary>
/// Boiling Boulders: https://adventofcode.com/2022/day/18
/// </summary>
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var surfaceVectors = ParseInputLines(parseFunc: Vector3D.Parse);
        return part switch
        {
            1 => ComputeTotalSurfaceArea(surfaceVectors),
            2 => ComputeExteriorSurfaceArea(surfaceVectors),
            _ => ProblemNotSolvedString
        };
    }

    private static int ComputeTotalSurfaceArea(IEnumerable<Vector3D> elements)
    {
        var totalSurfaceArea = 0;
        var elementsSet = new HashSet<Vector3D>(elements);
        
        foreach (var element in elementsSet)
        {
            var faceAdjacentPositions = element.GetAdjacentSet(Metric.Taxicab);
            totalSurfaceArea += 6 - faceAdjacentPositions.Count(p => elementsSet.Contains(p));
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
            foreach (var adj in queue.Dequeue().GetAdjacentSet(Metric.Taxicab))
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
}