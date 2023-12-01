using Problems.Common;
using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Problems.Y2019.D10;

/// <summary>
/// Monitoring Station: https://adventofcode.com/2019/day/10
/// </summary>
public class Solution : SolutionBase
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

    private static int FindMaxDetectable(HashSet<Vector2D> asteroids, out Vector2D pos)
    {
        pos = Vector2D.Zero;
        
        var max = 0;
        var memo = new Dictionary<Vector2D, Vector2D>();
        var linesOfSight = new HashSet<Vector2D>();
        
        foreach (var candidate in asteroids)
        {
            linesOfSight.Clear();
            foreach (var other in asteroids.Where(asteroid => asteroid != candidate))
            {
                var view = other - candidate;
                if (!memo.ContainsKey(view))
                {
                    memo[view] = Vector2D.MinCollinear(view);
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

    private static int FindNthDestroyed(IEnumerable<Vector2D> asteroids, Vector2D laser, Func<Vector2D, Vector2D> transform)
    {
        var destroyed = new List<Vector2D>();
        var others = asteroids.Except(laser).ToHashSet();

        var collinearSetsMap = others
            .GroupBy(asteroid => Vector2D.MinCollinear(asteroid - laser))
            .ToDictionary(g => g.Key, g => BuildDistanceQueue(laser, g));
        var laserSteps = new Queue<Vector2D>(collinearSetsMap.Keys
            .OrderBy(v => Vector2D.AngleBetweenDeg(Vector2D.Up, v).Modulo(360)));

        while (destroyed.Count < TargetCount)
        {
            var los = laserSteps.Dequeue();
            destroyed.Add(collinearSetsMap[los].Dequeue());
            
            if (!collinearSetsMap[los].Any())
            {
                collinearSetsMap.Remove(los);
            }

            laserSteps.Enqueue(los);
        }

        var target = destroyed.Last();
        var transformed = transform(target);
        return 100 * transformed.X + transformed.Y;
    }

    private static Queue<Vector2D> BuildDistanceQueue(Vector2D from, IEnumerable<Vector2D> to)
    {
        return new Queue<Vector2D>(to.OrderBy(v => Vector2D.Distance(v, from, Metric.Taxicab)));
    }
    
    private static HashSet<Vector2D> ParseAsteroids(IList<string> input, out Func<Vector2D, Vector2D> transform)
    {
        var rows = input.Count;
        var cols = input[0].Length;
        var asteroids = new HashSet<Vector2D>();

        for (var y = 0; y < rows; y++)
        for (var x = 0; x < cols; x++)
        {
            if (input[rows - y - 1][x] == Asteroid)
            {
                asteroids.Add(new Vector2D(x, y));
            }
        }

        transform = v => new Vector2D(v.X, rows - 1 - v.Y);
        return asteroids;
    }
}