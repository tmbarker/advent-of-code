using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2019.D10;

[PuzzleInfo("Monitoring Station", Topics.Vectors, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private const char Asteroid = '#';
    private const int TargetCount = 200;

    public override object Run(int part)
    {
        var input = GetInputLines();
        var asteroids = ParseAsteroids(input, out var transform);
        var visible = FindMaxDetectable(asteroids, out var pos);
        
        return part switch
        {
            1 => visible,
            2 => FindNthDestroyed(asteroids, pos, transform),
            _ => ProblemNotSolvedString
        };
    }

    private static int FindMaxDetectable(HashSet<Vec2D> asteroids, out Vec2D pos)
    {
        pos = Vec2D.Zero;
        
        var max = 0;
        var memo = new Dictionary<Vec2D, Vec2D>();
        var linesOfSight = new HashSet<Vec2D>();
        
        foreach (var candidate in asteroids)
        {
            linesOfSight.Clear();
            foreach (var other in asteroids.Where(asteroid => asteroid != candidate))
            {
                var view = other - candidate;
                if (!memo.ContainsKey(view))
                {
                    memo[view] = Vec2D.MinCollinear(view);
                }

                linesOfSight.Add(memo[view]);
            }

            if (linesOfSight.Count <= max)
            {
                continue;
            }
            
            max = linesOfSight.Count;
            pos = candidate;
        }
        
        return max;
    }

    private static int FindNthDestroyed(IEnumerable<Vec2D> asteroids, Vec2D laser, Func<Vec2D, Vec2D> transform)
    {
        var destroyed = new List<Vec2D>();
        var others = asteroids.Except(laser).ToHashSet();

        var collinearSetsMap = others
            .GroupBy(asteroid => Vec2D.MinCollinear(asteroid - laser))
            .ToDictionary(g => g.Key, g => BuildDistanceQueue(laser, g));
        var laserSteps = new Queue<Vec2D>(collinearSetsMap.Keys
            .OrderBy(v => Vec2D.AngleBetweenDeg(Vec2D.Up, v).Modulo(360)));

        while (destroyed.Count < TargetCount)
        {
            var los = laserSteps.Dequeue();
            destroyed.Add(collinearSetsMap[los].Dequeue());
            
            if (collinearSetsMap[los].Count == 0)
            {
                collinearSetsMap.Remove(los);
            }

            laserSteps.Enqueue(los);
        }

        var target = destroyed.Last();
        var transformed = transform(target);
        return 100 * transformed.X + transformed.Y;
    }

    private static Queue<Vec2D> BuildDistanceQueue(Vec2D from, IEnumerable<Vec2D> to)
    {
        return new Queue<Vec2D>(to.OrderBy(v => Vec2D.Distance(v, from, Metric.Taxicab)));
    }
    
    private static HashSet<Vec2D> ParseAsteroids(IList<string> input, out Func<Vec2D, Vec2D> transform)
    {
        var rows = input.Count;
        var cols = input[0].Length;
        var asteroids = new HashSet<Vec2D>();

        for (var y = 0; y < rows; y++)
        for (var x = 0; x < cols; x++)
        {
            if (input[rows - y - 1][x] == Asteroid)
            {
                asteroids.Add(new Vec2D(x, y));
            }
        }

        transform = v => new Vec2D(v.X, rows - 1 - v.Y);
        return asteroids;
    }
}