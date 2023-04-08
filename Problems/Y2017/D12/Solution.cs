using System.Text.RegularExpressions;
using Problems.Y2017.Common;
using Utilities.Extensions;

namespace Problems.Y2017.D12;

/// <summary>
/// Digital Plumber: https://adventofcode.com/2017/day/12
/// </summary>
public class Solution : SolutionBase2017
{
    public override int Day => 12;
    
    public override object Run(int part)
    {
        var input = GetInputLines();
        var adjacency = ParseAdjacency(input);
        
        return part switch
        {
            1 => CountNodes(adjacency, id: 0),
            2 => CountGraphs(adjacency),
            _ => ProblemNotSolvedString
        };
    }

    private static int CountNodes(IDictionary<int, HashSet<int>> adjacency, int id)
    {
        return GetConnectedNodes(adjacency, id).Count;
    }

    private static int CountGraphs(IDictionary<int, HashSet<int>> adjacency)
    {
        var count = 0;
        while (adjacency.Any())
        {
            var node = adjacency.Keys.First();
            var nodes = GetConnectedNodes(adjacency, node);

            foreach (var id in nodes)
            {
                adjacency.Remove(id);
            }
            
            count++;
        }

        return count;
    }

    private static HashSet<int> GetConnectedNodes(IDictionary<int, HashSet<int>> adjacency, int id)
    {
        var visited = new HashSet<int> { id };
        var queue = new Queue<int>(new[] { id });

        while (queue.Any())
        {
            var current = queue.Dequeue();
            foreach (var adj in adjacency[current])
            {
                if (visited.Add(adj))
                {
                    queue.Enqueue(adj);
                }
            }
        }

        return visited;
    }

    private static IDictionary<int, HashSet<int>> ParseAdjacency(IEnumerable<string> input)
    {
        var regex = new Regex(@"(?<Id>\d+) <->(?: (?<Adj>\d+),?)+");
        var adjacency = new Dictionary<int, HashSet<int>>();

        foreach (var line in input)
        {
            var match = regex.Match(line);
            var id = match.Groups["Id"].ParseInt();
            var adjacencies = match.Groups["Adj"].Captures.Select(c => c.ParseInt());

            adjacency[id] = new HashSet<int>(adjacencies);
        }

        return adjacency;
    }
}