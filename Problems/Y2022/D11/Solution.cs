using Problems.Y2022.Common;

namespace Problems.Y2022.D11;

/// <summary>
/// Monkey in the Middle: https://adventofcode.com/2022/day/11
/// </summary>
public class Solution : SolutionBase2022
{
    private const int RoundsPart1 = 20;
    private const int RoundsPart2 = 10000;

    public override int Day => 11;
    
    public override string Run(int part)
    {
        return part switch
        {
            0 => QuantifyMonkeyBusiness(RoundsPart1, true).ToString(),
            1 => QuantifyMonkeyBusiness(RoundsPart2, false).ToString(),
            _ => ProblemNotSolvedString,
        };
    }

    private long QuantifyMonkeyBusiness(int rounds, bool applyBoredDivisor)
    {
        var monkeyMap = MonkeyMap.Parse(GetInput(), applyBoredDivisor);
        var activityMap = monkeyMap.ToDictionary(kvp => kvp.Key, _ => (long)0);
        var round = 0;

        while (round < rounds)
        {
            for (var i = 0; i < monkeyMap.Count; i++)
            {
                while (monkeyMap[i].IsHoldingItem)
                {
                    var (throwTo, thrownItem) = monkeyMap[i].InspectNextItem();
                    activityMap[i]++;
                    monkeyMap[throwTo].CatchItem(thrownItem);
                }
            }
            
            round++;
        }

        return activityMap.Values
            .OrderByDescending(n => n)
            .Take(2)
            .Aggregate((first, second) => first * second);
        
    }
}