using Utilities.Collections;

namespace Solutions.Y2023.D25;

using Graph     = DefaultDict<string, HashSet<string>>;
using Component = HashSet<string>;

[PuzzleInfo("Snowverload", Topics.Graphs, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override int Parts => 1;

    public override object Run(int part)
    {
        var graph = new Graph(defaultSelector: _ => []); 
        foreach (var line in GetInputLines())
        {
            var tokens = line.Split(separator: " ");
            var vertex = tokens[0][..^1];
            
            for (var i = 1; i < tokens.Length; i++)
            {
                graph[vertex].Add(tokens[i]);
                graph[tokens[i]].Add(vertex);
            }
        }
        
        foreach (var seed in graph.Keys)
        {
            if (FindComponent(graph, seed, numCutEdges: 3, out var component))
            {
                return component.Count * (graph.Count - component.Count);
            }
        }
        
        throw new NoSolutionException();
    }

    private static bool FindComponent(Graph graph, string seed, int numCutEdges, out Component component)
    {
        component = [seed];
        var cutEdgeCandidates = graph[seed]
            .Select(node => (From: seed, To: node))
            .ToHashSet();

        while (cutEdgeCandidates.Count > numCutEdges)
        {
            var set = component;
            var next = cutEdgeCandidates
                .Select(edge => edge.To)
                .MinBy(node => graph[node].Sum(adj => set.Contains(adj) ? -1 : 1))!;
            
            component.Add(next);
            foreach (var vertex in graph[next])
            {
                if (component.Contains(vertex))
                {
                    cutEdgeCandidates.Remove((From: vertex, To: next));
                }
                else
                {
                    cutEdgeCandidates.Add((From: next, To: vertex));
                }
            }
        }

        return component.Count < graph.Count;
    }
}