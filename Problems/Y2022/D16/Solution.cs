using Problems.Y2022.Common;

namespace Problems.Y2022.D16;

/// <summary>
/// Proboscidea Volcanium: https://adventofcode.com/2022/day/16
/// </summary>
public class Solution : SolutionBase2022
{
    private const string Start = "AA";
    private const int TimeLimitAlone = 30;
    private const int TimeLimitWithHelp = 26;

    public override int Day => 16;
    
    public override object Run(int part)
    {
        return part switch
        {
            0 => ComputeMaxPressureRelieved(TimeLimitAlone, false),
            1 => ComputeMaxPressureRelieved(TimeLimitWithHelp, true),
            _ => ProblemNotSolvedString,
        };
    }

    private int ComputeMaxPressureRelieved(int timeLimit, bool withHelp)
    {
        var valveData = ValveData.Parse(GetInput(), Start);
        var strategyFinder = new MaxFlowFinder(valveData);
        
        return strategyFinder.Run(Start, timeLimit, withHelp);
    }
}