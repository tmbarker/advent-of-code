using Problems.Common;
using Problems.Y2020.Common;

namespace Problems.Y2020.D01;

/// <summary>
/// Report Repair: https://adventofcode.com/2020/day/1
/// </summary>
public class Solution : SolutionBase2020
{
    private const int SumTarget = 2020;
    
    public override int Day => 1;
    
    public override object Run(int part)
    {
        var numbers = ParseNumbers(GetInputLines());
        return part switch
        {
            0 => GetSumPairProduct(SumTarget, numbers),
            1 => GetSumTripletProduct(SumTarget, numbers),
            _ => ProblemNotSolvedString,
        };
    }

    private static int GetSumPairProduct(int targetSum, IList<int> numbers)
    {
        var indexMap = new Dictionary<int, int>();
        for (var i = 0; i < numbers.Count; i++)
        {
            var number = numbers[i];
            var difference = targetSum - number;

            if (indexMap.TryGetValue(difference, out var index))
            {
                return number * numbers[index];
            }

            if (!indexMap.ContainsKey(number))
            {
                indexMap[number] = i;
            }
        }

        throw new NoSolutionException();
    }
    
    private static int GetSumTripletProduct(int targetSum, IList<int> numbers)
    {
        var indexMap = new Dictionary<int, int>();
        for (var i = 0; i < numbers.Count; i++)
        {
            var n1 = numbers[i];
            foreach (var n2 in numbers)
            {
                var n3 = targetSum - n2 - n1;
                if (indexMap.TryGetValue(n3, out var index))
                {
                    return n1 * n2 * numbers[index];
                }
            }
            
            if (!indexMap.ContainsKey(n1))
            {
                indexMap[n1] = i;
            }
        }

        throw new NoSolutionException();
    }

    private static IList<int> ParseNumbers(IEnumerable<string> input)
    {
        return input.Select(int.Parse).ToList();
    }
}