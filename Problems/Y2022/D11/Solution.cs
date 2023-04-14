using Problems.Common;

namespace Problems.Y2022.D11;

/// <summary>
/// Monkey in the Middle: https://adventofcode.com/2022/day/11
/// </summary>
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => QuantifyMonkeyBusiness(rounds: 20,    applyBoredDivisor: true),
            2 => QuantifyMonkeyBusiness(rounds: 10000, applyBoredDivisor: false),
            _ => ProblemNotSolvedString
        };
    }

    private long QuantifyMonkeyBusiness(int rounds, bool applyBoredDivisor)
    {
        var monkeyMap = MonkeyData.Parse(GetInputLines(), applyBoredDivisor);
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