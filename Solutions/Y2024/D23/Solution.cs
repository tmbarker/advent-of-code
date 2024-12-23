using Utilities.Collections;

namespace Solutions.Y2024.D23;

using AdjacencyList = DefaultDict<string, HashSet<string>>;

[PuzzleInfo("LAN Party", Topics.Graphs, Difficulty.Medium)]
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var edges = ParseInputLines(parseFunc: line => line.Split("-"));
        var graph = new AdjacencyList(defaultSelector: _ => []);

        foreach (var vertices in edges)
        {
            graph[vertices[0]].Add(vertices[1]);
            graph[vertices[1]].Add(vertices[0]);
        }
        
        return part switch
        {
            1 => CountTriangles(graph),
            2 => CountMaxClique(graph),
            _ => PuzzleNotSolvedString
        };
    }
    
    private static int CountTriangles(AdjacencyList graph)
    {
        var count = 0;

        foreach (var u in graph.Keys)
        foreach (var v in graph[u])
        foreach (var w in graph[u].Intersect(graph[v]))
        {
            if (string.CompareOrdinal(u, v) >= 0) continue;
            if (string.CompareOrdinal(v, w) >= 0) continue;
            
            if (new[] { u, v, w }.Any(vertex => vertex.StartsWith('t')))
            {
                count++;
            }
        }

        return count;
    }
    
    private static string CountMaxClique(AdjacencyList graph)
    {
        var cliques = new List<HashSet<string>>();
        BronKerbosch(graph, r: [], p: [..graph.Keys], x: [], cliques);
        var max = cliques.MaxBy(clique => clique.Count);
        var str = string.Join(",", max!.Order());
        return str;
    }
    
    private static void BronKerbosch(AdjacencyList graph, HashSet<string> r, HashSet<string> p, HashSet<string> x,
        List<HashSet<string>> cliques)
    {
        if (p.Count == 0 && x.Count == 0)
        {
            cliques.Add([..r]);
            return;
        }

        var pivot = p.Concat(x).First();
        var nonNeighbors = new HashSet<string>(p.Except(graph[pivot]));

        foreach (var v in nonNeighbors)
        {
            var newR = new HashSet<string>(r) { v };
            var newP = new HashSet<string>(p.Intersect(graph[v]));
            var newX = new HashSet<string>(x.Intersect(graph[v]));
            BronKerbosch(graph, newR, newP, newX, cliques);
            p.Remove(v);
            x.Add(v);
        }
    }
}