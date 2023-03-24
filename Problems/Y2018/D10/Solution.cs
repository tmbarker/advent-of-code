using System.Text;
using Problems.Attributes;
using Problems.Y2018.Common;
using Utilities.Cartesian;
using Utilities.Extensions;

namespace Problems.Y2018.D10;

/// <summary>
/// The Stars Align: https://adventofcode.com/2018/day/10
/// </summary>
[Favourite("The Stars Align", Topics.Vectors, Difficulty.Easy)]
public class Solution : SolutionBase2018
{
    public override int Day => 10;
    
    public override object Run(int part)
    {
        return part switch
        {
            1 => WaitForMessage().Message,
            2 => WaitForMessage().Time,
            _ => ProblemNotSolvedString
        };
    }

    private (string Message, int Time) WaitForMessage()
    {
        var (pos, vel) = GetInitialPoses();
        var aabb = new Aabb2D(pos, inclusive: true);
        var area = aabb.GetArea();
        var prevArea = long.MaxValue;
        var time = 0;

        while (area < prevArea)
        {
            prevArea = area;
            time++;
            StepForwards(pos, vel);
            
            aabb = new Aabb2D(pos, inclusive: true);
            area = aabb.GetArea();
        }

        time--;
        StepBackwards(pos, vel);
        
        var alignedPos = pos.Normalize().ToHashSet();
        var alignedAabb = new Aabb2D(alignedPos, inclusive: true);
        var message = new StringBuilder();
        
        for (var y = 0; y < alignedAabb.Height; y++)
        {
            message.Append('\n');
            for (var x = 0; x < alignedAabb.Width; x++)
            {
                message.Append(alignedPos.Contains(new Vector2D(x, y)) ? '#' : '.');
            }
        }

        return (message.ToString(), time);
    }

    private static void StepForwards(IList<Vector2D> pos, IList<Vector2D> vel)
    {
        for (var i = 0; i < pos.Count; i++)
        {
            pos[i] += vel[i];
        }
    }
    
    private static void StepBackwards(IList<Vector2D> pos, IList<Vector2D> vel)
    {
        for (var i = 0; i < pos.Count; i++)
        {
            pos[i] -= vel[i];
        }
    }

    private (Vector2D[] Pos, Vector2D[] Vel) GetInitialPoses()
    {
        var input = GetInputLines();
        var count = input.Length;
        
        var pos = new Vector2D[count];
        var vel = new Vector2D[count];

        for (var i = 0; i < count; i++)
        {
            var numbers = input[i].ParseInts();
            pos[i] = new Vector2D(
                x: numbers[0],
                y: numbers[1]);
            vel[i] = new Vector2D(
                x: numbers[2],
                y: numbers[3]);
        }

        return (pos, vel);
    }
}