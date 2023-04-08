using Problems.Attributes;
using Problems.Y2017.Common;
using Utilities.Hexagonal;

namespace Problems.Y2017.D11;

/// <summary>
/// Hex Ed: https://adventofcode.com/2017/day/11
/// </summary>
[Favourite("Hex Ed", Topics.Math, Difficulty.Easy)]
public class Solution : SolutionBase2017
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

    public override int Day => 11;
    
    public override object Run(int part)
    {
        var input = GetInputText();
        var steps = input.Split(',');
        
        return part switch
        {
            1 => GetMinDistance(steps),
            2 => GetMaxDistance(steps),
            _ => ProblemNotSolvedString
        };
    }

    private static int GetMinDistance(IEnumerable<string> steps)
    {
        var pos = steps.Aggregate(
            seed: Hex.Zero,
            func: (current, step) => current + Directions[step]);

        return Hex.Distance(a: Hex.Zero, b: pos);
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

        return set.Max(p => Hex.Distance(a: Hex.Zero, b: p));
    }
}