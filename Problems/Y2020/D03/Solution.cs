using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Problems.Y2020.D03;

[PuzzleInfo("Toboggan Trajectory", Topics.Vectors|Topics.Math, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    private static readonly Vector2D InitialPos = Vector2D.Zero;
    private static readonly Vector2D Trajectory = new(x: 3, y: 1);
    private static readonly IList<Vector2D> Trajectories = new List<Vector2D>
    {
        new (x: 1, y: 1),
        new (x: 3, y: 1),
        new (x: 5, y: 1),
        new (x: 7, y: 1),
        new (x: 1, y: 2)
    };

    public override object Run(int part)
    {
        var forest = GetInputLines();
        return part switch
        {
            1 => GetTreesOnTrajectoryCount(InitialPos, Trajectory, forest),
            2 => GetTreesOnTrajectoriesProduct(InitialPos, Trajectories, forest),
            _ => ProblemNotSolvedString
        };
    }

    private static int GetTreesOnTrajectoriesProduct(Vector2D pos, IEnumerable<Vector2D> trajectories, IList<string> forest)
    {
        return trajectories.Aggregate(1, (current, trajectory) => current * GetTreesOnTrajectoryCount(pos, trajectory, forest));
    }
    
    private static int GetTreesOnTrajectoryCount(Vector2D pos, Vector2D trajectory, IList<string> forest)
    {
        var length = forest.Count;
        var width = forest[0].Length;
        var count = 0;

        while (pos.Y < length)
        {
            if (forest[pos.Y][pos.X] == '#')
            {
                count++;
            }
            
            pos = new Vector2D(
                x: (pos.X + trajectory.X).Modulo(width),
                y: pos.Y + trajectory.Y);
        }

        return count;
    }
}