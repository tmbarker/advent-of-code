using Utilities.Geometry.Euclidean;

namespace Solutions.Y2023.D16;

[PuzzleInfo("The Floor Will Be Lava", Topics.Vectors, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var lines = GetInputLines();
        var grid = Grid2D<char>.MapChars(lines);
        
        return part switch
        {
            1 => GetDefaultEnergized(grid),
            2 => GetMaxEnergized(grid),
            _ => PuzzleNotSolvedString
        };
    }

    private static int GetDefaultEnergized(Grid2D<char> grid)
    {
        return CountEnergized(grid, x: -1, y: grid.Height - 1, face: Vec2D.Right);
    }
    
    private static int GetMaxEnergized(Grid2D<char> grid)
    {
        var max = int.MinValue;
        for (var x = 0; x < grid.Width; x++)
        {
            max = Math.Max(max, CountEnergized(grid, x, y: grid.Height, face: Vec2D.Down));
            max = Math.Max(max, CountEnergized(grid, x, y: -1,          face: Vec2D.Up));
        }
        for (var y = 0; y < grid.Height; y++)
        {
            max = Math.Max(max, CountEnergized(grid, x: -1,         y, face: Vec2D.Right));
            max = Math.Max(max, CountEnergized(grid, x: grid.Width, y, face: Vec2D.Left));
        }
        return max;
    }
    
    private static int CountEnergized(Grid2D<char> grid, int x, int y, Vec2D face)
    {
        var start = new Pose2D(Pos: new Vec2D(x, y), Face: face);
        var queue = new Queue<Pose2D>(collection: [start]);
        var visited = new HashSet<Pose2D>();

        while (queue.Count > 0)
        {
            var pose = queue.Dequeue();
            var ahead = pose.Ahead;

            if (!grid.Contains(ahead))
            {
                continue;
            }

            var entity = grid[ahead];
            var yields = new List<Pose2D>(capacity: 2);
            
            switch (entity, pose.Face.X, pose.Face.Y)
            {
                case (entity: '\\', X:  0, Y:  1):
                case (entity: '/',  X:  0, Y: -1):
                    yields.Add(item: new Pose2D(Pos: ahead, Face: Vec2D.Left));
                    break;
                case (entity: '\\', X:  0, Y: -1):
                case (entity: '/',  X:  0, Y:  1):
                    yields.Add(item: new Pose2D(Pos: ahead, Face: Vec2D.Right));
                    break;
                case (entity: '\\', X: -1, Y:  0):
                case (entity: '/',  X:  1, Y:  0):
                    yields.Add(item: new Pose2D(Pos: ahead, Face: Vec2D.Up));
                    break;
                case (entity: '\\', X:  1, Y:  0):
                case (entity: '/',  X: -1, Y:  0):
                    yields.Add(item: new Pose2D(Pos: ahead, Face: Vec2D.Down));
                    break;
                case (entity: '-',  X: 0, Y: _):
                    yields.Add(item: new Pose2D(Pos: ahead, Face: Vec2D.Left));
                    yields.Add(item: new Pose2D(Pos: ahead, Face: Vec2D.Right));
                    break;
                case (entity: '|',  X: _, Y: 0):
                    yields.Add(item: new Pose2D(Pos: ahead, Face: Vec2D.Up));
                    yields.Add(item: new Pose2D(Pos: ahead, Face: Vec2D.Down));
                    break;
                case (entity: _,  X: _, Y: _):
                    yields.Add(item: pose.Step());
                    break;
            }

            foreach (var yield in yields.Where(yield => visited.Add(yield)))
            {
                queue.Enqueue(yield);
            }
        }
        
        return visited.DistinctBy(pose => pose.Pos).Count();
    }
}