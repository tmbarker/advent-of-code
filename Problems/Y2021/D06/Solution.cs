using Problems.Y2021.Common;

namespace Problems.Y2021.D06;

/// <summary>
/// Lanternfish: https://adventofcode.com/2021/day/6
/// </summary>
public class Solution : SolutionBase2021
{
    private const char Delimiter = ',';

    private const int DaysPart1 = 80;
    private const int DaysPart2 = 256;
    private const int ResetTo = 6;
    private const int SpawnAt = 8;

    public override int Day => 6;
    
    public override object Run(int part)
    {
        return part switch
        {
            0 => ModelLanternFish(GetInitialState(), DaysPart1),
            1 => ModelLanternFish(GetInitialState(), DaysPart2),
            _ => ProblemNotSolvedString,
        };
    }

    private static long ModelLanternFish(IEnumerable<int> initialState, int days)
    {
        var internalTimerCount = new long[SpawnAt + 1];
        foreach (var timer in initialState)
        {
            internalTimerCount[timer]++;
        }

        for (var day = 0; day < days; day++)
        {
            var readyToSpawn = internalTimerCount[0];
            for (var t = 0; t < SpawnAt; t++)
            {
                internalTimerCount[t] = internalTimerCount[t + 1];
            }

            internalTimerCount[ResetTo] += readyToSpawn;
            internalTimerCount[SpawnAt] = readyToSpawn;
        }

        return internalTimerCount.Sum();
    }

    private IEnumerable<int> GetInitialState()
    {
        AssertInputExists();

        return File.ReadAllText(GetInputFilePath())
            .Split(Delimiter)
            .Select(int.Parse);
    }
}