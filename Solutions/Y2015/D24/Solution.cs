namespace Solutions.Y2015.D24;

[PuzzleInfo("It Hangs in the Balance", Topics.Graphs|Topics.Math, Difficulty.Hard)]
public sealed class Solution : SolutionBase
{
    private readonly record struct State(int Index, bool Include, long Sum, long Remaining);
    
    public override object Run(int part)
    {
        var input = GetInputLines();
        var weights = input
            .Select(long.Parse)
            .OrderDescending()
            .ToArray();
        
        return part switch
        {
            1 => Search(weights, target: weights.Sum() / 3),
            2 => Search(weights, target: weights.Sum() / 4),
            _ => PuzzleNotSolvedString
        };
    }

    private static long Search(long[] numbers, long target)
    {
        var initial = new State(Index: -1, Include: false, Sum: 0, Remaining: numbers.Sum());
        var parentMap = new Dictionary<State, State>();
        var groups = new List<HashSet<long>>();
        var stack = new Stack<State>([initial]);

        while (stack.Count > 0)
        {
            var state = stack.Pop();
            if (state.Sum == target)
            {
                groups.Add(item: BacktrackGroup(head: state, parentMap, numbers));
                continue;
            }

            if (state.Sum > target || state.Remaining < target - state.Sum || state.Index >= numbers.Length - 1)
            {
                continue;
            }

            var omit = new State(
                Index: state.Index + 1,
                Include: false,
                Sum: state.Sum,
                Remaining: state.Remaining - numbers[state.Index + 1]);
            var take = new State(
                Index: state.Index + 1,
                Include: true,
                Sum: state.Sum + numbers[state.Index + 1],
                Remaining: state.Remaining - numbers[state.Index + 1]);

            parentMap[omit] = state;
            parentMap[take] = state;
            
            stack.Push(omit);
            stack.Push(take);
        }

        return groups
            .MinBy(g => g, GroupComparer.Instance)!
            .Aggregate(seed: 1L, func: (i, j) => i * j);
    }

    private static HashSet<long> BacktrackGroup(State head, IDictionary<State, State> parentMap, IList<long> numbers)
    {
        var group = new HashSet<long>(collection: [numbers[head.Index]]);
        var current = head;
        
        while (parentMap.TryGetValue(current, out var parent))
        {
            if (parent.Include)
            {
                group.Add(numbers[parent.Index]);
            }
            current = parent;
        }

        return group;
    }
}