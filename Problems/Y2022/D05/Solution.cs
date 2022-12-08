using System.Text;
using Problems.Y2022.Common;

namespace Problems.Y2022.D05;

/// <summary>
/// Supply Stacks: https://adventofcode.com/2022/day/5
/// </summary>
public class Solution : SolutionBase2022
{
    public override int Day => 5;
    
    public override string Run(int part)
    {
        AssertInputExists();

        return part switch
        {
            0 => GetTopCratesAfterPlanExecution(CranePickupCapabilities.OneAtATime),
            1 => GetTopCratesAfterPlanExecution(CranePickupCapabilities.ManyAtATime),
            _ => ProblemNotSolvedString,
        };
    }

    private string GetTopCratesAfterPlanExecution(CranePickupCapabilities pickupCapabilities)
    {
        return CranePlan.TryParse(File.ReadLines(GetInputFilePath()), out var cranePlan)
            ? FormTopCratesString(CraneOperator.ExecutePlan(cranePlan!, pickupCapabilities))
            : ProblemNotSolvedString;
    }

    private static string FormTopCratesString(StacksState state)
    {
        var sb = new StringBuilder();
        foreach (var stack in state.StackMap.Values)
        {
            sb.Append(stack.Peek());
        }

        return sb.ToString();
    }
}