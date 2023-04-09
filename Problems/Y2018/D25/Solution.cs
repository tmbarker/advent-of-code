using Problems.Y2018.Common;
using Utilities.Cartesian;
using Utilities.Collections;
using Utilities.Extensions;

namespace Problems.Y2018.D25;

/// <summary>
/// Four-Dimensional Adventure: https://adventofcode.com/2018/day/25
/// </summary>
public class Solution : SolutionBase2018
{
    public override int Day => 25;
    public override int Parts => 1;
    
    public override object Run(int part)
    {
        return part switch
        {
            1 => CountConstellations(),
            _ => ProblemNotSolvedString
        };
    }

    private int CountConstellations()
    {
        var points = ParseInputLines(parseFunc: ParsePoint).ToList();
        var disjointSet = new DisjointSet<Vector4D>();
        var adjacency = new Dictionary<Vector4D, IEnumerable<Vector4D>>();
        
        foreach (var point in points)
        {
            adjacency[point] = points.Where(p => Vector4D.Distance(
                a: p, 
                b: point, 
                metric: DistanceMetric.Taxicab) <= 3);
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
        
        return disjointSet.SetsCount;
    }

    private static Vector4D ParsePoint(string line)
    {
        var numbers = line.ParseInts();
        return new Vector4D(
            x: numbers[0],
            y: numbers[1],
            z: numbers[2],
            w: numbers[3]);
    }
}