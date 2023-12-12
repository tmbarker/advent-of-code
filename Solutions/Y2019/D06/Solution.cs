using Utilities.Graph;

namespace Solutions.Y2019.D06;

using OrbitMap = IDictionary<string, string>;
using Memo = IDictionary<string, int>;

[PuzzleInfo("Universal Orbit Map", Topics.Graphs|Topics.Recursion, Difficulty.Easy, favourite: true)]
public sealed class Solution : SolutionBase
{
    private const string Com = "COM";
    private const string You = "YOU";
    private const string Santa = "SAN";

    public override object Run(int part)
    {
        var map = ParseOrbitMap(GetInputLines());
        return part switch
        {
            1 => CountOrbits(map),
            2 => ComputeTransferCost(map, map[You], map[Santa]),
            _ => ProblemNotSolvedString
        };
    }

    private static int CountOrbits(OrbitMap orbits)
    {
        var memo = new Dictionary<string, int> { { Com, 0 } };
        return orbits.Keys.Sum(body => CountOrbits(body, orbits, memo));
    }

    private static int ComputeTransferCost(OrbitMap map, string from, string to)
    {
        var adjacencyList = new Dictionary<string, HashSet<string>>();
        foreach (var (a, b) in map)
        {
            adjacencyList.TryAdd(a, new HashSet<string>());
            adjacencyList.TryAdd(b, new HashSet<string>());

            adjacencyList[a].Add(b);
            adjacencyList[b].Add(a);
        }

        return GraphHelper.DijkstraUnweighted(from, to, adjacencyList);
    }
    
    private static int CountOrbits(string body, OrbitMap map, Memo memo)
    {
        if (memo.TryGetValue(body, out var orbits))
        {
            return orbits;
        }
        
        memo[body] = 1 + CountOrbits(map[body], map, memo);
        return memo[body];
    }

    private static OrbitMap ParseOrbitMap(IEnumerable<string> input)
    {
        return input
            .Select(line => line.Split(')'))
            .ToDictionary(bodies => bodies[1], bodies => bodies[0]);
    }
}