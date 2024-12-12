using Utilities.Collections;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2024.D12;

[PuzzleInfo("Garden Groups", Topics.Vectors, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();
        var plots = CollectPlots(input);
        
        return part switch
        {
            1 => plots.Sum(plot => plot.Count * GetPerim(plot)),
            2 => plots.Sum(plot => plot.Count * GetSides(plot)),
            _ => PuzzleNotSolvedString
        };
    }

    private static IEnumerable<HashSet<Vec2D>> CollectPlots(string[] input)
    {
        var grid = Grid2D<char>.MapChars(input);
        var sets = new DisjointSet<Vec2D>(collection: grid);

        foreach (var pos in grid)
        foreach (var adj in pos.GetAdjacentSet(Metric.Taxicab))
        {
            if (grid.Contains(adj) && grid[pos] == grid[adj])
            {
                sets.Union(pos, adj);
            }
        }

        return grid
            .GroupBy(pos => sets.FindSet(pos))
            .Select(grouping => grouping.ToHashSet());
    }

    private static int GetPerim(HashSet<Vec2D> plot)
    {
        return plot.Sum(pos =>
            pos.GetAdjacentSet(Metric.Taxicab).Count(adj => !plot.Contains(adj)));
    }
    
    private static int GetSides(HashSet<Vec2D> plot)
    {
        var seeds = plot.Where(pos => !plot.Contains(pos + Vec2D.Left));
        var turns = 0;
        var check = new HashSet<Pose2D>();

        foreach (var seed in seeds)
        {
            var start = seed + Vec2D.Left;
            var pose = new Pose2D(Pos: start, Face: Vec2D.Up);
            
            while (check.Add(pose))
            {
                var ahead = plot.Contains(pose.Ahead);
                var right = plot.Contains(pose.Right);
                var diag  = plot.Contains(pose.Right + pose.Face);

                switch (ahead, right, diag)
                {
                    case (ahead: true,  right: true, diag: _):
                        pose = pose.Turn(Rot3D.P90Z);
                        turns++;
                        continue;
                    case (ahead: false, right: true, diag: true):
                        pose = pose.Step();
                        continue;
                    case (ahead: false, right: true, diag: false):
                        pose = pose.Step().Turn(Rot3D.N90Z).Step();
                        turns++;
                        continue;
                }
            }   
        }
        
        return turns;
    }
}