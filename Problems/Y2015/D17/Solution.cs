using Problems.Common;

namespace Problems.Y2015.D17;

/// <summary>
/// No Such Thing as Too Much: https://adventofcode.com/2015/day/17
/// </summary>
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();
        var cups = ParseCups(input);
        var combinations = GetCombinations(cups, total: 150);
        
        return part switch
        {
            1 => combinations.Count,
            2 => combinations
                .GroupBy(state => state.NumUsed)
                .OrderBy(group => group.Key)
                .First()
                .Count(),
            _ => ProblemNotSolvedString
        };
    }

    private static HashSet<State> GetCombinations(IEnumerable<Cup> cups, int total)
    {
        var initial = new State(
            unused: new HashSet<Cup>(cups),
            numUsed: 0,
            totalVolume: 0); 
        
        var combinations = new HashSet<State>();
        var queue = new Queue<State>(new[] { initial });
        var visited = new HashSet<State> { initial };

        while (queue.Any())
        {
            var state = queue.Dequeue();
            foreach (var unused in state.Unused)
            {
                var next = state.AfterUsing(unused);
                if (next.TotalVolume > total || visited.Contains(next))
                {
                    continue;
                }

                if (next.TotalVolume == total)
                {
                    visited.Add(next);
                    combinations.Add(next);
                    continue;
                }

                visited.Add(next);
                queue.Enqueue(next);
            }
        }

        return combinations;
    }

    private static IEnumerable<Cup> ParseCups(IEnumerable<string> input)
    {
        return input.Select((line, index) => new Cup(Id: index, Size: int.Parse(line)));
    }
}