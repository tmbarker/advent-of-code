using Problems.Y2021.Common;
using System.Text.RegularExpressions;
using Utilities.Cartesian;

namespace Problems.Y2021.D17;

/// <summary>
/// Trick Shot: https://adventofcode.com/2021/day/17
/// </summary>
public class Solution : SolutionBase2021
{
    public override int Day => 17;
    
    public override object Run(int part)
    {
        var target = ParseTarget();
        return part switch
        {
            0 => ComputeMaxProjectileHeight(target),
            1 => ComputeNumTrajectories(target),
            _ => ProblemNotSolvedString,
        };
    }

    private static int ComputeMaxProjectileHeight(Aabb2D target)
    {
        var vY = Math.Abs(target.YMin) - 1;
        var height = vY * (vY + 1) / 2;
        
        return height;
    }

    private static int ComputeNumTrajectories(Aabb2D target)
    {
        var vMinX = (int)Math.Floor(Math.Sqrt(2 * target.XMin));
        var vMaxX = target.XMax;

        var vMinY = target.YMin;
        var vMaxY = Math.Abs(target.YMin) - 1;

        var count = 0;
        for (var x = vMinX; x <= vMaxX; x++)
        for (var y = vMinY; y <= vMaxY; y++)
        {
            if (CheckTrajectory(new Vector2D(x, y), target))
            {
                count++;
            }
        }

        return count;
    }

    private static bool CheckTrajectory(Vector2D v, Aabb2D target)
    {
        var pos = Vector2D.Zero;
        while (pos.Y >= target.YMin && pos.X <= target.XMax)
        {
            pos += v;
            v = StepVelocity(v);
            
            if (target.Contains(pos, true))
            {
                return true;
            }
        }
        
        return false;
    }

    private static Vector2D StepVelocity(Vector2D v)
    {
        var y = v.Y - 1;
        var x = v.X == 0 
            ? v.X 
            : v.X - 1;

        return new Vector2D(x, y);
    }
    
    private Aabb2D ParseTarget()
    {
        var matches = Regex.Matches(GetInputText(), @"-?\d+");
        
        return new Aabb2D(
            xMin: int.Parse(matches[0].Value),
            xMax: int.Parse(matches[1].Value),
            yMin: int.Parse(matches[2].Value),
            yMax: int.Parse(matches[3].Value));
    }
}