using Problems.Y2016.Common;
using Utilities.Cartesian;
using Utilities.Extensions;

namespace Problems.Y2016.D01;

/// <summary>
/// No Time for a Taxicab: https://adventofcode.com/2016/day/1
/// </summary>
public class Solution : SolutionBase2016
{
    public override int Day => 1;
    
    public override object Run(int part)
    {
        var input = GetInputText();
        var steps = input.Split(separator: ", ", StringSplitOptions.RemoveEmptyEntries);
        
        return part switch
        {
            1 => GetMinDistance(steps, haltOnRepeat: false),
            2 => GetMinDistance(steps, haltOnRepeat: true),
            _ => ProblemNotSolvedString
        };
    }

    private static int GetMinDistance(IEnumerable<string> steps, bool haltOnRepeat)
    {
        var pos = Vector2D.Zero;
        var face = Vector2D.Up;
        var visited = new HashSet<Vector2D>();

        foreach (var step in steps)
        {
            face = step[0] switch
            {
                'L' => Rotation3D.Positive90Z * face,
                'R' => Rotation3D.Negative90Z * face,
                _ => face
            };

            for (var i = 0; i < step.ParseInt(); i++)
            {
                pos += face;
                if (haltOnRepeat && !visited.Add(pos))
                {
                    return pos.Magnitude(metric: Metric.Taxicab);
                }
            }
        }

        return pos.Magnitude(metric: Metric.Taxicab);
    }
}