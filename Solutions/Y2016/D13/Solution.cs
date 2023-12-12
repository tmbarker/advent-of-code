using Utilities.Geometry.Euclidean;

namespace Solutions.Y2016.D13;

[PuzzleInfo("A Maze of Twisty Little Cubicles", Topics.Graphs | Topics.BitwiseOperations, Difficulty.Hard)]
public sealed class Solution : SolutionBase
{
    private static readonly Vector2D Start = new (x: 1, y: 1);
    private static readonly Vector2D Target = new (x: 31, y: 39);
    
    public override object Run(int part)
    {
        var input = GetInputText();
        var favourite = int.Parse(input);

        return part switch
        {
            1 => Search(Start, Target, favourite),
            2 => Explore(Start, favourite, maxDepth: 50),
            _ => ProblemNotSolvedString
        };
    }

    private static int Search(Vector2D start, Vector2D target, int favourite)
    {
        var queue = new Queue<Vector2D>([start]);
        var visited = new HashSet<Vector2D>([start]);
        var depth = 0;

        while (queue.Count > 0)
        {
            var nodesAtDepth = queue.Count;
            while (nodesAtDepth-- > 0)
            {
                var pos = queue.Dequeue();
                if (pos == target)
                {
                    return depth;
                }

                var adjacent = GetAdjacentOpen(pos, favourite);
                var unvisited = adjacent.Where(adj => !visited.Contains(adj));

                foreach (var adj in unvisited)
                {
                    visited.Add(adj);
                    queue.Enqueue(adj);
                }
            }

            depth++;
        }

        throw new NoSolutionException();
    }
    
    private static int Explore(Vector2D start, int favourite, int maxDepth)
    {
        var queue = new Queue<Vector2D>(new[] { start });
        var visited = new HashSet<Vector2D>(new[] { start });
        var depth = 0;

        while (queue.Count > 0 && depth < maxDepth)
        {
            var nodesAtDepth = queue.Count;
            while (nodesAtDepth-- > 0)
            {
                var pos = queue.Dequeue();
                var adjacent = GetAdjacentOpen(pos, favourite);
                var unvisited = adjacent.Where(adj => !visited.Contains(adj));

                foreach (var adj in unvisited)
                {
                    visited.Add(adj);
                    queue.Enqueue(adj);
                }
            }

            depth++;
        }

        return visited.Count;
    }

    private static IEnumerable<Vector2D> GetAdjacentOpen(Vector2D pos, int favourite)
    {
        return pos
            .GetAdjacentSet(Metric.Taxicab)
            .Where(adj => adj is { X: >= 0, Y: >= 0 })
            .Where(adj => IsOpen(adj, favourite));
    }

    private static bool IsOpen(Vector2D pos, int favourite)
    {
        var raw = pos.X * pos.X + 3 * pos.X + 2 * pos.X * pos.Y + pos.Y + pos.Y * pos.Y;
        var sum = raw + favourite;
        var set = CountSetBits(sum);

        return set % 2 == 0;
    }

    private static int CountSetBits(int n)
    {
        var set = 0;
        while (n > 0)
        {
            if (n % 2 > 0)
            {
                set++;
            }
            n /= 2;
        }

        return set;
    }
}