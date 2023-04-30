using Problems.Common;
using Utilities.Cartesian;

namespace Problems.Y2022.D12;

/// <summary>
/// Hill Climbing Algorithm: https://adventofcode.com/2022/day/12
/// </summary>
public class Solution : SolutionBase
{
    private const char MinHeight = 'a';
    private const char StartHeight = MinHeight;
    private const char StartMarker = 'S';
    private const char EndHeight = 'z';
    private const char EndMarker = 'E';
    
    public override object Run(int part)
    {
        ParseGrid(out var map, out var start, out var end);
        
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
            .GetAllPositions()
            .Where(pos => map[pos] == MinHeight)
            .Min(start => GetMinDistance(map, start, end));
    }

    private static int GetMinDistance(Grid2D<char> map, Vector2D start, Vector2D end)
    {
        var queue = new Queue<Vector2D>(new[] { start });
        var visited = new HashSet<Vector2D>(new[] { start });
        var depth = 0;
        
        while (queue.Any())
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
                    .Where(map.IsInDomain)
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
    
    private void ParseGrid(out Grid2D<char> grid, out Vector2D start, out Vector2D end)
    {
        grid = Grid2D<char>.MapChars(GetInputLines(), c => c);
        start = grid.Single(kvp => kvp.Value == StartMarker).Key;
        end = grid.Single(kvp => kvp.Value == EndMarker).Key;

        grid[start] = StartHeight;
        grid[end] = EndHeight;
    }
}