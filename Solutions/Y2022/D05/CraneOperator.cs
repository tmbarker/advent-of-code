namespace Solutions.Y2022.D05;

public static class CraneOperator
{
    public static Dictionary<int, Stack<char>> ExecutePlan(CranePlan plan, PickupMode pickupMode)
    {
        return pickupMode switch
        {
            PickupMode.OneAtATime => ExecutePlanOneAtATime(plan),
            PickupMode.ManyAtATime => ExecutePlanManyAtATime(plan),
            _ => throw new NoSolutionException()
        };
    }

    private static Dictionary<int, Stack<char>> ExecutePlanOneAtATime(CranePlan plan)
    {
        var stacksMap = plan.InitialStacksState;
        foreach (var instruction in plan.Instructions)
        {
            for (var i = 0; i < instruction.NumMoves; i++)
            {
                stacksMap[instruction.DestinationStack].Push(stacksMap[instruction.SourceStack].Pop());
            }
        }
        return stacksMap;
    }
    
    private static Dictionary<int, Stack<char>> ExecutePlanManyAtATime(CranePlan plan)
    {
        var stacksMap = plan.InitialStacksState;
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

        return stacksMap;
    }
}