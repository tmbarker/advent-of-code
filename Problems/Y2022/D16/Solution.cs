using Problems.Attributes;
using Problems.Y2022.Common;

namespace Problems.Y2022.D16;

/// <summary>
/// Proboscidea Volcanium: https://adventofcode.com/2022/day/16
/// </summary>
[Favourite("Proboscidea Volcanium", Topics.Graphs|Topics.Recursion, Difficulty.Hard)]
public class Solution : SolutionBase2022
{
    private const string Start = "AA";
    private const int TimeAlone = 30;
    private const int TimeWithHelp = 26;

    public override int Day => 16;
    
    public override object Run(int part)
    {
        var valveData = ValveData.Parse(GetInputLines());
        var strategyFinder = new StrategyFinder(valveData);
        
        return part switch
        {
            1 =>  GetMaxFlowAlone(strategyFinder),
            2 =>  GetMaxFlowWithHelp(strategyFinder),
            _ => ProblemNotSolvedString
        };
    }

    private static int GetMaxFlowAlone(StrategyFinder strategyFinder)
    {
        var max = 0;
        void OnStrategyFound(Strategy strategy)
        {
            max = Math.Max(max, strategy.Flow);
        }

        strategyFinder.StrategyFound += OnStrategyFound;
        strategyFinder.Run(Start, TimeAlone);

        return max;
    }
    
    private static int GetMaxFlowWithHelp(StrategyFinder strategyFinder)
    {
        var max = 0;
        var strategies = new List<Strategy>();
        void OnStrategyFound(Strategy strategy)
        {
            strategies.Add(strategy);
        }

        strategyFinder.StrategyFound += OnStrategyFound;
        strategyFinder.Run(Start, TimeWithHelp);
        
        foreach (var s1 in strategies)
        foreach (var s2 in strategies)
        {
            if (s1.Flow + s2.Flow > max && !s1.Opened.Intersect(s2.Opened).Any())
            {
                max = s1.Flow + s2.Flow;
            }
        }

        return max;
    }
}
