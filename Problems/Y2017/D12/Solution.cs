using System.Text.RegularExpressions;
using Problems.Common;
using Utilities.Collections;
using Utilities.Extensions;

namespace Problems.Y2017.D12;

/// <summary>
/// Digital Plumber: https://adventofcode.com/2017/day/12
/// </summary>
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();
        var adjacency = ParseAdjacency(input);
        
        return part switch
        {
            1 => CountElements(adjacency, id: 0),
            2 => CountSetPartitions(adjacency),
            _ => ProblemNotSolvedString
        };
    }

    private static int CountElements(IDictionary<int, HashSet<int>> adjacency, int id)
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

        return visited.Count;
    }

    private static int CountSetPartitions(IDictionary<int, HashSet<int>> adjacency)
    {
        var disjointSet = new DisjointSet<int>();

        foreach (var (element, adjacencies) in adjacency)
        {
            disjointSet.MakeSet(element);
            foreach (var adjacent in adjacencies)
            {
                disjointSet.MakeSet(adjacent);
                disjointSet.Union(element, adjacent);
            }
        }

        return disjointSet.PartitionsCount;
    }

    private static IDictionary<int, HashSet<int>> ParseAdjacency(IEnumerable<string> input)
    {
        var regex = new Regex(@"(?<Id>\d+) <->(?: (?<Adj>\d+),?)+");
        var adjacency = new Dictionary<int, HashSet<int>>();

        foreach (var line in input)
        {
            var match = regex.Match(line);
            var id = match.Groups["Id"].ParseInt();
            var adjacencies = match.Groups["Adj"].ParseInts();

            adjacency[id] = new HashSet<int>(adjacencies);
        }

        return adjacency;
    }
}