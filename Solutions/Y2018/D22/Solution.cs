using Utilities.Collections;
using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2018.D22;

[PuzzleInfo("Mode Maze", Topics.Graphs, Difficulty.Hard, favourite: true)]
public sealed class Solution : SolutionBase
{
    private const int MoveCost = 1;
    private const int SwapCost = 7;

    public override object Run(int part)
    {
        var input = GetInputLines();
        var scan = ParseInput(input);
        var cave = new Cave(scan);
        
        return part switch
        {
            1 => SumAreaRisks(cave, min: scan.Mouth, max: scan.Target),
            2 => Traverse(cave, scan),
            _ => PuzzleNotSolvedString
        };
    }

    private static long SumAreaRisks(Cave cave, Vec2D min, Vec2D max)
    {
        var aabb = new Aabb2D(min, max);
        var regions = aabb.Select(pos => cave[pos].Type);

        return regions.Sum(region => Cave.RegionRiskLevels[region]);
    }

    private static int Traverse(Cave cave, Scan scan)
    {
        var start =  new State(scan.Mouth, EquippedTool.Torch);
        var target = new State(scan.Target, EquippedTool.Torch);
        
        var heap = new PriorityQueue<State, int>(items: [(start, 0)]);
        var costs = new DefaultDict<State, int>(defaultValue: int.MaxValue / 2, items: [(start, 0)]);

        while (heap.Count > 0)
        {
            var current = heap.Dequeue();
            if (current == target)
            {
                return costs[current];
            }
            
            foreach (var (state, cost) in GetPossibleTransitions(current, cave))
            {
                if (costs[current] + cost < costs[state])
                {
                    costs[state] = costs[current] + cost;
                    heap.Enqueue(state, costs[state]);
                }
            }
        }
        
        throw new NoSolutionException();
    }

    private static IEnumerable<Transition> GetPossibleTransitions(State state, Cave cave)
    {
        var region = cave[state.Pos].Type;
        var swapTo = Cave.RegionAllowedTools[region]
            .Except(state.Tool)
            .Single();

        yield return new Transition(
            NextState: state with { Tool = swapTo },
            Cost: SwapCost);

        var adjacencies = state.Pos
            .GetAdjacentSet(Metric.Taxicab)
            .Where(pos => pos is { X: >= 0, Y: >= 0 });

        foreach (var adjacent in adjacencies)
        {
            var allowedTools = Cave.RegionAllowedTools[cave[adjacent].Type];
            var canEnter = allowedTools.Contains(state.Tool);
            
            if (canEnter)
            {
                yield return new Transition(
                    NextState: state with { Pos = adjacent },
                    Cost: MoveCost);
            }
        }
    }

    private static Scan ParseInput(string[] input)
    {
        return new Scan(
            Depth: input[0].ParseInt(),
            Mouth: Vec2D.Zero,
            Target: Vec2D.Parse(input[1]));
    }
}