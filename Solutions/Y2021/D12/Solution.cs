namespace Solutions.Y2021.D12;

[PuzzleInfo("Passage Pathing", Topics.Graphs|Topics.Recursion, Difficulty.Medium, favourite: true)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => CountPaths(bonusSmallCaveVisit: false),
            2 => CountPaths(bonusSmallCaveVisit: true),
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
            var vertices = line.Split(separator: '-');
            var v1 = vertices[0];
            var v2 = vertices[1];
            
            adjacencyMap.TryAdd(v1, new HashSet<string>());
            adjacencyMap.TryAdd(v2, new HashSet<string>());
            
            adjacencyMap[v1].Add(v2);
            adjacencyMap[v2].Add(v1);
        }
        return adjacencyMap;
    }
}