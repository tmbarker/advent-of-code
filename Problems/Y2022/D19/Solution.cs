using Problems.Attributes;
using Problems.Y2022.Common;
using System.Text.RegularExpressions;

namespace Problems.Y2022.D19;

/// <summary>
/// Not Enough Minerals: https://adventofcode.com/2022/day/19
/// </summary>
[Favourite("Not Enough Minerals", Topics.Graphs|Topics.Recursion, Difficulty.Hard)]
public class Solution : SolutionBase2022
{
    private const int TimeLimitPart1 = 24;
    private const int TimeLimitPart2 = 32;
    private const int NumBlueprintsPart2 = 3;

    private static readonly HashSet<Materials> MaterialTypes = new()
    {
        Materials.Ore,
        Materials.Clay,
        Materials.Obsidian,
        Materials.Geode,
    };

    public override int Day => 19;
    
    public override object Run(int part)
    {
        var blueprints = ParseBlueprints(GetInputLines());
        return part switch
        {
            1 => EvaluateQualityLevels(blueprints, TimeLimitPart1),
            2 => ComputeBlueprintProducts(blueprints.Take(NumBlueprintsPart2), TimeLimitPart2),
            _ => ProblemNotSolvedString
        };
    }

    private static int EvaluateQualityLevels(IEnumerable<Blueprint> blueprints, int timeLimit)
    {
        return blueprints.Sum(b => ComputeQualityLevel(b, timeLimit));
    }

    private static int ComputeBlueprintProducts(IEnumerable<Blueprint> blueprints, int timeLimit)
    {
        var maxGeodes = new List<int>();
        foreach (var blueprint in blueprints)
        {
            maxGeodes.Add(FindMaxGeodes(
                t: timeLimit,
                bp: blueprint,
                inv: Inventory.GetInitial(),
                allowedToBuild: Materials.All));
        }

        return maxGeodes.Aggregate((i, j) => i * j);
    }
    
    private static int ComputeQualityLevel(Blueprint blueprint, int timeLimit)
    {
        var maxBlueprintProduction = FindMaxGeodes(
            t: timeLimit,
            bp: blueprint,
            inv: Inventory.GetInitial(),
            allowedToBuild: Materials.All);
        
        return blueprint.Id * maxBlueprintProduction;
    }

    private static int FindMaxGeodes(int t, Blueprint bp, Inventory inv, Materials allowedToBuild)
    {
        switch (t)
        {
            case <= 0:
                return inv.Funds[Materials.Geode];
            case 1:
                return inv.Funds[Materials.Geode] + inv.Robots[Materials.Geode];
        }
        
        var maxGeodes = 0;
        foreach (var material in MaterialTypes)
        {
            if (IsBuildingReasonable(material, bp, inv, allowedToBuild))
            {
                maxGeodes = Math.Max(maxGeodes, FindMaxGeodes(
                    t: t - 1,
                    bp: bp,
                    inv: inv.AfterBuilding(bp, material),
                    allowedToBuild: Materials.All));
            }
        }

        if (IsWaitingReasonable(bp, inv))
        {
            maxGeodes = Math.Max(maxGeodes, FindMaxGeodes(
                t: t - 1,
                bp: bp,
                inv: inv.AfterWaiting(),
                allowedToBuild: FormAllowedToBuyAfterWaitingMask(bp, inv)));
        }

        return maxGeodes;
    }

    private static bool IsBuildingReasonable(Materials target, Blueprint bp, Inventory inv, Materials allowedToBuild)
    {
        if (!allowedToBuild.HasFlag(target))
        {
            return false;
        }

        if (target != Materials.Geode && inv.Robots[target] >= bp.HighestCosts[target])
        {
            return false;
        }
        
        return CanAffordToBuild(bp.RobotCosts[target], inv.Funds);
    }
    
    private static bool IsWaitingReasonable(Blueprint bp, Inventory inv)
    {
        return !bp.RobotCosts.Values.All(costs => CanAffordToBuild(costs, inv.Funds));
    }

    private static Materials FormAllowedToBuyAfterWaitingMask(Blueprint bp, Inventory inv)
    {
        var mask = Materials.All;
        foreach (var material in inv.Funds.Keys)
        {
            if (CanAffordToBuild(bp.RobotCosts[material], inv.Funds))
            {
                mask &= ~material;
            }
        }
        return mask;
    }

    private static bool CanAffordToBuild(Dictionary<Materials, int> costs, IReadOnlyDictionary<Materials, int> funds)
    {
        foreach (var (material, cost) in costs)
        {
            if (funds[material] < cost)
            {
                return false;
            }
        }
        return true;
    }
    
    private static IEnumerable<Blueprint> ParseBlueprints(IEnumerable<string> input)
    {
        return input.Select(ParseBlueprint);
    }

    private static Blueprint ParseBlueprint(string input)
    {
        var matches = Regex.Matches(input, @"\d+");
        return new Blueprint
        {
            Id = int.Parse(matches[0].Value),
            RobotCosts = new Dictionary<Materials, Dictionary<Materials, int>>
            {
                { Materials.Ore , new Dictionary<Materials, int>
                {
                    { Materials.Ore, int.Parse(matches[1].Value)}
                }},
                { Materials.Clay , new Dictionary<Materials, int>
                {
                    { Materials.Ore, int.Parse(matches[2].Value)}
                }},
                { Materials.Obsidian , new Dictionary<Materials, int>
                {
                    { Materials.Ore, int.Parse(matches[3].Value)},
                    { Materials.Clay, int.Parse(matches[4].Value)}
                }},
                { Materials.Geode , new Dictionary<Materials, int>
                {
                    { Materials.Ore, int.Parse(matches[5].Value)},
                    { Materials.Obsidian, int.Parse(matches[6].Value)}
                }},
            }
        };
    }
}