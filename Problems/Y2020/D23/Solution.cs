using System.Text;
using Problems.Common;

namespace Problems.Y2020.D23;

/// <summary>
/// Crab Cups: https://adventofcode.com/2020/day/23
/// </summary>
public class Solution : SolutionBase
{
    private const int Moves1 = 100;
    private const int Moves2 = 10000000;
    private const int PadTo = 1000000;

    public override object Run(int part)
    {
        var numbers = ParseNumbers(GetInputText());
        return part switch
        {
            1 => FormWrappingString(PlayGame(Moves1, numbers)),
            2 => ComputeProduct(PlayGame(Moves2, PadNumbersTo(numbers, PadTo))),
            _ => ProblemNotSolvedString
        };
    }

    private static int[] PlayGame(int moves, IList<int> cups)
    {
        var curCup = cups.First();
        var minCup = cups.Min();
        var maxCup = cups.Max();
        
        var nextCupMap = new int[cups.Count + 1];
        for (var i = 0; i < cups.Count; i++)
        {
            nextCupMap[i + 1] = cups[(cups.IndexOf(i + 1) + 1) % cups.Count];
        }
        
        for (var m = 0; m < moves; m++)
        {
            var pickedUp1 = nextCupMap[curCup];
            var pickedUp2 = nextCupMap[pickedUp1];
            var pickedUp3 = nextCupMap[pickedUp2];

            var targetCup = curCup;
            do
            {
                targetCup = targetCup - 1 < minCup
                    ? maxCup
                    : targetCup - 1;
            } 
            while (targetCup == pickedUp1 || targetCup == pickedUp2 || targetCup == pickedUp3);
            
            nextCupMap[curCup]    = nextCupMap[pickedUp3];
            nextCupMap[pickedUp3] = nextCupMap[targetCup];
            nextCupMap[targetCup] = pickedUp1;

            curCup = nextCupMap[curCup];
        }

        return nextCupMap;
    }
    
    private static string FormWrappingString(IReadOnlyList<int> nextCupMap)
    {
        var next = 1;
        var sb = new StringBuilder();
        
        for (var i = 0; i < nextCupMap.Count - 2; i++)
        {
            next = nextCupMap[next];
            sb.Append(next);
        }
        return sb.ToString();
    }

    private static long ComputeProduct(IReadOnlyList<int> nextCupMap)
    {
        var n1 = nextCupMap[1];
        var n2 = nextCupMap[n1];
        return (long)n1 * n2;
    }
    
    private static IList<int> PadNumbersTo(IList<int> numbers, int padTo)
    {
        var padded = new List<int>(Enumerable.Range(1, padTo));
        for (var i = 0; i < numbers.Count; i++)
        {
            padded[i] = numbers[i];
        }
        return padded;
    }
    
    private static IList<int> ParseNumbers(string input)
    {
        return new List<int>(input.Select(c => int.Parse(c.ToString())));
    }
}