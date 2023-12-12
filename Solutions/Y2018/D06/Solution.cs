using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2018.D06;

[PuzzleInfo("Chronal Coordinates", Topics.Vectors, Difficulty.Medium, favourite: true)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var pois = ParseInputLines(parseFunc: ParsePointOfInterest).ToList();
        return part switch
        {
            1 => GetLargestFiniteArea(pois),
            2 => GetLargestProximalArea(pois, maxDistance: 10000),
            _ => ProblemNotSolvedString
        };
    }

    private static int GetLargestFiniteArea(ICollection<Vector2D> pois)
    {
        var aabb = new Aabb2D(extents: pois);
        var areaCounts = pois.ToDictionary(
            keySelector: poi => poi,
            elementSelector: _ => 0);

        foreach (var coord in aabb)
        {
            var distances = new Dictionary<int, IList<Vector2D>>();
            foreach (var poi in pois)
            {
                var distance = Vector2D.Distance(
                    a: coord,
                    b: poi,
                    metric: Metric.Taxicab);
                
                distances.TryAdd(distance, new List<Vector2D>());
                distances[distance].Add(poi);
            }

            var minDistance = distances.Keys.Min();
            var poisAtDistance = distances[minDistance];

            if (poisAtDistance.Count == 1)
            {
                areaCounts[poisAtDistance.Single()]++;
            }
        }

        var finitePointsOfInterest = pois.Where(p => aabb.Contains(p, inclusive: false));
        return finitePointsOfInterest.Max(poi => areaCounts[poi]);
    }

    private static int GetLargestProximalArea(ICollection<Vector2D> pois, int maxDistance)
    {
        var aabb = new Aabb2D(extents: pois);
        return aabb.Count(pos => pois.Sum(poi => Vector2D.Distance(
            a: pos,
            b: poi,
            metric: Metric.Taxicab)) < maxDistance);
    }
    
    private static Vector2D ParsePointOfInterest(string line)
    {
        var numbers = line.ParseInts();
        return new Vector2D(
            x: numbers[0],
            y: numbers[1]);
    }
}