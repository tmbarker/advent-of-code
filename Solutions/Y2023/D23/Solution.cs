using Utilities.Collections;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2023.D23;

using Pos   = Vector2D;
using Graph = DefaultDict<Vector2D, HashSet<(Vector2D Adj, int Cost)>>;

[PuzzleInfo("A Long Walk", Topics.Graphs, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private const char Forest = '#';
    private static readonly Dictionary<char, Pos> Slopes = new()
    {
        { '^', Pos.Up },
        { 'v', Pos.Down },
        { '<', Pos.Left },
        { '>', Pos.Right }
    };
    
    public override object Run(int part)
    {
        var input = GetInputLines();
        var graph = ParseGraph(input, slopes: part == 1, out var start, out var end);
        var delta = 0;

        if (graph[end].Count == 1)
        {
            (end, delta) = graph[end].Single();
        }

        return Dfs(graph, goal: end, pos: start, visited: [], max: 0, n: 0) + delta;
    }
    
    private static int Dfs(Graph graph, Pos goal, Pos pos, HashSet<Pos> visited, int max, int n)
    {
        if (pos == goal)
        {
            return n;
        }
        
        foreach (var (adj, cost) in graph[pos])
        {
            if (visited.Add(adj))
            {
                max = Math.Max(max, Dfs(graph, goal, adj, visited, max, n: n + cost));
                visited.Remove(adj);
            }
        }
        
        return max;
    }

    private static Graph ParseGraph(IList<string> input, bool slopes, out Pos start, out Pos end)
    {
        start = new Pos(x: input[0].IndexOf('.'),  y: input.Count - 1);
        end =   new Pos(x: input[^1].IndexOf('.'), y: 0);
        
        var grid = Grid2D<char>.MapChars(input);
        var graph = new Graph(defaultSelector: _ => []);
        var nodes = grid
            .Where(pos => grid[pos] != Forest)
            .Where(pos => !slopes || !Slopes.ContainsKey(grid[pos]))
            .Where(pos => GetAdjacent(grid, pos, slopes).Count != 2)
            .ToHashSet();

        foreach (var node in nodes)
        {
            var queue = new Queue<Pos>(collection: [node]);
            var visited = new HashSet<Pos>(collection: [node]);
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

                    foreach (var adj in GetAdjacent(grid, head, slopes).Where(adj => visited.Add(adj)))
                    {
                        queue.Enqueue(adj);
                    }
                }
                cost++;
            }
        }

        return graph;
    }
    
    private static HashSet<Pos> GetAdjacent(Grid2D<char> grid, Pos pos, bool slopes)
    {
        var candidates = new List<Pos>(capacity: 4);
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