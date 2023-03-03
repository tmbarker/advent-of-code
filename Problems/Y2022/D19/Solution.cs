using Problems.Attributes;
using Problems.Y2022.Common;
using Utilities.Extensions;

namespace Problems.Y2022.D19;

/// <summary>
/// Not Enough Minerals: https://adventofcode.com/2022/day/19
/// </summary>
[Favourite("Not Enough Minerals", Topics.Graphs|Topics.Recursion, Difficulty.Hard)]
public class Solution : SolutionBase2022
{
    private static readonly HashSet<Materials> MaterialTypes = new()
    {
        Materials.Ore,
        Materials.Clay,
        Materials.Obsidian,
        Materials.Geode
    };

    public override int Day => 19;
    
    public override object Run(int part)
    {
        var blueprints = ParseInputLines(parseFunc: ParseBlueprint);
        return part switch
        {
            1 => EvaluateQualityLevels(blueprints, timeLimit: 24),
            2 => ComputeBlueprintProducts(blueprints.Take(count: 3), timeLimit: 32),
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

    private static Blueprint ParseBlueprint(string input)
    {
        var numbers = input.Numbers();
        return new Blueprint
        {
            Id = numbers[0],
            RobotCosts = new Dictionary<Materials, Dictionary<Materials, int>>
            {
                { Materials.Ore , new Dictionary<Materials, int>
                {
                    { Materials.Ore, numbers[1]}
                }},
                { Materials.Clay , new Dictionary<Materials, int>
                {
                    { Materials.Ore, numbers[2]}
                }},
                { Materials.Obsidian , new Dictionary<Materials, int>
                {
                    { Materials.Ore, numbers[3]},
                    { Materials.Clay, numbers[4]}
                }},
                { Materials.Geode , new Dictionary<Materials, int>
                {
                    { Materials.Ore, numbers[5]},
                    { Materials.Obsidian, numbers[6]}
                }},
            }
        };
    }
}