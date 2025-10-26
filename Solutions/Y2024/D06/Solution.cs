using Utilities.Geometry.Euclidean;

namespace Solutions.Y2024.D06;

[PuzzleInfo("Guard Gallivant", Topics.Vectors, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    private const char Start = '^';
    private const char Obstacle = '#';
    
    public override object Run(int part)
    {
        var lines = GetInputLines();
        var grid = Grid2D<char>.MapChars(lines);
        var start = grid.Find(Start);
        var path = Navigate(grid, start);
        
        return part switch
        {
            1 => path.Count,
            2 => CountLoops(grid, start, path),
            _ => PuzzleNotSolvedString
        };
    }

    private static HashSet<Vec2D> Navigate(Grid2D<char> grid, Vec2D start)
    {
        var pose = new Pose2D(Pos: start, Face: Vec2D.Up);
        var visited = new HashSet<Vec2D>();
        
        while (grid.Contains(pose.Pos))
        {
            visited.Add(pose.Pos);
            pose = grid.Contains(pose.Ahead) && grid[pose.Ahead] == Obstacle
                ? pose.Turn(Rot3D.N90Z)
                : pose.Step();
        }
        
        return visited;
    }
    
    private static int CountLoops(Grid2D<char> grid, Vec2D start, HashSet<Vec2D> path)
    {
        return path.Except([start])
            .AsParallel()
            .Count(candidate => CheckLoop(grid, start, obstacle: candidate));
    }
    
    private static bool CheckLoop(Grid2D<char> grid, Vec2D start, Vec2D obstacle)
    {
        var pose = new Pose2D(Pos: start, Face: Vec2D.Up);
        var visited = new HashSet<Pose2D>();
        
        while (visited.Add(pose) && grid.Contains(pose.Ahead))
        {
            pose = pose.Ahead == obstacle || grid[pose.Ahead] == Obstacle
                ? pose.Turn(Rot3D.N90Z)
                : pose.Step();
        }

        return grid.Contains(pose.Ahead);
    }
}