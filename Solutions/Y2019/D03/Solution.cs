using Utilities.Geometry.Euclidean;

namespace Solutions.Y2019.D03;

using Route = string[];
using PathCosts = Dictionary<Vec2D, int>;

[PuzzleInfo("Crossed Wires", Topics.Vectors, Difficulty.Easy, favourite: true)]
public sealed class Solution : SolutionBase
{
    private static readonly Dictionary<char, Vec2D> Directions = new()
    {
        {'U', Vec2D.Up},
        {'D', Vec2D.Down},
        {'L', Vec2D.Left},
        {'R', Vec2D.Right}
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
            _ => PuzzleNotSolvedString
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
        return costs.W1.Keys
            .Where(p => costs.W2.ContainsKey(p))
            .Min(p => costs.W1[p] + costs.W2[p]);
    }

    private static (PathCosts W1, PathCosts W2) GetPathCosts((Route W1, Route W2) routes)
    {
        return (GetPathCosts(routes.W1), GetPathCosts(routes.W2));
    }
    
    private static PathCosts GetPathCosts(Route instructions)
    {
        var map = new PathCosts();
        var pos = Vec2D.Zero;
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

    private static (Route W1, Route W2) ParseWireRoutes(string[] input)
    {
        var w1 = input[0].Split(separator: ',');
        var w2 = input[1].Split(separator: ',');
        return (w1, w2);
    }
}