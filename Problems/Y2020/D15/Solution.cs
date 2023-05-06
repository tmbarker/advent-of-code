using Problems.Common;

namespace Problems.Y2020.D15;

/// <summary>
/// Rambunctious Recitation: https://adventofcode.com/2020/day/15
/// </summary>
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var initialNumbers = GetInitialNumbers(GetInputText());
        return part switch
        {
            1 => GetNthSpokenNumber(initialNumbers, n: 2020),
            2 => GetNthSpokenNumber(initialNumbers, n: 30000000),
            _ => ProblemNotSolvedString
        };
    }

    private static int GetNthSpokenNumber(IEnumerable<int> startingNumbers, int n)
    {
        var turnNumber = 1;
        var lastSpoken = 0;
        var spokenMap = new Dictionary<int, (int Last, int Previous)>();

        foreach (var number in startingNumbers)
        {
            lastSpoken = number;
            spokenMap[number] = (turnNumber, turnNumber);

            turnNumber++;
        }
        
        while (turnNumber <= n)
        {
            lastSpoken = spokenMap[lastSpoken].Last - spokenMap[lastSpoken].Previous;
            spokenMap[lastSpoken] = spokenMap.ContainsKey(lastSpoken)
                ? spokenMap[lastSpoken] = (turnNumber, spokenMap[lastSpoken].Last)
                : spokenMap[lastSpoken] = (turnNumber, turnNumber);

            turnNumber++;
        }

        return lastSpoken;
    }

    private static IEnumerable<int> GetInitialNumbers(string input)
    {
        return input.Split(separator: ',').Select(int.Parse);
    }
}