using Utilities.Geometry.Euclidean;

namespace Solutions.Y2022.D12;

[PuzzleInfo("Hill Climbing Algorithm", Topics.Graphs, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private const char MinHeight = 'a';
    private const char StartHeight = MinHeight;
    private const char StartMarker = 'S';
    private const char EndHeight = 'z';
    private const char EndMarker = 'E';
    
    public override object Run(int part)
    {
        var lines = GetInputLines();
        var map = ParseGrid(lines, out var start, out var end);
        
        return part switch
        {
            1 => GetMinDistance(map, start, end),
            2 => GetFewestStepsFromMinHeight(map, end),
            _ => ProblemNotSolvedString
        };
    }

    private static int GetFewestStepsFromMinHeight(Grid2D<char> map, Vector2D end)
    {
        return map
            .Where(pos => map[pos] == MinHeight)
            .Min(start => GetMinDistance(map, start, end));
    }

    private static int GetMinDistance(Grid2D<char> map, Vector2D start, Vector2D end)
    {
        var queue = new Queue<Vector2D>(new[] { start });
        var visited = new HashSet<Vector2D>(new[] { start });
        var depth = 0;
        
        while (queue.Count > 0)
        {
            var nodesAtDepth = queue.Count;
            while (nodesAtDepth-- > 0)
            {
                var pos = queue.Dequeue();
                if (pos == end)
                {
                    return depth;
                }

                var adjacent = pos
                    .GetAdjacentSet(Metric.Taxicab)
                    .Where(map.Contains)
                    .Where(adj => map[adj] - map[pos] <= 1)
                    .Where(adj => !visited.Contains(adj));
                
                foreach (var adj in adjacent)
                {
                    visited.Add(adj);
                    queue.Enqueue(adj);
                }
            }

            depth++;
        }

        return int.MaxValue;
    }
    
    private static Grid2D<char> ParseGrid(IList<string> lines, out Vector2D start, out Vector2D end)
    {
        var grid = Grid2D<char>.MapChars(lines);
        
        start = grid.Single(pos => grid[pos] == StartMarker);
        end = grid.Single(pos => grid[pos] == EndMarker);
        
        grid[start] = StartHeight;
        grid[end] = EndHeight;
        
        return grid;
    }
}