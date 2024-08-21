using System.Text;
using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2018.D10;

[PuzzleInfo("The Stars Align", Topics.Vectors, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => WaitForMessage().Message,
            2 => WaitForMessage().Time,
            _ => PuzzleNotSolvedString
        };
    }

    private (string Message, int Time) WaitForMessage()
    {
        var (pos, vel) = GetInitialPoses();
        var aabb = new Aabb2D(pos);
        var area = aabb.Area;
        var prevArea = long.MaxValue;
        var time = 0;

        while (area < prevArea)
        {
            prevArea = area;
            time++;
            StepForwards(pos, vel);
            
            aabb = new Aabb2D(pos);
            area = aabb.Area;
        }

        time--;
        StepBackwards(pos, vel);
        
        var alignedPos = pos.Normalize().ToHashSet();
        var alignedAabb = new Aabb2D(alignedPos);
        var message = new StringBuilder();
        
        for (var y = 0; y < alignedAabb.Height; y++)
        {
            message.Append('\n');
            for (var x = 0; x < alignedAabb.Width; x++)
            {
                message.Append(alignedPos.Contains(new Vec2D(x, y)) ? '#' : '.');
            }
        }

        return (message.ToString(), time);
    }

    private static void StepForwards(IList<Vec2D> pos, IList<Vec2D> vel)
    {
        for (var i = 0; i < pos.Count; i++)
        {
            pos[i] += vel[i];
        }
    }
    
    private static void StepBackwards(IList<Vec2D> pos, IList<Vec2D> vel)
    {
        for (var i = 0; i < pos.Count; i++)
        {
            pos[i] -= vel[i];
        }
    }

    private (Vec2D[] Pos, Vec2D[] Vel) GetInitialPoses()
    {
        var input = GetInputLines();
        var count = input.Length;
        
        var pos = new Vec2D[count];
        var vel = new Vec2D[count];

        for (var i = 0; i < count; i++)
        {
            var numbers = input[i].ParseInts();
            pos[i] = new Vec2D(
                x: numbers[0],
                y: numbers[1]);
            vel[i] = new Vec2D(
                x: numbers[2],
                y: numbers[3]);
        }

        return (pos, vel);
    }
}