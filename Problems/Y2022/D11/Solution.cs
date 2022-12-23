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
    
    public override object Run(int part)
    {
        return part switch
        {
            0 => QuantifyMonkeyBusiness(RoundsPart1, true),
            1 => QuantifyMonkeyBusiness(RoundsPart2, false),
            _ => ProblemNotSolvedString,
        };
    }

    private long QuantifyMonkeyBusiness(int rounds, bool applyBoredDivisor)
    {
        var monkeyMap = MonkeyData.Parse(GetInput(), applyBoredDivisor);
        var activityCounts = new long[monkeyMap.Count];
        var round = 0;

        while (round < rounds)
        {
            for (var i = 0; i < monkeyMap.Count; i++)
            {
                while (monkeyMap[i].IsHoldingItem)
                {
                    var (throwTo, thrownItem) = monkeyMap[i].InspectNextItem();
                    activityCounts[i]++;
                    monkeyMap[throwTo].CatchItem(thrownItem);
                }
            }
            
            round++;
        }

        return activityCounts
            .OrderByDescending(n => n)
            .Take(2)
            .Aggregate((first, second) => first * second);
        
    }
}