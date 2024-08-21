using Utilities.Geometry.Euclidean;

namespace Solutions.Y2022.D18;

[PuzzleInfo("Boiling Boulders", Topics.Vectors, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var surfaceVectors = ParseInputLines(parseFunc: Vec3D.Parse);
        return part switch
        {
            1 => ComputeTotalSurfaceArea(surfaceVectors),
            2 => ComputeExteriorSurfaceArea(surfaceVectors),
            _ => PuzzleNotSolvedString
        };
    }

    private static int ComputeTotalSurfaceArea(IEnumerable<Vec3D> elements)
    {
        var totalSurfaceArea = 0;
        var elementsSet = new HashSet<Vec3D>(elements);
        
        foreach (var element in elementsSet)
        {
            var faceAdjacentPositions = element.GetAdjacentSet(Metric.Taxicab);
            totalSurfaceArea += 6 - faceAdjacentPositions.Count(p => elementsSet.Contains(p));
        }

        return totalSurfaceArea;
    }

    private static int ComputeExteriorSurfaceArea(IEnumerable<Vec3D> elements)
    {
        var elementsSet = new HashSet<Vec3D>(elements);
        var boundingSet = new HashSet<Vec3D>();
        var queue = new Queue<Vec3D>();
        var aabb = new Aabb3D(elementsSet, false);

        queue.Enqueue(aabb.Min);
        boundingSet.Add(aabb.Min);
        
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