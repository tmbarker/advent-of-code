using Utilities.Extensions;

namespace Solutions.Y2015.D09;

[PuzzleInfo("All in a Single Night", Topics.Graphs, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private readonly record struct Route(string From, string To, int Cost);
    
    public override object Run(int part)
    {
        var input = GetInputLines();
        var routes = input.Select(ParseRoute).ToList();
        
        var lookup = new Dictionary<(string, string), int>();
        var places = routes
            .SelectMany(route => new[] { route.From, route.To })
            .ToHashSet();
        
        foreach (var route in routes)
        {
            lookup[(route.From, route.To)] = route.Cost;
            lookup[(route.To, route.From)] = route.Cost;
        }
        
        return part switch
        {
            1 => FindMin(places, lookup),
            2 => FindMax(places, lookup),
            _ => ProblemNotSolvedString
        };
    }

    private static int FindMin(HashSet<string> places, IDictionary<(string, string), int> lookup)
    {
        var min = int.MaxValue;
        foreach (var place in places)
        {
            min = Math.Min(min, FindMin(
                current: place,
                cost: 0,
                unvisited: places.Except(place).ToHashSet(),
                lookup: lookup));
        }

        return min;
    }
    
    private static int FindMax(HashSet<string> places, IDictionary<(string, string), int> lookup)
    {
        var max = int.MinValue;
        foreach (var place in places)
        {
            max = Math.Max(max, FindMax(
                current: place,
                cost: 0,
                unvisited: places.Except(place).ToHashSet(),
                lookup: lookup));
        }

        return max;
    }

    private static int FindMin(string current, int cost, HashSet<string> unvisited,
        IDictionary<(string, string), int> lookup)
    {
        if (unvisited.Count == 0)
        {
            return cost;
        }
        
        var min = int.MaxValue;
        foreach (var place in unvisited)
        {
            min = Math.Min(min, FindMin(
                current: place,
                cost: cost + lookup[(current, place)],
                unvisited: unvisited.Except(place).ToHashSet(),
                lookup: lookup));
        }

        return min;
    }
    
    private static int FindMax(string current, int cost, HashSet<string> unvisited,
        IDictionary<(string, string), int> lookup)
    {
        if (unvisited.Count == 0)
        {
            return cost;
        }
        
        var max = int.MinValue;
        foreach (var place in unvisited)
        {
            max = Math.Max(max, FindMax(
                current: place,
                cost: cost + lookup[(current, place)],
                unvisited: unvisited.Except(place).ToHashSet(),
                lookup: lookup));
        }

        return max;
    }

    private static Route ParseRoute(string line)
    {
        var tokens = line.Split(' ');
        return new Route(From: tokens[0], To: tokens[2], Cost: int.Parse(tokens[4]));
    }
}