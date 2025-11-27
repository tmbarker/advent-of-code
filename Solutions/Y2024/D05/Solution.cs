using Utilities.Extensions;
using Utilities.Graph;

namespace Solutions.Y2024.D05;

[PuzzleInfo("Print Queue", Topics.Graphs, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var chunks = ChunkInputByNonEmpty();
        var graph = new DirectedGraph<int>();
        var total = 0;

        foreach (var rule in chunks[0].Select(line => line.ParseInts()))
        {
            graph.AddEdge(from: rule[0], to: rule[1]);
        }
        
        foreach (var update in chunks[1].Select(line => line.ParseInts()))
        {
            var correct = true;
            for (var i = 0; i < update.Length; i++)
            {
                correct &= update.Where((_, j) => j < i).All(n => graph.Outgoing[n].Contains(update[i]));
                correct &= update.Where((_, j) => j > i).All(n => graph.Outgoing[update[i]].Contains(n));
            }

            switch (correct)
            {
                case true when part == 1:
                    total += update[update.Length / 2];
                    break;
                case false when part == 2:
                    var sorted = Order(graph, update);
                    total += sorted[sorted.Count / 2];
                    break;
            }
        }

        return total;
    }

    private static List<int> Order(DirectedGraph<int> graph, IList<int> numbers)
    {
        var sorted = new List<int>();
        var unsorted = new List<int>(numbers);
        
        while (unsorted.Count != 0)
        {
            var next = unsorted.Single(candidate =>
                unsorted.All(other => candidate == other || graph.Outgoing[candidate].Contains(other)));
            
            sorted.Add(next);
            unsorted.Remove(next);
        }
        
        return sorted;
    }
}