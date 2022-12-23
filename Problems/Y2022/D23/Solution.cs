using Problems.Y2022.Common;
using Utilities.DataStructures.Cartesian;
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
        var elfPositions = ParseElfPositions(GetInput());
        return part switch
        {
            0 => EmptyPositionsInBoundingRect(Simulate(elfPositions, NumRounds)),
            1 => SimulateToSteadyState(elfPositions),
            _ => ProblemNotSolvedString,
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

        foreach (var elf in positions)
        {
            var allAdj = elf.GetAdjacentSet(DistanceMetric.Chebyshev);
            if (allAdj.All(p => !positions.Contains(p)))
            {
                continue;
            }

            for (var i = roundIndex; i < roundIndex + MoveChoices.NumChoices; i++)
            {
                var (move, checkSet) = MoveChoices.Get(i);
                if (checkSet.Any(check => positions.Contains(elf + check)))
                {
                    continue;
                }

                var target = elf + move;
                targetsMap[elf] = target;
                targetsCount.EnsureContainsKey(target);
                targetsCount[target]++;
                break;
            }
        }

        var numMoves = 0;
        foreach (var (elf, target) in targetsMap)
        {
            if (targetsCount[target] > 1)
            {
                continue;
            }

            numMoves++;
            positions.Remove(elf);
            positions.Add(target);
        }

        return numMoves;
    }

    private static int EmptyPositionsInBoundingRect(IReadOnlySet<Vector2D> positions)
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

    private static HashSet<Vector2D> ParseElfPositions(IList<string> input)
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