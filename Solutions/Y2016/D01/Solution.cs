using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2016.D01;

[PuzzleInfo("No Time for a Taxicab", Topics.Vectors, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
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
        var pose = new Pose2D(pos: Vector2D.Zero, face: Vector2D.Up);
        var visited = new HashSet<Vector2D>();

        foreach (var step in steps)
        {
            pose = step[0] switch
            {
                'L' => pose.Turn(Rotation3D.Positive90Z),
                'R' => pose.Turn(Rotation3D.Negative90Z),
                _ => pose
            };

            for (var i = 0; i < step.ParseInt(); i++)
            {
                pose = pose.Step();
                if (haltOnRepeat && !visited.Add(pose.Pos))
                {
                    return pose.Pos.Magnitude(metric: Metric.Taxicab);
                }
            }
        }

        return pose.Pos.Magnitude(metric: Metric.Taxicab);
    }
}