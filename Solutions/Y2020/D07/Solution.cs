using System.Text.RegularExpressions;
using Utilities.Extensions;

namespace Solutions.Y2020.D07;

using ContentMap = IReadOnlyDictionary<string, IList<BagContent>>;
using Memo = IDictionary<(string, string), bool>;

[PuzzleInfo("Handy Haversacks", Topics.Graphs|Topics.Recursion, Difficulty.Medium, favourite: true)]
public sealed class Solution : SolutionBase
{
    private const string ShinyGold = "shiny gold";
    
    public override object Run(int part)
    {
        var map = ParseCapacityMap(GetInputLines());
        return part switch
        {
            1 => CountBagsThatCanContain(targetColour: ShinyGold, map),
            2 => CountBagsInside(targetColour: ShinyGold, map),
            _ => ProblemNotSolvedString
        };
    }

    private static int CountBagsThatCanContain(string targetColour, ContentMap map)
    {
        var memo = new Dictionary<(string, string), bool>();
        return map.Keys.Count(bagColour => CheckBagCanContain(bagColour, targetColour, map, memo));
    }

    private static int CountBagsInside(string targetColour, ContentMap map)
    {
        var containedCount = 0;
        var containedBags = map[targetColour];
        
        foreach (var (count, colour) in containedBags)
        {
            containedCount += count;
            containedCount += count * CountBagsInside(colour, map);
        }

        return containedCount;
    }

    private static bool CheckBagCanContain(string bagColour, string targetColour, ContentMap map, Memo memo)
    {
        var key = (bagColour, targetColour);
        if (memo.TryGetValue(key, out var result))
        {
            return result;
        }

        memo[key] = map[bagColour]
            .Select(c => c.Colour)
            .Any(c => c == targetColour || CheckBagCanContain(c, targetColour, map, memo));
        
        return memo[key];
    }
    
    private static ContentMap ParseCapacityMap(IEnumerable<string> input)
    {
        var capacityMap = new Dictionary<string, IList<BagContent>>();
        foreach (var line in input)
        {
            var colour = Regex.Match(line, @"([a-z ]+) bags contain").Groups[1].Value;
            var contents = Regex.Matches(line, @"(\d+) ([a-z ]+) bags?[\.|,]");

            capacityMap.Add(colour, new List<BagContent>());
            foreach (Match match in contents)
            {
                capacityMap[colour].Add(new BagContent(
                    Count: match.Groups[1].ParseInt(), 
                    Colour: match.Groups[2].Value));
            }
        }
        return capacityMap;
    }
}