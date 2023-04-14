using System.Text.RegularExpressions;
using Problems.Common;
using Utilities.Extensions;
using Utilities.Graph;

namespace Problems.Y2017.D07;

/// <summary>
/// Recursive Circus: https://adventofcode.com/2017/day/7
/// </summary>
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();
        var tower = ParseTower(input);
        
        return part switch
        {
            1 => tower.Root,
            2 => tower.Balance(),
            _ => ProblemNotSolvedString
        };
    }

    private static Tower ParseTower(IEnumerable<string> input)
    {
        var regex = new Regex(@"(?<Id>[a-z]+) \((?<Weight>\d+)\)(?: ->)?(?: (?<Edges>[a-z]+),?)*");
        var weights = new Dictionary<string, int>();
        var adjacency = new Dictionary<string, HashSet<string>>();
        var edges = new List<DirectedGraph<string>.Edge>();

        foreach (var line in input)
        {
            var match = regex.Match(line);
            var id = match.Groups["Id"].Value;
            var weight = match.Groups["Weight"].ParseInt();
            var adjacencies = match.Groups["Edges"].Captures.Select(c => c.Value);
            
            weights.Add(id, weight);
            foreach (var adj in adjacencies)
            {
                adjacency.TryAdd(id, new HashSet<string>());
                adjacency[id].Add(adj);
            }
        }

        foreach (var (source, sinks) in adjacency)
        foreach (var sink in sinks)
        {
            edges.Add(new DirectedGraph<string>.Edge(From: source, To: sink));
        }

        var graph = new DirectedGraph<string>(edges);
        var tower = new Tower(graph, weights);

        return tower;
    }
}