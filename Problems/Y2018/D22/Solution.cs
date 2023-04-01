using Problems.Attributes;
using Problems.Common;
using Problems.Y2018.Common;
using Utilities.Cartesian;
using Utilities.Extensions;

namespace Problems.Y2018.D22;

/// <summary>
/// Mode Maze: https://adventofcode.com/2018/day/22
/// </summary>
[Favourite("Mode Maze", Topics.Graphs, Difficulty.Hard)]
public class Solution : SolutionBase2018
{
    private const int MoveCost = 1;
    private const int SwapCost = 7;
    
    public override int Day => 22;
    
    public override object Run(int part)
    {
        var input = GetInputLines();
        var scan = ParseInput(input);
        var cave = new Cave(scan);
        
        return part switch
        {
            1 => SumAreaRisks(cave, min: scan.Mouth, max: scan.Target),
            2 => Traverse(cave, scan),
            _ => ProblemNotSolvedString
        };
    }

    private static long SumAreaRisks(Cave cave, Vector2D min, Vector2D max)
    {
        var aabb = new Aabb2D(extents: new[] { min, max }, inclusive: true);
        var regions = aabb.Select(pos => cave[pos].Type);

        return regions.Sum(region => Cave.RegionRiskLevels[region]);
    }

    private static int Traverse(Cave cave, Scan scan)
    {
        var start = new State(scan.Mouth, EquippedTool.Torch);
        var target = new State(scan.Target, EquippedTool.Torch);
        
        var heap = new PriorityQueue<State, int>(new[] { (start, 0) });
        var costs = new Dictionary<State, int> { { start, 0 } };

        while (heap.Count > 0)
        {
            var current = heap.Dequeue();
            if (current == target)
            {
                return costs[current];
            }
            
            foreach (var (state, cost) in GetPossibleTransitions(current, cave))
            {
                costs.EnsureContainsKey(state, int.MaxValue);
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
            .GetAdjacentSet(DistanceMetric.Taxicab)
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

    private static Scan ParseInput(IList<string> input)
    {
        var coords = input[1].ParseInts();
        var target = new Vector2D(
            x: coords[0],
            y: coords[1]);

        return new Scan(
            Depth: input[0].ParseInts()[0],
            Mouth: Vector2D.Zero,
            Target: target);
    }
}