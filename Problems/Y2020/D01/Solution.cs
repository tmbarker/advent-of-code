using Problems.Common;
using Problems.Y2020.Common;

namespace Problems.Y2020.D01;

/// <summary>
/// Report Repair: https://adventofcode.com/2020/day/1
/// </summary>
public class Solution : SolutionBase2020
{
    public override int Day => 1;
    
    public override object Run(int part)
    {
        var numbers = ParseNumbers(GetInputLines());
        return part switch
        {
            1 => GetSumPairProduct(Year, numbers),
            2 => GetSumTripletProduct(Year, numbers),
            _ => ProblemNotSolvedString
        };
    }

    private static int GetSumPairProduct(int targetSum, IReadOnlySet<int> numbers)
    {
        foreach (var n1 in numbers)
        {
            var n2 = targetSum - n1;
            if (numbers.Contains(n2))
            {
                return n1 * n2;
            }
        }
        
        throw new NoSolutionException();
    }
    
    private static int GetSumTripletProduct(int targetSum, IReadOnlySet<int> numbers)
    {
        foreach (var n1 in numbers)
        foreach (var n2 in numbers)
        {
            var n3 = targetSum - n1 - n2;
            if (numbers.Contains(n3))
            {
                return n1 * n2 * n3;
            }
        }

        throw new NoSolutionException();
    }

    private static IReadOnlySet<int> ParseNumbers(IEnumerable<string> input)
    {
        return input.Select(int.Parse).ToHashSet();
    }
}