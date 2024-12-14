using System.Text;
using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2024.D14;

[PuzzleInfo("Restroom Redoubt", Topics.Vectors, Difficulty.Easy, favourite: true)]
public class Solution : SolutionBase
{
    private const int Width = 101;
    private const int Height = 103;
    
    public record struct Robot(Vec2D Pos, Vec2D Vel)
    {
        public static Robot Parse(string s)
        {
            var chunks = s.Split(' ');
            var pos = Vec2D.Parse(chunks[0]);
            var vel = Vec2D.Parse(chunks[1]);
            return new Robot(pos, vel);
        }

        public void Tick(Aabb2D aabb)
        {
            var naive = Pos + Vel;
            Pos = new Vec2D(naive.X.Modulo(aabb.Width), naive.Y.Modulo(aabb.Height));
        }
    }
    
    public override object Run(int part)
    {
        var robots = ParseInputLines(Robot.Parse);
        var aabb = new Aabb2D(xMin: 0, yMin: 0, xMax: Width - 1, yMax: Height - 1);
        
        return part switch
        {
            1 => GetSafetyFactor(robots, aabb),
            2 => GetMaxDensity(robots, aabb),
            _ => PuzzleNotSolvedString
        };
    }
    
    private static int GetSafetyFactor(Robot[] robots, Aabb2D aabb)
    {
        for (var t = 0; t < 100; t++)
        for (var i = 0; i < robots.Length; i++)
        {
            robots[i].Tick(aabb);
        }

        return robots.Count(r => r.Pos is { X: < Width / 2, Y: < Height / 2 }) *
               robots.Count(r => r.Pos is { X: < Width / 2, Y: > Height / 2 }) *
               robots.Count(r => r.Pos is { X: > Width / 2, Y: < Height / 2 }) *
               robots.Count(r => r.Pos is { X: > Width / 2, Y: > Height / 2 });
    }

    private int GetMaxDensity(Robot[] robots, Aabb2D aabb)
    {
        var maxElapsed = 0;
        var maxDensity = int.MaxValue;
        
        for (var t = 1; t <= 10_000; t++)
        {
            for (var i = 0; i < robots.Length; i++)
            {
                robots[i].Tick(aabb);
            }

            var center = robots.Aggregate(seed: Vec2D.Zero, (sum, r) => sum + r.Pos) / robots.Length;
            var weight = robots.Sum(r => Vec2D.Distance(r.Pos, center, Metric.Taxicab));
            
            if (weight < maxDensity)
            {
                maxDensity = weight;
                maxElapsed = t;

                Log($"Density increasing at tick t = {t}");
                Log(log: Map(robots, aabb));
            }
        }

        return maxElapsed;
    }
    
    private static string Map(IEnumerable<Robot> robots, Aabb2D aabb)
    {
        var map = robots.Select(robot => robot.Pos).ToHashSet();
        var sb = new StringBuilder();

        for (var i = 0; i < aabb.Height; i++)
        for (var j = 0; j < aabb.Width; j++)
        {
            if (j == 0) sb.Append('\n');
            sb.Append(map.Contains(new Vec2D(X: j, Y: i)) ? '#' : '.');
        }

        return sb.ToString();
    }
}