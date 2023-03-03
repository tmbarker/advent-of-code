using Problems.Attributes;
using Problems.Y2021.Common;
using Utilities.Extensions;

namespace Problems.Y2021.D12;

/// <summary>
/// Passage Pathing: https://adventofcode.com/2021/day/12
/// </summary>
[Favourite("Passage Pathing", Topics.Graphs|Topics.Recursion, Difficulty.Medium)]
public class Solution : SolutionBase2021
{
    public override int Day => 12;
    
    public override object Run(int part)
    {
        return part switch
        {
            1 => CountPaths(false),
            2 => CountPaths(true),
            _ => ProblemNotSolvedString
        };
    }

    private int CountPaths(bool bonusSmallCaveVisit)
    {
        var adjacencyMap = ParseAdjacencyMap(GetInputLines());
        var caveTraverser = new PathFinder(adjacencyMap, bonusSmallCaveVisit);
        var numPaths = 0;

        void OnPathFound()
        {
            numPaths++;
        }

        caveTraverser.PathFound += OnPathFound;
        caveTraverser.Run();

        return numPaths;
    }

    private static Dictionary<string, HashSet<string>> ParseAdjacencyMap(IEnumerable<string> lines)
    {
        var adjacencyMap = new Dictionary<string, HashSet<string>>();
        foreach (var line in lines)
        {
            var vertices = line.Split('-');
            var v1 = vertices[0];
            var v2 = vertices[1];
            
            adjacencyMap.EnsureContainsKey(v1, new HashSet<string>());
            adjacencyMap.EnsureContainsKey(v2, new HashSet<string>());
            
            adjacencyMap[v1].Add(v2);
            adjacencyMap[v2].Add(v1);
        }
        return adjacencyMap;
    }
}