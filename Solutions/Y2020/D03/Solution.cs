using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2020.D03;

[PuzzleInfo("Toboggan Trajectory", Topics.Vectors|Topics.Math, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    private static readonly Vec2D InitialPos = Vec2D.Zero;
    private static readonly Vec2D Trajectory = new(X: 3, Y: 1);
    private static readonly IList<Vec2D> Trajectories = new List<Vec2D>
    {
        new (X: 1, Y: 1),
        new (X: 3, Y: 1),
        new (X: 5, Y: 1),
        new (X: 7, Y: 1),
        new (X: 1, Y: 2)
    };

    public override object Run(int part)
    {
        var forest = GetInputLines();
        return part switch
        {
            1 => GetTreesOnTrajectoryCount(InitialPos, Trajectory, forest),
            2 => GetTreesOnTrajectoriesProduct(InitialPos, Trajectories, forest),
            _ => PuzzleNotSolvedString
        };
    }

    private static int GetTreesOnTrajectoriesProduct(Vec2D pos, IEnumerable<Vec2D> trajectories, IList<string> forest)
    {
        return trajectories.Aggregate(1, (current, trajectory) => current * GetTreesOnTrajectoryCount(pos, trajectory, forest));
    }
    
    private static int GetTreesOnTrajectoryCount(Vec2D pos, Vec2D trajectory, IList<string> forest)
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
            
            pos = new Vec2D(
                X: (pos.X + trajectory.X).Modulo(width),
                Y: pos.Y + trajectory.Y);
        }

        return count;
    }
}