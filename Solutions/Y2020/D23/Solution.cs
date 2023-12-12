using System.Text;
using Utilities.Extensions;

namespace Solutions.Y2020.D23;

[PuzzleInfo("Crab Cups", Topics.Simulation, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var cups = GetInputText()
            .Select(StringExtensions.AsDigit)
            .ToList();
        
        var map = part switch
        {
            1 => PlayGame(moves: 100, cups),
            2 => PlayGame(moves: 10000000, cups: Pad(cups, length: 1000000)),
            _ => throw new NoSolutionException()
        };
        
        return part switch
        {
            1 => FormWrappingString(map),
            2 => ComputeProduct(map),
            _ => throw new NoSolutionException()
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
            nextCupMap[cups[i]] = cups[(i + 1) % cups.Count];
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
        var nc1 = nextCupMap[1];
        var nc2 = nextCupMap[nc1];
        return (long)nc1 * nc2;
    }
    
    private static IList<int> Pad(IList<int> numbers, int length)
    {
        var padded = new List<int>(Enumerable.Range(1, length));
        for (var i = 0; i < numbers.Count; i++)
        {
            padded[i] = numbers[i];
        }
        
        return padded;
    }
}