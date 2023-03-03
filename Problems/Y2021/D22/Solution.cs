using Problems.Y2021.Common;
using System.Text.RegularExpressions;
using Utilities.Cartesian;
using Utilities.Extensions;

namespace Problems.Y2021.D22;

/// <summary>
/// Reactor Reboot: https://adventofcode.com/2021/day/22
/// </summary>
public class Solution : SolutionBase2021
{
    private const int InitRegionHalfWidth = 50;
    
    public override int Day => 22;
    
    public override object Run(int part)
    {
        var instructions = ParseInput(GetInputLines());
        return part switch
        {
            1 => Init(instructions),
            2 => Reboot(instructions),
            _ => ProblemNotSolvedString
        };
    }

    private static int Init(IEnumerable<(bool On, Aabb3D Aabb)> instructions)
    {
        var initRegion = Aabb3D.CubeCenteredAtOrigin(InitRegionHalfWidth);
        var onSet = new HashSet<Vector3D>();

        foreach (var step in instructions)
        {
            if (!Aabb3D.FindOverlap(initRegion, step.Aabb, out var overlap))
            {
                continue;
            }
            
            for (var x = overlap.XMin; x <= overlap.XMax; x++)
            for (var y = overlap.YMin; y <= overlap.YMax; y++)
            for (var z = overlap.ZMin; z <= overlap.ZMax; z++)
            {
                if (step.On)
                {
                    onSet.Add(new Vector3D(x, y, z));
                }
                else
                {
                    onSet.Remove(new Vector3D(x, y, z));
                }
            }
        }

        return onSet.Count;
    }

    private static long Reboot(IEnumerable<(bool On, Aabb3D Aabb)> instructions)
    {
        var signedAabbs = new Dictionary<Aabb3D, int>();
        foreach (var (on, aabb) in instructions)
        {
            foreach (var (signedAabb, weight) in signedAabbs.Freeze())
            {
                if (Aabb3D.FindOverlap(aabb, signedAabb, out var overlap))
                {
                    signedAabbs.EnsureContainsKey(overlap);
                    signedAabbs[overlap] -= weight;
                }
            }

            if (on)
            {
                signedAabbs.EnsureContainsKey(aabb);
                signedAabbs[aabb]++;
            }
        }

        var cubes = 0L;
        foreach (var (aabb, weight) in signedAabbs)
        {
            cubes += weight * aabb.GetVolume();
        }   
        return cubes;
    }
    
    private static IEnumerable<(bool on, Aabb3D aabb)> ParseInput(IEnumerable<string> input)
    {
        return input.Select(ParseLine);
    }

    private static (bool on, Aabb3D aabb) ParseLine(string line)
    {
        var matches = Regex.Matches(line, @"-?\d+");
        var on = line.StartsWith("on");
        var cuboid = new Aabb3D(
            xMin: int.Parse(matches[0].Value),
            xMax: int.Parse(matches[1].Value),
            yMin: int.Parse(matches[2].Value),
            yMax: int.Parse(matches[3].Value),
            zMin: int.Parse(matches[4].Value),
            zMax: int.Parse(matches[5].Value));

        return (on, cuboid);
    }
}