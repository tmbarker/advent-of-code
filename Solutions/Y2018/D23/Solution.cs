using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2018.D23;

[PuzzleInfo("Experimental Emergency Teleportation", Topics.Vectors, Difficulty.Hard, favourite: true)]
public sealed class Solution : SolutionBase
{
    private readonly record struct Nanobot(Vec3D Pos, int Range);
    
    public override object Run(int part)
    {
        var nanobots = ParseInputLines(parseFunc: ParseNanobot);
        return part switch
        {
            1 => CountInRangeOfStrongest(nanobots),
            2 => MaximizeBotsInRange(nanobots),
            _ => PuzzleNotSolvedString
        };
    }

    private static int CountInRangeOfStrongest(IList<Nanobot> bots)
    {
        var strongest = bots.MaxBy(bot => bot.Range);
        return bots.Count(bot => Vec3D.Distance(
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
                new Vec3D(X: bot.Pos.X - bot.Range, Y: bot.Pos.Y - bot.Range, Z: bot.Pos.Z - bot.Range),
                new Vec3D(X: bot.Pos.X + bot.Range, Y: bot.Pos.Y + bot.Range, Z: bot.Pos.Z + bot.Range)
            };
        });
        
        var area = new Aabb3D(extents: extrema.ToArray(), inclusive: true);
        var max = Search(bots, area);

        return max.Magnitude(Metric.Taxicab);
    }

    private static Vec3D Search(IList<Nanobot> bots, Aabb3D area)
    {
        var heap = new PriorityQueue<Aabb3D, SearchRanking>(items: [(area, Rank(area, bots))]);
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
            dist += pos.X < region.Min.X ? region.Min.X - pos.X : pos.X - region.Max.X;
            dist += pos.Y < region.Min.Y ? region.Min.Y - pos.Y : pos.Y - region.Max.Y;
            dist += pos.Z < region.Min.Z ? region.Min.Z - pos.Z : pos.Z - region.Max.Z;

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
        var pos = new Vec3D(
            X: numbers[0],
            Y: numbers[1],
            Z: numbers[2]);

        return new Nanobot(pos, numbers[3]);
    }
}