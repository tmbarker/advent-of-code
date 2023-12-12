using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2018.D23;

[PuzzleInfo("Experimental Emergency Teleportation", Topics.Vectors, Difficulty.Hard, favourite: true)]
public sealed class Solution : SolutionBase
{
    private readonly record struct Nanobot(Vector3D Pos, int Range);
    
    public override object Run(int part)
    {
        var nanobots = ParseInputLines(parseFunc: ParseNanobot).ToList();
        return part switch
        {
            1 => CountInRangeOfStrongest(nanobots),
            2 => MaximizeBotsInRange(nanobots),
            _ => ProblemNotSolvedString
        };
    }

    private static int CountInRangeOfStrongest(IList<Nanobot> bots)
    {
        var strongest = bots.MaxBy(bot => bot.Range);
        return bots.Count(bot => Vector3D.Distance(
            a: strongest.Pos,
            b: bot.Pos,
            metric: Metric.Taxicab) <= strongest.Range);
    }

    private static int MaximizeBotsInRange(IList<Nanobot> bots)
    {
        var extrema = bots.SelectMany(bot =>
        {
            return new[]
            {
                new Vector3D(x: bot.Pos.X - bot.Range, y: bot.Pos.Y - bot.Range, z: bot.Pos.Z - bot.Range),
                new Vector3D(x: bot.Pos.X + bot.Range, y: bot.Pos.Y + bot.Range, z: bot.Pos.Z + bot.Range)
            };
        });
        
        var area = new Aabb3D(extents: extrema.ToArray(), inclusive: true);
        var max = Search(bots, area);

        return max.Magnitude(Metric.Taxicab);
    }

    private static Vector3D Search(IList<Nanobot> bots, Aabb3D area)
    {
        var heap = new PriorityQueue<Aabb3D, SearchRanking>(new[] { (area, Rank(area, bots)) });
        while (heap.Count > 0)
        {
            var region = heap.Dequeue();
            if (region.Volume == 1L)
            {
                return region.Single();
            }

            foreach (var child in Octree.Subdivide(region))
            {
                heap.Enqueue(child, priority: Rank(child, bots));
            }
        }

        throw new NoSolutionException();
    }

    private static SearchRanking Rank(Aabb3D region, IEnumerable<Nanobot> bots)
    {
        var inRange = 0;
        foreach (var (pos, range) in bots)
        {
            if (region.Contains(pos, inclusive: true))
            {
                inRange++;
                continue;
            }

            var dist = 0;
            dist += pos.X < region.XMin ? region.XMin - pos.X : pos.X - region.XMax;
            dist += pos.Y < region.YMin ? region.YMin - pos.Y : pos.Y - region.YMax;
            dist += pos.Z < region.ZMin ? region.ZMin - pos.Z : pos.Z - region.ZMax;

            if (dist <= range)
            {
                inRange++;
            }
        }
        
        var volume = region.Volume;
        var distance = region.Center.Magnitude(Metric.Taxicab);

        return new SearchRanking(inRange, distance, volume);
    }

    private static Nanobot ParseNanobot(string line)
    {
        var numbers = line.ParseInts();
        var pos = new Vector3D(
            x: numbers[0],
            y: numbers[1],
            z: numbers[2]);

        return new Nanobot(pos, numbers[3]);
    }
}