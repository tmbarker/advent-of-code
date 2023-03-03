using Problems.Y2022.Common;
using Utilities.Cartesian;
using Utilities.Extensions;

namespace Problems.Y2022.D23;

/// <summary>
/// Unstable Diffusion: https://adventofcode.com/2022/day/23
/// </summary>
public class Solution : SolutionBase2022
{
    private const char Elf = '#';
    private const int NumRounds = 10;

    public override int Day => 23;
    
    public override object Run(int part)
    {
        var positions = ParsePositions(GetInputLines());
        return part switch
        {
            1 => EmptyPositionsInBoundingBox(Simulate(positions, NumRounds)),
            2 => SimulateToSteadyState(positions),
            _ => ProblemNotSolvedString
        };
    }
    
    private static HashSet<Vector2D> Simulate(HashSet<Vector2D> positions, int numRounds)
    {
        for (var i = 0; i < numRounds; i++)
        {
            Diffuse(positions, i);
        }
        
        return positions;
    }

    private static int SimulateToSteadyState(HashSet<Vector2D> positions)
    {
        var index = 0;
        for (;; index++)
        {
            if (Diffuse(positions, index) <= 0)
            {
                break;
            }
        }

        return index + 1;
    }
    
    private static int Diffuse(HashSet<Vector2D> positions, int roundIndex)
    {
        var targetsMap = new Dictionary<Vector2D, Vector2D>();
        var targetsCount = new Dictionary<Vector2D, int>();

        foreach (var actor in positions)
        {
            var allAdj = actor.GetAdjacentSet(DistanceMetric.Chebyshev);
            if (allAdj.All(p => !positions.Contains(p)))
            {
                continue;
            }

            for (var i = roundIndex; i < roundIndex + MovePreferences.Count; i++)
            {
                var (move, checkSet) = MovePreferences.Get(i);
                if (checkSet.Any(check => positions.Contains(actor + check)))
                {
                    continue;
                }

                var target = actor + move;
                targetsMap[actor] = target;
                targetsCount.EnsureContainsKey(target);
                targetsCount[target]++;
                break;
            }
        }

        var numMoves = 0;
        foreach (var (actor, target) in targetsMap)
        {
            if (targetsCount[target] > 1)
            {
                continue;
            }

            numMoves++;
            positions.Remove(actor);
            positions.Add(target);
        }

        return numMoves;
    }

    private static int EmptyPositionsInBoundingBox(IReadOnlySet<Vector2D> positions)
    {
        var xMin = positions.Min(p => p.X);
        var xMax = positions.Max(p => p.X);
        var yMin = positions.Min(p => p.Y);
        var yMax = positions.Max(p => p.Y);
        var emptyCount = 0;
        
        for (var y = yMin; y <= yMax; y++)
        for (var x = xMin; x <= xMax; x++)
        {
            if (!positions.Contains(new Vector2D(x, y)))
            {
                emptyCount++;
            }
        }

        return emptyCount;
    }

    private static HashSet<Vector2D> ParsePositions(IList<string> input)
    {
        var set = new HashSet<Vector2D>();
        
        for (var y = 0; y < input.Count; y++)
        for (var x = 0; x < input[y].Length; x++)
        {
            if (input[input.Count - y - 1][x] == Elf)
            {
                set.Add(new Vector2D(x, y));
            }
        }

        return set;
    }
}