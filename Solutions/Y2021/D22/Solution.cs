using Utilities.Collections;
using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2021.D22;

[PuzzleInfo("Reactor Reboot", Topics.Math, Difficulty.Hard)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var instructions = ParseInputLines(parseFunc: ParseInstruction);
        return part switch
        {
            1 => Init(instructions),
            2 => Reboot(instructions),
            _ => PuzzleNotSolvedString
        };
    }

    private static int Init(IEnumerable<(bool On, Aabb3D Aabb)> instructions)
    {
        var onSet = new HashSet<Vec3D>();
        var initRegion = new Aabb3D(
            v1: Vec3D.Zero - 50 * Vec3D.One,
            v2: Vec3D.Zero + 50 * Vec3D.One);

        foreach (var step in instructions)
        {
            if (!Aabb3D.Overlap(initRegion, step.Aabb, out var overlap))
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
        var signedAabbs = new DefaultDict<Aabb3D, int>(defaultValue: 0);
        foreach (var (on, aabb) in instructions)
        {
            foreach (var (signedAabb, weight) in signedAabbs.Freeze())
            {
                if (Aabb3D.Overlap(aabb, signedAabb, out var overlap))
                {
                    signedAabbs[overlap] -= weight;
                }
            }

            if (on)
            {
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