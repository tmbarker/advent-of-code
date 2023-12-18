using Utilities.Collections;
using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2023.D17;

[PuzzleInfo("Clumsy Crucible", Topics.Graphs, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private readonly record struct State(Vector2D Pos, Vector2D Face, int Count);
    
    public override object Run(int part)
    {
        return part switch
        {
            1 => Search(stepRange: new Range<int>(min: 0, max: 3)),
            2 => Search(stepRange: new Range<int>(min: 4, max: 10)),
            _ => ProblemNotSolvedString
        };
    }
    
    private int Search(Range<int> stepRange)
    {
        var grid = Grid2D<int>.MapChars(strings: GetInputLines(), elementFunc: c => c.AsDigit());
        var start = new Vector2D(x: 0, y: grid.Height - 1);
        var end = new Vector2D(x: grid.Width - 1, y: 0);
        
        var cost = new DefaultDict<State, int>(defaultValue: int.MaxValue / 2);
        var heap = new PriorityQueue<State, int>();
        
        foreach (var init in GetInitial(start))
        {
            cost[init] = 0;
            heap.Enqueue(init, priority: 0);
        }

        while (heap.Count > 0)
        {
            var cur = heap.Dequeue();
            if (cur.Pos == end && cur.Count >= stepRange.Min)
            {
                return cost[cur];
            }
            
            foreach (var adj in GetAdjacent(cur, stepRange))
            {
                if (grid.Contains(adj.Pos) && cost[cur] + grid[adj.Pos] < cost[adj])
                {
                    cost[adj] = cost[cur] + grid[adj.Pos];
                    heap.Enqueue(adj, cost[adj]);
                }
            }
        }

        throw new NoSolutionException();
    }
    
    private static IEnumerable<State> GetInitial(Vector2D origin)
    {
        yield return new State(Pos: origin, Face: Vector2D.Right, Count: 0);
        yield return new State(Pos: origin, Face: Vector2D.Down,  Count: 0);
    }
    
    private static IEnumerable<State> GetAdjacent(State current, Range<int> stepRange)
    {
        if (current.Count >= stepRange.Min)
        {
            var l = (Vector2D)(Rotation3D.Positive90Z * current.Face);
            var r = (Vector2D)(Rotation3D.Negative90Z * current.Face);
            
            yield return new State(Pos: current.Pos + l, Face: l, Count: 1);
            yield return new State(Pos: current.Pos + r, Face: r, Count: 1);
        }
        
        if (current.Count < stepRange.Max)
        {
            yield return new State(Pos: current.Pos + current.Face, Face: current.Face, Count: current.Count + 1);
        }
    }
}