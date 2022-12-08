namespace Problems.Y2022.D05;

public static class CraneOperator
{
    public static StacksState ExecutePlan(CranePlan plan, CranePickupMode pickupMode)
    {
        return pickupMode switch
        {
            CranePickupMode.OneAtATime => ExecutePlanOneAtATime(plan),
            CranePickupMode.ManyAtATime => ExecutePlanManyAtATime(plan),
            _ => throw new ArgumentOutOfRangeException(nameof(pickupMode), pickupMode, null)
        };
    }

    private static StacksState ExecutePlanOneAtATime(CranePlan plan)
    {
        var stacksMap = plan.InitialStacksState.StackMap;
        
        foreach (var instruction in plan.Instructions)
        {
            for (var i = 0; i < instruction.NumMoves; i++)
            {
                stacksMap[instruction.DestinationStack].Push(stacksMap[instruction.SourceStack].Pop());
            }
        }

        return new StacksState(stacksMap);
    }
    
    private static StacksState ExecutePlanManyAtATime(CranePlan plan)
    {
        var stacksMap = plan.InitialStacksState.StackMap;
        var buffer = new Stack<char>();
        
        foreach (var instruction in plan.Instructions)
        {
            for (var i = 0; i < instruction.NumMoves; i++)
            {
                buffer.Push(stacksMap[instruction.SourceStack].Pop());
            }

            while (buffer.Count > 0)
            {
                stacksMap[instruction.DestinationStack].Push(buffer.Pop());
            }
        }

        return new StacksState(stacksMap);
    }
}