using Problems.Y2020.Common;
using System.Text.RegularExpressions;

namespace Problems.Y2020.D07;

using ContentMap = IReadOnlyDictionary<string, IList<BagContent>>;

/// <summary>
/// Handy Haversacks: https://adventofcode.com/2020/day/7
/// </summary>
public class Solution : SolutionBase2020
{
    private const string ShinyGold = "shiny gold";
    
    public override int Day => 7;
    
    public override object Run(int part)
    {
        var map = ParseCapacityMap(GetInputLines());
        return part switch
        {
            0 => CountBagsThatCanContain(ShinyGold, map),
            1 => CountBagsInside(ShinyGold, map),
            _ => ProblemNotSolvedString,
        };
    }

    private static int CountBagsThatCanContain(string targetColour, ContentMap map)
    {
        return map.Keys.Count(bagColour => CheckBagCanContain(bagColour, targetColour, map));
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

    private static bool CheckBagCanContain(string bagColour, string targetColour, ContentMap map)
    {
        var colours = map[bagColour].Select(c => c.Colour);
        return colours.Any(c => c == targetColour || CheckBagCanContain(c, targetColour, map));
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
                    count: int.Parse(match.Groups[1].Value), 
                    colour: match.Groups[2].Value));
            }
        }
        return capacityMap;
    }
}