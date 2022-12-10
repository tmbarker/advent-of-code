using System.Text;
using Problems.Common;
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
        if (!CranePlan.TryParse(GetInput(), out var plan))
        {
            throw new NoSolutionException();
        }

        return part switch
        {
            0 => GetTopCratesAfterPlan(plan!, CranePickupMode.OneAtATime),
            1 => GetTopCratesAfterPlan(plan!, CranePickupMode.ManyAtATime),
            _ => ProblemNotSolvedString,
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