using Problems.Attributes;
using Problems.Common;
using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Problems.Y2019.D03;

using Route = IEnumerable<string>;
using PathCosts = IDictionary<Vector2D, int>;

/// <summary>
/// Crossed Wires: https://adventofcode.com/2019/day/3
/// </summary>
[Favourite("Crossed Wires", Topics.Vectors, Difficulty.Easy)]
public class Solution : SolutionBase
{
    private static readonly Dictionary<char, Vector2D> Directions = new()
    {
        {'U', Vector2D.Up},
        {'D', Vector2D.Down},
        {'L', Vector2D.Left},
        {'R', Vector2D.Right}
    };

    public override object Run(int part)
    {
        var input = GetInputLines();
        var routes = ParseWireRoutes(input);
        var costs = GetPathCosts(routes);
        
        return part switch
        {
            1 => FindClosestWireIntersection(costs),
            2 => FindCheapestIntersection(costs),
            _ => ProblemNotSolvedString
        };
    }

    private static int FindClosestWireIntersection((PathCosts W1, PathCosts W2) costs)
    {
        return costs.W1.Keys
            .Intersect(costs.W2.Keys)
            .Select(i => i.Magnitude(Metric.Taxicab))
            .Min();
    }
    
    private static int FindCheapestIntersection((PathCosts W1, PathCosts W2) costs)
    {
        return costs.W1
            .WhereKeys(p => costs.W2.ContainsKey(p))
            .Select(kvp => kvp.Key)
            .Min(p => costs.W1[p] + costs.W2[p]);
    }

    private static (PathCosts W1, PathCosts W2) GetPathCosts((Route W1, Route W2) routes)
    {
        return (GetPathCosts(routes.W1), GetPathCosts(routes.W2));
    }
    
    private static PathCosts GetPathCosts(Route instructions)
    {
        var map = new Dictionary<Vector2D, int>();
        var pos = Vector2D.Zero;
        var cost = 0;
        
        foreach (var instr in instructions)
        {
            var dir = Directions[instr[0]];
            var count = int.Parse(instr[1..]);

            for (var i = 0; i < count; i++)
            {
                pos += dir;
                cost++;

                map.TryAdd(pos, cost);
            }
        }

        return map;
    }

    private static (Route W1, Route W2) ParseWireRoutes(IList<string> input)
    {
        var w1 = input[0].Split(separator: ',');
        var w2 = input[1].Split(separator: ',');
        return (w1, w2);
    }
}