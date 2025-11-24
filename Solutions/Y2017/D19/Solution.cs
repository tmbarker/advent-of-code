using System.Text;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2017.D19;

[PuzzleInfo("A Series of Tubes", Topics.Graphs, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private readonly record struct PathSummary(string Letters, int Steps);
    
    private const char Junction = '+';
    private static readonly HashSet<char> Traversable = ['|', '-', '+'];

    public override object Run(int part)
    {
        var input = GetInputLines();
        var map = GetInputGrid();
        var start = new Vec2D(
            X: input[0].IndexOf('|'),
            Y: map.Height - 1);

        return part switch
        {
            1 => Traverse(map, start).Letters,
            2 => Traverse(map, start).Steps,
            _ => PuzzleNotSolvedString
        };
    }

    private static PathSummary Traverse(Grid2D<char> map, Vec2D start)
    {
        var letters = new StringBuilder();
        var steps = 1;
        var pose = new Pose2D(Pos: start, Face: Vec2D.Down);

        while (CanMoveTo(map, pos: pose.Ahead))
        {
            pose = pose.Step();
            steps++;
            
            if (char.IsLetter(map[pose.Pos]))
            {
                letters.Append(map[pose.Pos]);
            }

            if (map[pose.Pos] == Junction)
            {
                pose = CanMoveTo(map, pose.Pos + Rot3D.P90Z.Transform(pose.Face))
                    ? pose.Turn(Rot3D.P90Z)
                    : pose.Turn(Rot3D.N90Z);
            }
        }

        return new PathSummary(
            Letters: letters.ToString(),
            Steps: steps);
    }

    private static bool CanMoveTo(Grid2D<char> map, Vec2D pos)
    {
        return map.Contains(pos) && (Traversable.Contains(map[pos]) || char.IsLetter(map[pos]));
    }
}