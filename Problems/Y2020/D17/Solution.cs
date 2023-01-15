using Problems.Y2020.Common;
using Utilities.Cartesian;

namespace Problems.Y2020.D17;

/// <summary>
/// Conway Cubes: https://adventofcode.com/2020/day/17
/// </summary>
public class Solution : SolutionBase2020
{
    private const char On = '#';
    private const int Cycles = 6;

    private static readonly IReadOnlySet<int> StayOnSet = new HashSet<int> { 2, 3 };
    private static readonly IReadOnlySet<int> TurnOnSet = new HashSet<int> { 3 };

    public override int Day => 17;
    
    public override object Run(int part)
    {
        return part switch
        {
            0 => Cycle3D(GetInputLines(), Cycles),
            1 => Cycle4D(GetInputLines(), Cycles),
            _ => ProblemNotSolvedString,
        };
    }

    private static int Cycle3D(IList<string> input, int cycles)
    {
        var active = ParseInitial(input, (x, y) => new Vector3D(x, y, 0));
        for (var i = 0; i < cycles; i++)
        {
            active = Cycle3D(active);
        }

        return active.Count;
    }
    
    private static int Cycle4D(IList<string> input, int cycles)
    {
        var active = ParseInitial(input, (x, y) => new Vector4D(x, y, 0, 0));
        for (var i = 0; i < cycles; i++)
        {
            active = Cycle4D(active);
        }

        return active.Count;
    }

    private static ISet<Vector3D> Cycle3D(ICollection<Vector3D> active)
    {
        var nextActive = new HashSet<Vector3D>();
        foreach (var pos in new Aabb3D(active, false))
        {
            var activeAdjCount = pos
                .GetAdjacentSet(DistanceMetric.Chebyshev)
                .Count(active.Contains);

            AddIfShouldBeActive(pos, nextActive, active, activeAdjCount);
        }
        return nextActive;
    }
    
    private static ISet<Vector4D> Cycle4D(ICollection<Vector4D> active)
    {
        var nextActive = new HashSet<Vector4D>();
        foreach (var pos in new Aabb4D(active, false))
        {
            var activeAdjCount = pos
                .GetAdjacentSet(DistanceMetric.Chebyshev)
                .Count(active.Contains);

            AddIfShouldBeActive(pos, nextActive, active, activeAdjCount);
        }
        return nextActive;
    }

    private static void AddIfShouldBeActive<T>(T pos, ISet<T> next, ICollection<T> current, int adjCount)
    {
        if (current.Contains(pos) && StayOnSet.Contains(adjCount))
        {
            next.Add(pos);
        }
        else if (!current.Contains(pos) && TurnOnSet.Contains(adjCount))
        {
            next.Add(pos);
        }
    }
    
    private static ISet<T> ParseInitial<T>(IList<string> input, Func<int,int, T> parseFunc)
    {
        var set = new HashSet<T>();
        var rows = input.Count;
        var cols = input[0].Length;
        
        for (var y = 0; y < rows; y++)
        for (var x = 0; x < cols; x++)
        {
            if (input[rows - y - 1][x] == On)
            {
                set.Add(parseFunc(x, rows - y - 1));
            }
        }

        return set;
    }
}