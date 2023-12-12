using Utilities.Geometry.Hexagonal;

namespace Problems.Y2017.D11;

[PuzzleInfo("Hex Ed", Topics.Math, Difficulty.Easy, favourite: true)]
public sealed class Solution : SolutionBase
{
    private static readonly Dictionary<string, Hex> Directions = new()
    {
        { "n",  Hex.Directions[Flat.N]  },
        { "ne", Hex.Directions[Flat.Ne] },
        { "se", Hex.Directions[Flat.Se] },
        { "s",  Hex.Directions[Flat.S]  },
        { "sw", Hex.Directions[Flat.Sw] },
        { "nw", Hex.Directions[Flat.Nw] }
    };

    public override object Run(int part)
    {
        var input = GetInputText();
        var steps = input.Split(separator: ',');
        
        return part switch
        {
            1 => GetEndDistance(steps),
            2 => GetMaxDistance(steps),
            _ => ProblemNotSolvedString
        };
    }

    private static int GetEndDistance(IEnumerable<string> steps)
    {
        var end = steps.Aggregate(
            seed: Hex.Zero,
            func: (pos, step) => pos + Directions[step]);

        return end.Magnitude;
    }
    
    private static int GetMaxDistance(IEnumerable<string> steps)
    {
        var pos = Hex.Zero;
        var set = new HashSet<Hex>();

        foreach (var step in steps)
        {
            pos += Directions[step];
            set.Add(pos);
        }

        return set.Max(p => p.Magnitude);
    }
}