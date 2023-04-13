using Problems.Attributes;
using Problems.Y2017.Common;
using Utilities.Extensions;

namespace Problems.Y2017.D24;

/// <summary>
/// Electromagnetic Moat: https://adventofcode.com/2017/day/24
/// </summary>
[Favourite("Electromagnetic Moat", Topics.Graphs, Difficulty.Medium)]
public class Solution : SolutionBase2017
{
    public override int Day => 24;
    
    public override object Run(int part)
    {
        return part switch
        {
            1 => GetMaxStrength(bridgeComparer: new StrengthComparer()),
            2 => GetMaxStrength(bridgeComparer: new LengthComparer()),
            _ => ProblemNotSolvedString
        };
    }

    private int GetMaxStrength(IComparer<Bridge> bridgeComparer)
    {
        var input = GetInputLines();
        var helper = BuildAdapterHelper(input);
        var bridges = GetBridges(
            head: 0,
            used: new HashSet<string>(),
            helper: helper);

        return bridges.MaxBy(bridge => bridge, bridgeComparer).Strength;
    }
    
    private static IEnumerable<Bridge> GetBridges(int head, HashSet<string> used, AdapterHelper helper)
    {
        var bridges = new List<Bridge>();
        var compatibilities = helper.GetCompatibilities(head, used);
        
        if (compatibilities.Count == 0)
        {
            var bridge = new Bridge(
                Strength: helper.GetStrength(used),
                Length: used.Count);

            bridges.Add(bridge);
            return bridges;
        }
        
        foreach (var compatibility in compatibilities)
        {
            //  It is always optimal to append a symmetric adapter if one is available
            //
            if (compatibility.ResultingPort == head)
            {
                return GetBridges(
                    head: head,
                    used: UnionAfter(used, compatibility.ViaAdapter),
                    helper: helper);
            }
            
            bridges.AddRange(GetBridges(
                head: compatibility.ResultingPort,
                used: UnionAfter(used, compatibility.ViaAdapter),
                helper: helper));
        }

        return bridges;
    }

    private static HashSet<string> UnionAfter(IEnumerable<string> used, string after)
    {
        return new HashSet<string>(used) { after };
    }

    private static AdapterHelper BuildAdapterHelper(IEnumerable<string> input)
    {
        var helper = new AdapterHelper();
        var adapters = input.Select(line =>
        {
            var numbers = line.ParseInts();
            var adapter = new Adapter(
                Key: line,
                Port1: numbers[0],
                Port2: numbers[1]);

            return adapter;
        });

        foreach (var adapter in adapters)
        {
            helper.RegisterAdapter(adapter);
        }

        return helper;
    }
}