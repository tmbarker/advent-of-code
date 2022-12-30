using Problems.Y2021.Common;

namespace Problems.Y2021.D12;

/// <summary>
/// Passage Pathing: https://adventofcode.com/2021/day/12
/// </summary>
public class Solution : SolutionBase2021
{
    private const char Delimiter = '-';

    public override int Day => 12;
    
    public override object Run(int part)
    {
        return part switch
        {
            0 => CountPaths(false),
            1 => CountPaths(true),
            _ => ProblemNotSolvedString,
        };
    }

    private int CountPaths(bool bonusSmallCaveVisit)
    {
        var adjacencyMap = ParseAdjacencyMap(GetInputLines());
        var caveTraverser = new CaveTraverser(adjacencyMap, bonusSmallCaveVisit);
        var numPaths = 0;

        void OnPathFound()
        {
            numPaths++;
        }

        caveTraverser.PathFound += OnPathFound;
        caveTraverser.FindPaths();

        return numPaths;
    }

    private static Dictionary<string, HashSet<string>> ParseAdjacencyMap(IEnumerable<string> lines)
    {
        var adjacencyMap = new Dictionary<string, HashSet<string>>();
        foreach (var line in lines)
        {
            var vertices = line.Split(Delimiter);
            var v1 = vertices[0];
            var v2 = vertices[1];
            
            if (!adjacencyMap.ContainsKey(v1))
            {
                adjacencyMap.Add(v1, new HashSet<string>());
            }
            if (!adjacencyMap.ContainsKey(v2))
            {
                adjacencyMap.Add(v2, new HashSet<string>());
            }

            adjacencyMap[v1].Add(v2);
            adjacencyMap[v2].Add(v1);
        }
        return adjacencyMap;
    }
}