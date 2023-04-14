using System.Text;
using Problems.Common;

namespace Problems.Y2022.D05;

/// <summary>
/// Supply Stacks: https://adventofcode.com/2022/day/5
/// </summary>
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        if (!CranePlan.TryParse(GetInputLines(), out var plan))
        {
            throw new NoSolutionException(message: "Failed to parse input");
        }

        return part switch
        {
            1 => GetTopCratesAfterPlan(plan!, CranePickupMode.OneAtATime),
            2 => GetTopCratesAfterPlan(plan!, CranePickupMode.ManyAtATime),
            _ => ProblemNotSolvedString
        };
    }

    private static string GetTopCratesAfterPlan(CranePlan plan, CranePickupMode mode)
    {
        return GetTopCrates(CraneOperator.ExecutePlan(plan, mode));
    }
    
    private static string GetTopCrates(StacksState state)
    {
        var sb = new StringBuilder();
        foreach (var stack in state.StackMap.Values)
        {
            sb.Append(stack.Peek());
        }

        return sb.ToString();
    }
}