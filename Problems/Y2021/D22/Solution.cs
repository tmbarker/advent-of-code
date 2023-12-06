using Problems.Common;
using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Problems.Y2021.D22;

/// <summary>
/// Reactor Reboot: https://adventofcode.com/2021/day/22
/// </summary>
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var instructions = ParseInputLines(parseFunc: ParseInstruction);
        return part switch
        {
            1 => Init(instructions),
            2 => Reboot(instructions),
            _ => ProblemNotSolvedString
        };
    }

    private static int Init(IEnumerable<(bool On, Aabb3D Aabb)> instructions)
    {
        var initRegion = Aabb3D.CubeCenteredAt(center: Vector3D.Zero, extent: 50);
        var onSet = new HashSet<Vector3D>();

        foreach (var step in instructions)
        {
            if (!Aabb3D.FindOverlap(initRegion, step.Aabb, out var overlap))
            {
                continue;
            }

            foreach (var pos in overlap)
            {
                if (step.On)
                {
                    onSet.Add(pos);
                }
                else
                {
                    onSet.Remove(pos);
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
            cubes += weight * aabb.Volume;
        }   
        return cubes;
    }

    private static (bool on, Aabb3D aabb) ParseInstruction(string line)
    {
        var numbers = line.ParseInts();
        var on = line.StartsWith("on");
        var cuboid = new Aabb3D(
            xMin: numbers[0],
            xMax: numbers[1],
            yMin: numbers[2],
            yMax: numbers[3],
            zMin: numbers[4],
            zMax: numbers[5]);

        return (on, cuboid);
    }
}