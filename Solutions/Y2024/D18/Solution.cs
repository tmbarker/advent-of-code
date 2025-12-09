using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2024.D18;

[PuzzleInfo("RAM Run", Topics.Graphs, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var bytes = ParseInputLines(Vec2D.Parse);
        return part switch
        {
            1 => Navigate(bytes),
            2 => FindImpasse(bytes),
            _ => PuzzleNotSolvedString
        };
    }

    private static int Navigate(Vec2D[] bytes)
    {
        return TryNavigate(bytes[..1024], out var steps)
            ? steps
            : throw new NoSolutionException();
    }

    private static string FindImpasse(Vec2D[] bytes)
    {
        var result = -1;
        var left = 1025;
        var right = bytes.Length - 1;

        while (left <= right)
        {
            var mid = left + (right - left) / 2;
            if (!TryNavigate(bytes[..(mid + 1)], out _))
            {
                result = mid;
                right = mid - 1;
            }
            else
            {
                left = mid + 1;
            }
        }

        var impasse = bytes[result];
        return $"{impasse.X},{impasse.Y}";
    }
    
    private static bool TryNavigate(Vec2D[] bytes, out int steps)
    {
        var exit = new Vec2D(X: 70, Y: 70);
        var aabb = new Aabb2D(v1: Vec2D.Zero, v2: exit);
        
        var queue = new Queue<Vec2D>([Vec2D.Zero]);
        var visited = new HashSet<Vec2D>([Vec2D.Zero]);
        var corrupt = bytes.ToHashSet();

        steps = 0;
        while (queue.Count != 0)
        {
            var heads = queue.Count;
            while (heads-- > 0)
            {
                var pos = queue.Dequeue();
                if (pos == exit) return true;

                pos
                    .GetAdjacentSet(Metric.Taxicab)
                    .Where(adj => aabb.Contains(adj) && !corrupt.Contains(adj))
                    .Where(visited.Add)
                    .ForEach(queue.Enqueue);
            }
            steps++;
        }
        
        return false;
    }
}