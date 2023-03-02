using Problems.Common;
using Problems.Y2020.Common;

namespace Problems.Y2020.D09;

/// <summary>
/// Encoding Error: https://adventofcode.com/2020/day/9
/// </summary>
public class Solution : SolutionBase2020
{
    private const int PreambleLength = 25;
    
    public override int Day => 9;
    
    public override object Run(int part)
    {
        var numbers = GetNumbers(GetInputLines());
        return part switch
        {
            1 =>  FindWeakness(numbers),
            2 =>  ExploitWeakness(numbers),
            _ => ProblemNotSolvedString
        };
    }

    private static long FindWeakness(IList<long> numbers)
    {
        var window = new Queue<long>(numbers.Take(PreambleLength));
        for (var i = PreambleLength; i < numbers.Count; i++)
        {
            if (!TwoSumExists(numbers[i], new HashSet<long>(window)))
            {
                return numbers[i];
            }

            window.Dequeue();
            window.Enqueue(numbers[i]);
        }

        throw new NoSolutionException();
    }

    private static long ExploitWeakness(IList<long> numbers)
    {
        var weakness = FindWeakness(numbers);
        var trailingSums = new List<Sum>();

        foreach (var number in numbers)
        {
            for (var j = trailingSums.Count - 1; j >= 0; j--)
            {
                if (trailingSums[j].Add(number) == weakness)
                {
                    return trailingSums[j].GetExtremaSum();
                }
                
                if (trailingSums[j].Value > weakness)
                {
                    trailingSums.RemoveAt(j);
                }
            }

            trailingSums.Add(new Sum(number));
        }

        throw new NoSolutionException();
    }
    
    private static bool TwoSumExists(long number, IReadOnlySet<long> window)
    {
        return window.Select(n1 => number - n1).Any(window.Contains);
    }

    private static IList<long> GetNumbers(IEnumerable<string> input)
    {
        return input.Select(long.Parse).ToList();
    }
}