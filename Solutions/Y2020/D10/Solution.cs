namespace Solutions.Y2020.D10;

[PuzzleInfo("Adapter Array", Topics.Recursion, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private const int Range = 3;
    private static readonly IReadOnlySet<int> DifferencesOfInterest = new HashSet<int> { 1, 3 };

    public override object Run(int part)
    {
        var input = GetInputLines();
        var sortedAdapters = GetSortedAdapters(input);
        
        return part switch
        {
            1 => ComputeAdapterDifferencesProduct(sortedAdapters),
            2 => CountValidAdapterArrangements(sortedAdapters),
            _ => PuzzleNotSolvedString
        };
    }

    private static int ComputeAdapterDifferencesProduct(List<int> sortedAdapters)
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
        elements.Add(item: 0);
        elements.Add(item: elements.Max() + Range);
        
        elements.Sort();
        return elements;
    }
}