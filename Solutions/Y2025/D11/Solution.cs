using Utilities.Graph;

namespace Solutions.Y2025.D11;

using Memo = Dictionary<(string From, string To), long>;

[PuzzleInfo("Reactor", Topics.Graphs, Difficulty.Medium, favourite: true)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var dag = new DirectedGraph<string>();
        var memo = new Memo();
        
        foreach (var chunks in ParseInputLines(line => line.Split(": ")))
        foreach (var adjacent in chunks[1].Split(' '))
        {
            dag.AddEdge(from: chunks[0], to: adjacent);
        }
        
        return part switch
        {
            1 => Part1(graph: dag, memo),
            2 => Part2(graph: dag, memo),
            _ => PuzzleNotSolvedString
        };
    }

    private static long Part1(DirectedGraph<string> graph, Memo memo)
    {
        return Paths(graph, "you", "out", memo);
    }
    
    private static long Part2(DirectedGraph<string> graph, Memo memo)
    {
        return Paths(graph, "svr", "dac", memo) * Paths(graph, "dac", "fft", memo) * Paths(graph, "fft", "out", memo) +
               Paths(graph, "svr", "fft", memo) * Paths(graph, "fft", "dac", memo) * Paths(graph, "dac", "out", memo);
    }
    
    private static long Paths(DirectedGraph<string> graph, string from, string to, Memo memo)
    {
        if (memo.TryGetValue((from, to), out var cached))
        {
            return cached;
        }
        
        if (from == to)
        {
            return memo[(from, to)] = 1L;
        }

        return memo[(from, to)] = graph.Outgoing[from].Sum(adjacent => Paths(graph, from: adjacent, to, memo));
    }
}