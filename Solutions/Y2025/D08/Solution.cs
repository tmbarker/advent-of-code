using Utilities.Collections;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2025.D08;

using Pair = (Vec3D A, Vec3D B);

[PuzzleInfo("Playground", Topics.Vectors, Difficulty.Medium, favourite: true)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var boxes = ParseInputLines(Vec3D.Parse);
        var count = boxes.Length * (boxes.Length - 1) / 2;
        var pairs = new PriorityQueue<Pair, long>(initialCapacity: count);
        
        for (var i = 1; i < boxes.Length; i++)
        for (var j = 0; j < i;            j++)
        {
            var a = boxes[i];
            var b = boxes[j];
            pairs.Enqueue(element: (a, b), priority: SqrDistance(a, b));
        }
        
        return part switch
        {
            1 => Part1(boxes, pairs),
            2 => Part2(boxes, pairs),
            _ => PuzzleNotSolvedString
        };
    }
    
    private static long Part1(Vec3D[] boxes, PriorityQueue<Pair, long> pairs)
    {
        var disjointSet = new DisjointSet<Vec3D>(boxes);
        for (var i = 0; i < 1000; i++)
        {
            var (a, b) = pairs.Dequeue();
            disjointSet.Union(a, b);
        }
        
        return disjointSet
            .GetComponents()
            .OrderByDescending(component => component.Count)
            .Take(3)
            .Aggregate(seed: 1L, func: (acc, component) => acc * component.Count);
    }
    
    private static long Part2(Vec3D[] boxes, PriorityQueue<Pair, long> pairs)
    {
        var disjointSet = new DisjointSet<Vec3D>(boxes);
        var pair = default(Pair);
        
        while (disjointSet.ComponentsCount > 1)
        {
            pair = pairs.Dequeue();
            disjointSet.Union(pair.A, pair.B);
        }
        
        return (long)pair.A.X * pair.B.X;
    }

    private static long SqrDistance(Vec3D a, Vec3D b)
    {
        var dx = (long)a.X - b.X;
        var dy = (long)a.Y - b.Y;
        var dz = (long)a.Z - b.Z;
        return dx * dx + dy * dy + dz * dz;
    }
}