using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2024.D10;

[PuzzleInfo("Hoof It", Topics.Graphs|Topics.Vectors, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    private readonly record struct State(Vec2D Pos, string Path);
    
    public override object Run(int part)
    {
        return part switch
        {
            1 => Evaluate(evalFunc: paths => paths.DistinctBy(path => path.Pos).Count()),
            2 => Evaluate(evalFunc: paths => paths.Count),
            _ => PuzzleNotSolvedString
        };
    }
    
    private int Evaluate(Func<HashSet<State>, int> evalFunc)
    {
        var input = GetInputLines();
        var grid = Grid2D<int>.MapChars(input, c => c.AsDigit());

        return grid
            .Where(pos => grid[pos] == 0)
            .Select(trailhead => CollectPaths(grid, trailhead))
            .Sum(evalFunc);
    }
    
    private static HashSet<State> CollectPaths(Grid2D<int> grid, Vec2D trailhead)
    {
        var paths = new HashSet<State>();
        var queue = new Queue<State>(collection: [new State(Pos: trailhead, Path: $"{trailhead}")]);

        while (queue.Count != 0)
        {
            var state = queue.Dequeue();
            if (grid[state.Pos] == 9)
            {
                paths.Add(state);
                continue;
            }

            foreach (var adj in state.Pos.GetAdjacentSet(Metric.Taxicab))
            {
                if (grid.Contains(adj) && grid[adj] == grid[state.Pos] + 1)
                {
                    queue.Enqueue(new State(Pos: adj, Path: $"{state.Path}{adj}"));
                }
            }
        }

        return paths;
    }
}