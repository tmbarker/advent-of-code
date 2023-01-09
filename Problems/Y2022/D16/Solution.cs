using Problems.Y2022.Common;

namespace Problems.Y2022.D16;

/// <summary>
/// Proboscidea Volcanium: https://adventofcode.com/2022/day/16
/// </summary>
public class Solution : SolutionBase2022
{
    private const string Start = "AA";
    private const int TimeAlone = 30;
    private const int TimeWithHelp = 26;

    public override int Day => 16;
    
    public override object Run(int part)
    {
        return part switch
        {
            0 => ComputeMaxPressureRelieved(TimeAlone, false),
            1 => ComputeMaxPressureRelieved(TimeWithHelp, true),
            _ => ProblemNotSolvedString,
        };
    }

    private int ComputeMaxPressureRelieved(int timeLimit, bool withHelp)
    {
        var valveData = ValveData.Parse(GetInputLines());
        var strategyFinder = new StrategyFinder(valveData);
        
        return strategyFinder.Run(Start, timeLimit, withHelp);
    }
}