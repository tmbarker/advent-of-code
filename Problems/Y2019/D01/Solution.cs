using Problems.Y2019.Common;

namespace Problems.Y2019.D01;

/// <summary>
/// The Tyranny of the Rocket Equation: https://adventofcode.com/2019/day/1
/// </summary>
public class Solution : SolutionBase2019
{
    public override int Day => 1;
    
    public override object Run(int part)
    {
        var masses = ParseMasses(GetInputLines());
        return part switch
        {
            1 =>  masses.Sum(GetNaiveFuelRequirement),
            2 =>  masses.Sum(GetIterativeFuelRequirement),
            _ => ProblemNotSolvedString
        };
    }

    private static int GetNaiveFuelRequirement(int mass)
    {
        return mass / 3 - 2;
    }
    
    private static int GetIterativeFuelRequirement(int mass)
    {
        var total = 0;
        var fuel = GetNaiveFuelRequirement(mass);

        while (fuel > 0)
        {
            total += fuel;
            fuel = GetNaiveFuelRequirement(fuel);
        }

        return total;
    }

    private static IEnumerable<int> ParseMasses(IEnumerable<string> input)
    {
        return input.Select(int.Parse);
    }
}