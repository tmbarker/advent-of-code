using Utilities.Collections;
using Utilities.Extensions;
using Utilities.Geometry.Euclidean;
using Utilities.Numerics;

namespace Solutions.Y2023.D17;

[PuzzleInfo("Clumsy Crucible", Topics.Graphs, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private const int Inf = int.MaxValue / 2;
    private readonly record struct State(Vector2D Pos, Vector2D Face, int Count);
    
    public override object Run(int part)
    {
        var input = GetInputLines();
        var grid = Grid2D<int>.MapChars(strings: input, elementFunc: c => c.AsDigit());
        
        return part switch
        {
            1 => Search(grid, constraint: new Range<int>(Min: 1, Max: 3)),
            2 => Search(grid, constraint: new Range<int>(Min: 4, Max: 10)),
            _ => ProblemNotSolvedString
        };
    }
    
    private static int Search(Grid2D<int> grid, Range<int> constraint)
    {
        var start = new Vector2D(x: 0, y: grid.Height - 1);
        var goal = new Vector2D(x: grid.Width - 1, y: 0);
        var init = new State(Pos: start, Face: Vector2D.Right, Count: 0);

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
                    heap.Enqueue(adj, cost[adj] + Vector2D.Distance(a: adj.Pos, b: goal, Metric.Taxicab));
                }
            }
        }

        throw new NoSolutionException();
    }
    
    private static IEnumerable<State> GetAdjacent(State state, Range<int> constraint)
    {
        if (state.Count >= constraint.Min)
        {
            var l = (Vector2D)(Rotation3D.Positive90Z * state.Face);
            var r = (Vector2D)(Rotation3D.Negative90Z * state.Face);
            
            yield return new State(Pos: state.Pos + l, Face: l, Count: 1);
            yield return new State(Pos: state.Pos + r, Face: r, Count: 1);
        }
        
        if (state.Count < constraint.Max)
        {
            yield return new State(Pos: state.Pos + state.Face, Face: state.Face, Count: state.Count + 1);
        }
    }
}