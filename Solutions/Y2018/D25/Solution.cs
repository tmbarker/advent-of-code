using Utilities.Collections;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2018.D25;

[PuzzleInfo("Four-Dimensional Adventure", Topics.Vectors, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override int Parts => 1;
    
    public override object Run(int part)
    {
        return part switch
        {
            1 => CountConstellations(),
            _ => PuzzleNotSolvedString
        };
    }

    private int CountConstellations()
    {
        var points = ParseInputLines(parseFunc: Vec4D.Parse);
        var disjointSet = new DisjointSet<Vec4D>();
        var adjacency = new Dictionary<Vec4D, IEnumerable<Vec4D>>();
        
        foreach (var point in points)
        {
            adjacency[point] = points.Where(p => Vec4D.Distance(
                a: p, 
                b: point, 
                metric: Metric.Taxicab) <= 3);
        }
        
        foreach (var (point, adjacencies) in adjacency)
        {
            disjointSet.MakeSet(point);
            foreach (var adjacent in adjacencies)
            {
                disjointSet.MakeSet(adjacent);
                disjointSet.Union(point, adjacent);
            }
        }
        
        return disjointSet.PartitionsCount;
    }
}