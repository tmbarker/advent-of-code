using Problems.Y2020.Common;
using Utilities.Cartesian;
using Utilities.Extensions;

namespace Problems.Y2020.D03;

/// <summary>
/// Toboggan Trajectory: https://adventofcode.com/2020/day/3
/// </summary>
public class Solution : SolutionBase2020
{
    private const char Tree = '#';

    private static readonly Vector2D InitialPos = Vector2D.Zero;
    private static readonly Vector2D Trajectory = new(3, 1);
    private static readonly IList<Vector2D> Trajectories = new List<Vector2D>()
    {
        new (1, 1),
        new (3, 1),
        new (5, 1),
        new (7, 1),
        new (1, 2),
    };

    public override int Day => 3;

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
            if (forest[pos.Y][pos.X] == Tree)
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