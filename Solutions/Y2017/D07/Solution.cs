using System.Text.RegularExpressions;
using Utilities.Collections;
using Utilities.Extensions;
using Utilities.Graph;

namespace Solutions.Y2017.D07;

[PuzzleInfo("Recursive Circus", Topics.Graphs, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();
        var tower = ParseTower(input);
        
        return part switch
        {
            1 => tower.Root,
            2 => tower.Balance(),
            _ => PuzzleNotSolvedString
        };
    }

    private static Tower ParseTower(IEnumerable<string> input)
    {
        var weights = new Dictionary<string, int>();
        var adjacency = new DefaultDict<string, HashSet<string>>(defaultSelector: _ => []);
        var edges = new List<DirectedGraph<string>.Edge>();

        foreach (var line in input)
        {
            var match = Regex.Match(line, @"(?<Id>[a-z]+) \((?<Weight>\d+)\)(?: ->)?(?: (?<Edges>[a-z]+),?)*");
            var id = match.Groups["Id"].Value;
            var weight = match.Groups["Weight"].ParseInt();
            var adjacencies = match.Groups["Edges"].Captures.Select(c => c.Value);
            
            weights.Add(id, weight);
            foreach (var adj in adjacencies)
            {
                adjacency[id].Add(adj);
            }
        }

        foreach (var (source, sinks) in adjacency)
        foreach (var sink in sinks)
        {
            edges.Add(item: new DirectedGraph<string>.Edge(From: source, To: sink));
        }

        var graph = new DirectedGraph<string>(edges);
        var tower = new Tower(graph, weights);

        return tower;
    }
}