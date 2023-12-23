using Utilities.Collections;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2023.D23;

using Graph = DefaultDict<Vector2D, HashSet<Vector2D>>;

[PuzzleInfo("A Long Walk", Topics.Graphs, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private const char Forest = '#';
    private static readonly Dictionary<char, Vector2D> SlopeMap = new()
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

        return Search(graph, start, end);
    }

    private static int Search(Graph graph, Vector2D start, Vector2D end)
    {
        return Dfs(graph, goal: end, pos: start, visited: [], n: 0);
    }
    
    private static int Dfs(Graph graph, Vector2D goal, Vector2D pos, HashSet<Vector2D> visited, int n)
    {
        if (pos == goal)
        {
            return n;
        }
     
        var max = 0;
        foreach (var adj in graph[pos])
        {
            if (visited.Add(adj))
            {
                max = Math.Max(max, Dfs(graph, goal, adj, visited, n: n + 1));
                visited.Remove(adj);
            }
        }
        return max;
    }

    private static Graph ParseGraph(IList<string> input, bool slopes, out Vector2D start, out Vector2D end)
    {
        start = new Vector2D(x: input[0].IndexOf('.'), y: input.Count - 1);
        end = new Vector2D(x: input[^1].IndexOf('.'), y: 0);
        
        var grid = Grid2D<char>.MapChars(input);
        var graph = new Graph(defaultSelector: _ => []);

        foreach (var pos in grid)
        {
            graph[pos] = GetAdjacent(grid, pos, slopes);
        }

        return graph;
    }
    
    private static HashSet<Vector2D> GetAdjacent(Grid2D<char> grid, Vector2D pos, bool slopes)
    {
        var candidates = new List<Vector2D>(capacity: 4);
        if (slopes && SlopeMap.TryGetValue(grid[pos], out var dir))
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