using Utilities.Collections;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2023.D23;

using Graph = DefaultDict<Vector2D, HashSet<(Vector2D Adj, int Cost)>>;

[PuzzleInfo("A Long Walk", Topics.Graphs, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private const char Forest = '#';
    private static readonly Dictionary<char, Vector2D> Slopes = new()
    {
        { '^', Vector2D.Up },
        { 'v', Vector2D.Down },
        { '<', Vector2D.Left },
        { '>', Vector2D.Right }
    };
    
    public override object Run(int part)
    {
        var input = GetInputLines();
        var graph = ParseGraph(input, slopes: part == 1, out var start, out var end);
        var delta = 0;

        if (graph[end].Count == 1)
        {
            var adj = graph[end].Single();
            delta = adj.Cost;
            end = adj.Adj;
        }

        return Dfs(graph, goal: end, pos: start, visited: [], n: 0) + delta;
    }
    
    private static int Dfs(Graph graph, Vector2D goal, Vector2D pos, HashSet<Vector2D> visited, int n)
    {
        if (pos == goal)
        {
            return n;
        }
     
        var max = 0;
        foreach (var (adj, cost) in graph[pos])
        {
            if (visited.Add(adj))
            {
                max = Math.Max(max, Dfs(graph, goal, adj, visited, n: n + cost));
                visited.Remove(adj);
            }
        }
        return max;
    }

    private static Graph ParseGraph(IList<string> input, bool slopes, out Vector2D start, out Vector2D end)
    {
        start = new Vector2D(x: input[0].IndexOf('.'),  y: input.Count - 1);
        end =   new Vector2D(x: input[^1].IndexOf('.'), y: 0);
        
        var grid = Grid2D<char>.MapChars(input);
        var graph = new Graph(defaultSelector: _ => []);
        var nodes = grid
            .Where(pos => grid[pos] != Forest)
            .Where(pos => !slopes || !Slopes.ContainsKey(grid[pos]))
            .Where(pos => GetAdjacent(grid, pos, slopes).Count != 2)
            .ToHashSet();

        foreach (var node in nodes)
        {
            var queue = new Queue<Vector2D>(collection: [node]);
            var visited = new HashSet<Vector2D>(collection: [node]);
            var cost = 0;

            while (queue.Count > 0)
            {
                var heads = queue.Count;
                while (heads-- > 0)
                {
                    var head = queue.Dequeue();
                    if (head != node && nodes.Contains(head))
                    {
                        graph[node].Add((Adj: head, Cost: cost));
                        continue;
                    }

                    foreach (var adj in GetAdjacent(grid, head, slopes))
                    {
                        if (visited.Add(adj))
                        {
                            queue.Enqueue(adj);   
                        }
                    }
                }
                cost++;
            }
        }

        return graph;
    }
    
    private static HashSet<Vector2D> GetAdjacent(Grid2D<char> grid, Vector2D pos, bool slopes)
    {
        var candidates = new List<Vector2D>(capacity: 4);
        if (slopes && Slopes.TryGetValue(grid[pos], out var dir))
        {
            candidates.Add(item: pos + dir);
        }
        else
        {
            candidates.AddRange(collection: pos.GetAdjacentSet(Metric.Taxicab));
        }
        return candidates
            .Where(adj => grid.Contains(adj) && grid[adj] != Forest)
            .ToHashSet();
    }
}