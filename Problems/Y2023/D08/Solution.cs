using System.Text.RegularExpressions;
using Problems.Common;
using Utilities.Extensions;
using Utilities.Numerics;

namespace Problems.Y2023.D08;

/// <summary>
/// Haunted Wasteland: https://adventofcode.com/2023/day/8
/// </summary>
public sealed class Solution : SolutionBase
{
    private readonly record struct Node(string Id, string Left, string Right);
    
    public override object Run(int part)
    {
        var lines = GetInputLines();
        var dirs = lines[0];
        var nodes = lines[2..]
            .Select(ParseNode)
            .ToDictionary(n => n.Id);
        
        return part switch
        {
            1 => NavigateSingle(dirs, nodes),
            2 => NavigateMany(dirs, nodes),
            _ => ProblemNotSolvedString
        };
    }

    private static long NavigateSingle(string dirs, IDictionary<string, Node> nodes)
    {
        return Navigate(dirs, nodes, start: "AAA", stop: id => id == "ZZZ");
    }
    
    private static long NavigateMany(string dirs, IDictionary<string, Node> nodes)
    {
        var stop = (string id) => id.EndsWith('Z');
        var cycles = nodes.Keys
            .Where(id => id.EndsWith('A'))
            .Select(id => Navigate(dirs, nodes, start: id, stop))
            .ToArray();
        
        return Numerics.Lcm(cycles);
    }

    private static long Navigate(string dirs, IDictionary<string, Node> nodes, string start, Func<string, bool> stop)
    {
        var i = 0;
        var p = nodes[start];
        
        while (!stop.Invoke(p.Id))
        {
            p = dirs[i++ % dirs.Length] switch
            {
                'L' => nodes[p.Left],
                'R' => nodes[p.Right],
                _ => throw new NoSolutionException()
            };
        }

        return i;
    }
    
    private static Node ParseNode(string line)
    {
        var values = Regex.Matches(line, pattern: "[A-Z1-9]{3}").SelectValues();
        return new Node(
            Id:    values[0],
            Left:  values[1],
            Right: values[2]);
    }
}