using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2021.D17;

[PuzzleInfo("Trick Shot", Topics.Math, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var target = ParseTarget();
        return part switch
        {
            1 => ComputeMaxProjectileHeight(target),
            2 => ComputeNumTrajectories(target),
            _ => PuzzleNotSolvedString
        };
    }

    private static int ComputeMaxProjectileHeight(Aabb2D target)
    {
        var vY = Math.Abs(target.Min.Y) - 1;
        var height = vY * (vY + 1) / 2;
        
        return height;
    }

    private static int ComputeNumTrajectories(Aabb2D target)
    {
        var vMinX = (int)Math.Floor(Math.Sqrt(2 * target.Min.X));
        var vMaxX = target.Max.X;

        var vMinY = target.Min.Y;
        var vMaxY = Math.Abs(target.Min.Y) - 1;

        var count = 0;
        for (var x = vMinX; x <= vMaxX; x++)
        for (var y = vMinY; y <= vMaxY; y++)
        {
            if (CheckTrajectory(new Vec2D(x, y), target))
            {
                count++;
            }
        }

        return count;
    }

    private static bool CheckTrajectory(Vec2D vel, Aabb2D target)
    {
        var pos = Vec2D.Zero;
        while (pos.Y >= target.Min.Y && pos.X <= target.Max.X)
        {
            pos += vel;
            vel = StepVelocity(vel);
            
            if (target.Contains(pos, true))
            {
                return true;
            }
        }
        
        return false;
    }

    private static Vec2D StepVelocity(Vec2D v)
    {
        var y = v.Y - 1;
        var x = v.X == 0 
            ? v.X 
            : v.X - 1;

        return new Vec2D(x, y);
    }
    
    private Aabb2D ParseTarget()
    {
        var input = GetInputText();
        var numbers = input.ParseInts();
        
        return new Aabb2D(
            xMin: numbers[0],
            xMax: numbers[1],
            yMin: numbers[2],
            yMax: numbers[3]);
    }
}