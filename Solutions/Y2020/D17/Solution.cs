using Utilities.Geometry.Euclidean;

namespace Solutions.Y2020.D17;

[PuzzleInfo("Conway Cubes", Topics.Vectors|Topics.Simulation, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private static readonly HashSet<int> StayOnSet = [2, 3];
    private static readonly HashSet<int> TurnOnSet = [3];

    public override object Run(int part)
    {
        return part switch
        {
            1 => Cycle3D(GetInputLines(), cycles: 6),
            2 => Cycle4D(GetInputLines(), cycles: 6),
            _ => PuzzleNotSolvedString
        };
    }

    private static int Cycle3D(IList<string> input, int cycles)
    {
        var active = ParseInitial(input, (x, y) => new Vec3D(x, y, z: 0));
        for (var i = 0; i < cycles; i++)
        {
            active = Cycle3D(active);
        }

        return active.Count;
    }
    
    private static int Cycle4D(IList<string> input, int cycles)
    {
        var active = ParseInitial(input, (x, y) => new Vec4D(x, y, z: 0, w: 0));
        for (var i = 0; i < cycles; i++)
        {
            active = Cycle4D(active);
        }

        return active.Count;
    }

    private static ISet<Vec3D> Cycle3D(ICollection<Vec3D> active)
    {
        var nextActive = new HashSet<Vec3D>();
        foreach (var pos in new Aabb3D(active, inclusive: false))
        {
            var activeAdjCount = pos
                .GetAdjacentSet(Metric.Chebyshev)
                .Count(active.Contains);

            AddIfShouldBeActive(pos, nextActive, active, activeAdjCount);
        }
        return nextActive;
    }
    
    private static ISet<Vec4D> Cycle4D(ICollection<Vec4D> active)
    {
        var nextActive = new HashSet<Vec4D>();
        foreach (var pos in new Aabb4D(active, inclusive: false))
        {
            var activeAdjCount = pos
                .GetAdjacentSet(Metric.Chebyshev)
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
            if (input[rows - y - 1][x] == '#')
            {
                set.Add(parseFunc(x, rows - y - 1));
            }
        }

        return set;
    }
}