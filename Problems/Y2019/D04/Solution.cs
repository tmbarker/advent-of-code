using Problems.Common;
using Utilities.Cartesian;

namespace Problems.Y2019.D04;

/// <summary>
/// Secure Container: https://adventofcode.com/2019/day/4
/// </summary>
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputText();
        var range = ParseValidRange(input);
        
        return part switch
        {
            1 => CountValidPasswords(range, minRun: 2, maxRun: 6),
            2 => CountValidPasswords(range, minRun: 2, maxRun: 2),
            _ => ProblemNotSolvedString
        };
    }

    private static int CountValidPasswords(Aabb1D range, int minRun, int maxRun)
    {
        return Enumerable.Range(range.Min, range.Length).Count(p => IsValid(p, minRun, maxRun));
    }

    private static bool IsValid(int password, int minRun, int maxRun)
    {
        var chars = new List<char>(password.ToString());
        var observedRuns = new HashSet<int>();
        var curObservedRun = 1;

        for (var i = 1; i < chars.Count; i++)
        {
            if (chars[i] < chars[i - 1])
            {
                return false;
            }
            
            if (chars[i] == chars[i - 1])
            {
                curObservedRun++;
            }
            else
            {
                observedRuns.Add(curObservedRun);
                curObservedRun = 1;
            }
        }

        observedRuns.Add(curObservedRun);
        return observedRuns.Any(r => r >= minRun && r <= maxRun);
    }
    
    private static Aabb1D ParseValidRange(string input)
    {
        var elements = input.Split('-');
        return new Aabb1D(
            min: int.Parse(elements[0]),
            max: int.Parse(elements[1]));
    }
}