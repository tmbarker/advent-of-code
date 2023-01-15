using Problems.Y2020.Common;

namespace Problems.Y2020.D15;

/// <summary>
/// Rambunctious Recitation: https://adventofcode.com/2020/day/15
/// </summary>
public class Solution : SolutionBase2020
{
    private const int Turns1 = 2020;
    private const int Turns2 = 30000000;
    
    public override int Day => 15;
    
    public override object Run(int part)
    {
        var initialNumbers = GetInitialNumbers(GetInputText());
        return part switch
        {
            0 => GetNthSpokenNumber(initialNumbers, Turns1),
            1 => GetNthSpokenNumber(initialNumbers, Turns2),
            _ => ProblemNotSolvedString,
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
        return input.Split(',').Select(int.Parse);
    }
}