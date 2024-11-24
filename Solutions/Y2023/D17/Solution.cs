using Utilities.Collections;
using Utilities.Extensions;
using Utilities.Geometry.Euclidean;
using Utilities.Numerics;

namespace Solutions.Y2023.D17;

[PuzzleInfo("Clumsy Crucible", Topics.Graphs, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private const int Inf = int.MaxValue / 2;
    private readonly record struct State(Vec2D Pos, Vec2D Face, int Count);
    
    public override object Run(int part)
    {
        var input = GetInputLines();
        var grid = Grid2D<int>.MapChars(strings: input, elementFunc: c => c.AsDigit());
        
        return part switch
        {
            1 => Search(grid, constraint: new Range<int>(min: 1, max: 3)),
            2 => Search(grid, constraint: new Range<int>(min: 4, max: 10)),
            _ => PuzzleNotSolvedString
        };
    }
    
    private static int Search(Grid2D<int> grid, Range<int> constraint)
    {
        var start = new Vec2D(X: 0, Y: grid.Height - 1);
        var goal = new Vec2D(X: grid.Width - 1, Y: 0);
        var init = new State(Pos: start, Face: Vec2D.Right, Count: 0);

        var cost = new DefaultDict<State, int>(defaultValue: Inf, items: [(init, 0)]);
        var heap = new PriorityQueue<State, int>(items: [(init, 0)]);

        while (heap.Count > 0)
        {
            var state = heap.Dequeue();
            if (state.Pos == goal && state.Count >= constraint.Min)
            {
                return cost[state];
            }
            
            foreach (var adj in GetAdjacent(state, constraint))
            {
                if (grid.Contains(adj.Pos) && cost[state] + grid[adj.Pos] < cost[adj])
                {
                    cost[adj] = cost[state] + grid[adj.Pos];
                    heap.Enqueue(adj, cost[adj] + Vec2D.Distance(a: adj.Pos, b: goal, Metric.Taxicab));
                }
            }
        }

        throw new NoSolutionException();
    }
    
    private static IEnumerable<State> GetAdjacent(State state, Range<int> constraint)
    {
        if (state.Count >= constraint.Min)
        {
            var l = (Vec2D)(Rot3D.P90Z * state.Face);
            var r = (Vec2D)(Rot3D.N90Z * state.Face);
            
            yield return new State(Pos: state.Pos + l, Face: l, Count: 1);
            yield return new State(Pos: state.Pos + r, Face: r, Count: 1);
        }
        
        if (state.Count < constraint.Max)
        {
            yield return new State(Pos: state.Pos + state.Face, Face: state.Face, Count: state.Count + 1);
        }
    }
}