using Problems.Attributes;
using Problems.Common;

namespace Problems.Y2022.D19;

/// <summary>
/// Not Enough Minerals: https://adventofcode.com/2022/day/19
/// </summary>
[Favourite("Not Enough Minerals", Topics.Graphs|Topics.Recursion, Difficulty.Hard)]
public sealed class Solution : SolutionBase
{
    private static readonly HashSet<Materials> MaterialTypes = new()
    {
        Materials.Ore,
        Materials.Clay,
        Materials.Obsidian,
        Materials.Geode
    };

    public override object Run(int part)
    {
        var blueprints = ParseInputLines(parseFunc: Blueprint.Parse);
        return part switch
        {
            1 => ComputeQualityLevels(blueprints, timeLimit: 24),
            2 => ComputeGeodeProducts(blueprints.Take(count: 3), timeLimit: 32),
            _ => ProblemNotSolvedString
        };
    }

    private static int ComputeQualityLevels(IEnumerable<Blueprint> blueprints, int timeLimit)
    {
        return blueprints.Sum(bp => bp.Id * FindMaxGeodes(
            m: 0,
            t: timeLimit,
            bp: bp,
            inv: Inventory.GetInitial(),
            canBuildMask: Materials.All));
    }

    private static int ComputeGeodeProducts(IEnumerable<Blueprint> blueprints, int timeLimit)
    {
        var maxGeodes = new List<int>();
        foreach (var blueprint in blueprints)
        {
            maxGeodes.Add(FindMaxGeodes(
                m: 0,
                t: timeLimit,
                bp: blueprint,
                inv: Inventory.GetInitial(),
                canBuildMask: Materials.All));
        }

        return maxGeodes.Aggregate((a, b) => a * b);
    }

    private static int FindMaxGeodes(int m, int t, Blueprint bp, Inventory inv, Materials canBuildMask)
    {
        if (t == 1)
        {
            return inv.Robots[Materials.Geode] + inv.Funds[Materials.Geode];
        }
        
        if (GetUpperBound(Materials.Geode, inv, t) <= m)
        {
            return m;
        }

        foreach (var material in MaterialTypes)
        {
            if (IsBuildingReasonable(material, bp, inv, t, canBuildMask))
            {
                m = Math.Max(m, FindMaxGeodes(
                    m: m,
                    t: t - 1,
                    bp: bp,
                    inv: inv.AfterBuilding(bp, material),
                    canBuildMask: Materials.All));
            }
        }

        if (IsWaitingReasonable(bp, inv))
        {
            m = Math.Max(m, FindMaxGeodes(
                m: m,
                t: t - 1,
                bp: bp,
                inv: inv.AfterWaiting(),
                canBuildMask: UpdateCanBuildMaskAfterWaiting(bp, inv, canBuildMask)));
        }

        return m;
    }

    private static int GetUpperBound(Materials target, Inventory inv, int t)
    {
        return inv.Funds[target] + t * inv.Robots[target] + t * (t - 1);
    }

    private static bool IsBuildingReasonable(Materials target, Blueprint bp, Inventory inv, int t, Materials canBuild)
    {
        if (!canBuild.HasFlag(target))
        {
            return false;
        }

        if (target != Materials.Geode && inv.Robots[target] * t + inv.Funds[target] >= bp.HighestCosts[target] * t)
        {
            return false;
        }
        
        return CanAffordToBuild(bp.RobotCosts[target], inv.Funds);
    }
    
    private static bool IsWaitingReasonable(Blueprint bp, Inventory inv)
    {
        return !bp.RobotCosts.Values.All(costs => CanAffordToBuild(costs, inv.Funds));
    }

    private static Materials UpdateCanBuildMaskAfterWaiting(Blueprint bp, Inventory inv, Materials mask)
    {
        foreach (var material in MaterialTypes)
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
}