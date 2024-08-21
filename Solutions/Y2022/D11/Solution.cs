namespace Solutions.Y2022.D11;

[PuzzleInfo("Monkey in the Middle", Topics.Simulation|Topics.Math, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => QuantifyMonkeyBusiness(rounds: 20,    applyBoredDivisor: true),
            2 => QuantifyMonkeyBusiness(rounds: 10000, applyBoredDivisor: false),
            _ => PuzzleNotSolvedString
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
            .OrderDescending()
            .Take(2)
            .Aggregate((first, second) => first * second);
        
    }
}