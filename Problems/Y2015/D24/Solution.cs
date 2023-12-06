using Problems.Common;

namespace Problems.Y2015.D24;

/// <summary>
/// It Hangs in the Balance: https://adventofcode.com/2015/day/24
/// </summary>
public sealed class Solution : SolutionBase
{
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
            _ => ProblemNotSolvedString
        };
    }

    private static long Search(IList<long> numbers, long target)
    {
        var initial = new State(Index: -1, Include: false, Sum: 0, Remaining: numbers.Sum());
        var parentMap = new Dictionary<State, State>();
        var groups = new List<HashSet<long>>();
        var stack = new Stack<State>(new[] { initial });

        while (stack.Any())
        {
            var state = stack.Pop();
            if (state.Sum == target)
            {
                groups.Add(BacktrackGroup(head: state, parentMap, numbers));
                continue;
            }

            if (state.Sum > target || state.Remaining < target - state.Sum || state.Index >= numbers.Count - 1)
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
        var group = new HashSet<long> { numbers[head.Index] };
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

    private readonly record struct State(int Index, bool Include, long Sum, long Remaining);
}