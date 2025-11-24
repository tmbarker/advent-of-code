using Utilities.Collections;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2024.D20;

[PuzzleInfo("Race Condition", Topics.Graphs, Difficulty.Medium)]
public sealed class Solution : SolutionBase 
{
    private readonly record struct Cheat(Vec2D Start, Vec2D End)
    {
        public int Cost => Vec2D.Distance(Start, End, Metric.Taxicab);
    }
    
    public override object Run(int part)
    {
        return part switch
        {
            1 => CountCheats(duration: 2),
            2 => CountCheats(duration: 20),
            _ => PuzzleNotSolvedString
        };
    }
    
    private int CountCheats(int duration)
    {
        var grid = GetInputGrid();
        var cost = EvaluateCosts(grid,
            start: grid.Find('S'),
            end:   grid.Find('E'));

        var path = cost.Keys.ToHashSet();
        return path
            .SelectMany(start => GetCheats(path, start, duration))
            .Count(cheat => cost[cheat.Start] + cheat.Cost <= cost[cheat.End] - 100);
    }

    private static IEnumerable<Cheat> GetCheats(HashSet<Vec2D> path, Vec2D start, int duration)
    {
        return path
            .Where(end => Vec2D.Distance(a: start, b: end, Metric.Taxicab) <= duration)
            .Select(end => new Cheat(start, end));
    }
    
    private static DefaultDict<Vec2D, int> EvaluateCosts(Grid2D<char> grid, Vec2D start, Vec2D end)
    {
        var heap = new PriorityQueue<Vec2D, int>([(start, 0)]);
        var cost = new DefaultDict<Vec2D, int>(defaultValue: int.MaxValue, [(start, 0)]);

        while (heap.Count != 0)
        {
            var pos = heap.Dequeue();
            if (pos == end) break;
            
            foreach (var adj in pos.GetAdjacentSet(Metric.Taxicab))
            {
                if (grid[adj] != '#' && cost[pos] + 1 < cost[adj])
                {
                    cost[adj] = cost[pos] + 1;
                    heap.Enqueue(adj, cost[adj]);
                }
            }
        }

        return cost;
    }
}