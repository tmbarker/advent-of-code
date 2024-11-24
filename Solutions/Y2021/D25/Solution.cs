using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2021.D25;

using Herds = Dictionary<Vec2D, ISet<Vec2D>>;

[PuzzleInfo("Sea Cucumber", Topics.Vectors|Topics.Simulation, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private static readonly Dictionary<char, Vec2D> HerdDirections = new()
    {
        { '>', Vec2D.Right },
        { 'v', Vec2D.Down }
    };
    
    public override int Parts => 1;

    public override object Run(int part)
    {
        ParseInput(GetInputLines(), out var herds, out var bounds);
        return part switch
        {
            1 => CountStepsTillSteadyState(herds, bounds),
            _ => PuzzleNotSolvedString
        };
    }

    private int CountStepsTillSteadyState(Herds herds, Aabb2D bounds)
    {
        for (var i = 0;; i++)
        {
            if (LogsEnabled)
            {
                Console.WriteLine($"Stepped herds: #{i}");   
            }
            
            if (!TryStepHerds(herds, bounds))
            {
                return i + 1;
            }
        }
    }

    private static bool TryStepHerds(Herds herds, Aabb2D bounds)
    {
        var moved = false;
        
        foreach (var (direction, herd) in herds)
        {
            var occupied = FlattenHerds(herds);
            foreach (var memberAt in herd.Freeze())
            {
                if (!TryStepMember(memberAt, direction, occupied, bounds, out var movedTo))
                {
                    continue;
                }
                
                herds[direction].Remove(memberAt);
                herds[direction].Add(movedTo);
                moved = true;
            }
        }
        
        return moved;
    }

    private static bool TryStepMember(Vec2D memberAt, Vec2D heading, ICollection<Vec2D> occupied, Aabb2D bounds, out Vec2D movedTo)
    {
        movedTo = memberAt + heading;
        if (!bounds.Contains(movedTo, true))
        {
            movedTo = new Vec2D(
                X: movedTo.X.Modulo(bounds.Width),
                Y: movedTo.Y.Modulo(bounds.Height));
        }

        return !occupied.Contains(movedTo);
    }

    private static ICollection<Vec2D> FlattenHerds(Herds herds)
    {
        return new List<Vec2D>(herds.Values.SelectMany(m => m));
    }

    private static void ParseInput(IList<string> input, out Dictionary<Vec2D, ISet<Vec2D>> herds, out Aabb2D bounds)
    {
        var rows = input.Count;
        var cols = input[0].Length;

        bounds = new Aabb2D(xMin: 0, xMax: cols - 1, yMin: 0, yMax: rows - 1);
        herds = HerdDirections.Values.ToDictionary<Vec2D, Vec2D, ISet<Vec2D>>(
            keySelector: d => d,
            elementSelector: _ => new HashSet<Vec2D>());

        for (var y = 0; y < rows; y++)
        for (var x = 0; x < cols; x++)
        {
            if (HerdDirections.TryGetValue(input[y][x], out var direction))
            {
                herds[direction].Add(new Vec2D(x, rows - y - 1));
            }
        }
    }
}