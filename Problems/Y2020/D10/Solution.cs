using Problems.Y2020.Common;

namespace Problems.Y2020.D10;

/// <summary>
/// Adapter Array: https://adventofcode.com/2020/day/10
/// </summary>
public class Solution : SolutionBase2020
{
    private const int Range = 3;
    private static readonly IReadOnlySet<int> DifferencesOfInterest = new HashSet<int> { 1, 3 };

    public override int Day => 10;
    
    public override object Run(int part)
    {
        var sortedAdapters = GetSortedAdapters(GetInputLines());
        return part switch
        {
            1 => ComputeAdapterDifferencesProduct(sortedAdapters),
            2 => CountValidAdapterArrangements(sortedAdapters),
            _ => ProblemNotSolvedString
        };
    }

    private static int ComputeAdapterDifferencesProduct(IReadOnlyList<int> sortedAdapters)
    {
        var differences = DifferencesOfInterest.ToDictionary(
            keySelector: d => d,
            elementSelector: _ => 0);

        for (var i = 1; i < sortedAdapters.Count; i++)
        {
            if (differences.ContainsKey(sortedAdapters[i] - sortedAdapters[i - 1]))
            {
                differences[sortedAdapters[i] - sortedAdapters[i - 1]]++;
            }
        }

        return differences.Values.Aggregate((i, j) => i * j);
    }

    private static long CountValidAdapterArrangements(IReadOnlyCollection<int> sortedAdapters)
    {
        return CountPaths(
            from: sortedAdapters.First(), 
            to: sortedAdapters.Last(), 
            nodes: sortedAdapters.ToHashSet(), 
            memo: new Dictionary<int, long>());
    }

    private static long CountPaths(int from, int to, IReadOnlySet<int> nodes, IDictionary<int, long> memo)
    {
        if (from == to)
        {
            return 1;
        }

        var count = 0L;
        for (var i = 1; i <= Range; i++)
        {
            var next = from + i;
            if (nodes.Contains(next))
            {
                count += memo.TryGetValue(next, out var value) 
                    ? value 
                    : CountPaths(next, to, nodes, memo);
            }
        }

        memo[from] = count;
        return count;
    }

    private static List<int> GetSortedAdapters(IEnumerable<string> input)
    {
        var elements = input.Select(int.Parse).ToList();
        elements.Add(0);
        elements.Add(elements.Max() + Range);
        
        elements.Sort();
        return elements;
    }
}