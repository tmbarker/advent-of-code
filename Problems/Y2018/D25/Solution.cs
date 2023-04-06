using Problems.Y2018.Common;
using Utilities.Cartesian;
using Utilities.Extensions;

namespace Problems.Y2018.D25;

using Constellation = List<Vector4D>;

/// <summary>
/// Four-Dimensional Adventure: https://adventofcode.com/2018/day/25
/// </summary>
public class Solution : SolutionBase2018
{
    public override int Day => 25;
    public override int Parts => 1;
    
    public override object Run(int part)
    {
        return part switch
        {
            1 => CountConstellations(),
            _ => ProblemNotSolvedString
        };
    }

    private int CountConstellations()
    {
        var points = ParseInputLines(parseFunc: ParsePoint).ToList();
        var constellationMap = new Dictionary<int, Constellation>();

        for (var i = 0; i < points.Count; i++)
        {
            var inConstellations = new List<int>();
            foreach (var (key, constellation) in constellationMap)
            {
                if (constellation.Any(point => Vector4D.Distance(a: points[i], b: point, metric: DistanceMetric.Taxicab) <= 3))
                {
                    inConstellations.Add(key);
                }
            }

            var newConstellation = new Constellation { points[i] };
            if (inConstellations.Any())
            {
                foreach (var key in inConstellations)
                {
                    newConstellation.AddRange(constellationMap[key]);
                    constellationMap.Remove(key);
                }
            }

            constellationMap.Add(i, newConstellation);
        }

        return constellationMap.Count;
    }

    private static Vector4D ParsePoint(string line)
    {
        var numbers = line.ParseInts();
        return new Vector4D(
            x: numbers[0],
            y: numbers[1],
            z: numbers[2],
            w: numbers[3]);
    }
}