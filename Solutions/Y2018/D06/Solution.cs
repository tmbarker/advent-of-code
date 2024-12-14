using Utilities.Collections;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2018.D06;

[PuzzleInfo("Chronal Coordinates", Topics.Vectors, Difficulty.Medium, favourite: true)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var pois = ParseInputLines(parseFunc: Vec2D.Parse);
        return part switch
        {
            1 => GetLargestFiniteArea(pois),
            2 => GetLargestProximalArea(pois, maxDistance: 10000),
            _ => PuzzleNotSolvedString
        };
    }

    private static int GetLargestFiniteArea(ICollection<Vec2D> pois)
    {
        var aabb = new Aabb2D(extents: pois);
        var areaCounts = pois.ToDictionary(
            keySelector: poi => poi,
            elementSelector: _ => 0);

        foreach (var coord in aabb)
        {
            var distances = new DefaultDict<int, List<Vec2D>>(defaultSelector: _ => []);
            foreach (var poi in pois)
            {
                var distance = Vec2D.Distance(
                    a: coord,
                    b: poi,
                    metric: Metric.Taxicab);
                
                distances[distance].Add(poi);
            }

            var minDistance = distances.Keys.Min();
            var poisAtDistance = distances[minDistance];

            if (poisAtDistance.Count == 1)
            {
                areaCounts[poisAtDistance.Single()]++;
            }
        }

        return pois
            .Where(p => aabb.Contains(p, inclusive: false))
            .Max(poi => areaCounts[poi]);
    }

    private static int GetLargestProximalArea(ICollection<Vec2D> pois, int maxDistance)
    {
        var aabb = new Aabb2D(extents: pois);
        return aabb.Count(pos => pois.Sum(poi => Vec2D.Distance(
            a: pos,
            b: poi,
            metric: Metric.Taxicab)) < maxDistance);
    }
}