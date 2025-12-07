using Utilities.Geometry.Euclidean;

namespace Solutions.Y2025.D07;

[PuzzleInfo("Laboratories", Topics.Graphs, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var grid = GetInputGrid();
        var start = grid.Find('S');
        
        return part switch
        {
            1 => Part1(grid, start),
            2 => Part2(grid, start),
            _ => PuzzleNotSolvedString
        };
    }
    
    private static long Part1(Grid2D<char> grid, Vec2D start)
    {
        var queue = new Queue<Vec2D>([start]);
        var visited = new HashSet<Vec2D>([start]);
        var count = 0;

        while (queue.Count > 0)
        {
            var pos = queue.Dequeue();
            var below = pos + Vec2D.Down;

            if (!grid.Contains(below))
            {
                continue;
            }
            
            switch (grid[below])
            {
                case '.':
                    EnqueueIfNew(below);
                    break;
                case '^':
                    count++;
                    EnqueueIfNew(below + Vec2D.Left);
                    EnqueueIfNew(below + Vec2D.Right);
                    break;
            }
        }
        
        return count;

        void EnqueueIfNew(Vec2D pos)
        {
            if (visited.Add(pos))
            {
                queue.Enqueue(pos);
            }
        }
    }
    
    private static long Part2(Grid2D<char> grid, Vec2D start)
    {
        return CountTimelines(grid, pos: start, memo: []);
    }

    private static long CountTimelines(Grid2D<char> grid, Vec2D pos, Dictionary<Vec2D, long> memo)
    {
        if (memo.TryGetValue(pos, out var cached))
        {
            return cached;
        }
        
        var below = pos + Vec2D.Down;
        if (!grid.Contains(below))
        {
            memo[pos] = 1L;
            return memo[pos];
        }

        switch (grid[below])
        {
            case '.':
                memo[pos] = CountTimelines(grid, below, memo);
                return memo[pos];
            case '^':
                memo[pos] = CountTimelines(grid, below + Vec2D.Left, memo) + CountTimelines(grid, below + Vec2D.Right, memo);
                return memo[pos];
            default:
                throw new NoSolutionException("Unreachable");
        }
    }
}