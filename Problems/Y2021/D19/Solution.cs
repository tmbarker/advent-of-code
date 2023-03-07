using Problems.Attributes;
using Problems.Y2021.Common;
using Utilities.Cartesian;

namespace Problems.Y2021.D19;

/// <summary>
/// Beacon Scanner: https://adventofcode.com/2021/day/19
/// </summary>
[Favourite("Beacon Scanner", Topics.Vectors, Difficulty.Hard)]
public class Solution : SolutionBase2021
{
    private const int IntersectionThreshold = 12;
    
    private static readonly IReadOnlySet<ScannerTransform> ScannerTransforms = new HashSet<ScannerTransform>
    {
        new(Rotation3D.Zero,         Rotation3D.RotationsAroundAxis(Axis.X)), // +x -> +x
        new(Rotation3D.Positive180Y, Rotation3D.RotationsAroundAxis(Axis.X)), // +x -> -x
        new(Rotation3D.Positive90Y,  Rotation3D.RotationsAroundAxis(Axis.Z)), // +x -> +z
        new(Rotation3D.Negative90Y,  Rotation3D.RotationsAroundAxis(Axis.Z)), // +x -> -z
        new(Rotation3D.Positive90Z,  Rotation3D.RotationsAroundAxis(Axis.Y)), // +x -> +y
        new(Rotation3D.Negative90Z,  Rotation3D.RotationsAroundAxis(Axis.Y)), // +x -> -y
    };

    public override int Day => 19;
    
    public override object Run(int part)
    {
        var reportings = Report.Parse(GetInputLines());
        var map = BuildMap(reportings);

        return part switch
        {
            1 => map.GetDistinctBeaconCount(),
            2 => map.GetMaxScannerDistance(DistanceMetric.Taxicab),
            _ => ProblemNotSolvedString
        };
    }

    private Map BuildMap(IList<Reporting> reportings)
    {
        var map = new Map(reportings[0]);
        var unmatchedReportings = new List<Reporting>(reportings.Skip(1));
        var incongruent = reportings.ToDictionary(
            keySelector: r => r.ScannerId,
            elementSelector: _ => new HashSet<int>());

        var i = 0;
        while (unmatchedReportings.Count > 0)
        {
            if (TryMapReporting(unmatchedReportings[i], map, incongruent))
            {
                unmatchedReportings.RemoveAt(i);
            }

            if (unmatchedReportings.Any())
            {
                i = ++i % unmatchedReportings.Count;
            }
        }

        return map;
    }

    private bool TryMapReporting(Reporting reporting, Map map, IDictionary<int, HashSet<int>> incongruent)
    {
        foreach (var (knownScannerId, knownBeacons) in map.KnownBeacons)
        {
            if (incongruent[reporting.ScannerId].Contains(knownScannerId))
            {
                continue;
            }
            
            foreach (var (r1, r2) in GetTransformations())
            {
                var transformedPositions = TransformPositions(reporting.Beacons, r1, r2);
                if (!TryFindOffset(knownBeacons, transformedPositions, out var offset))
                {
                    continue;
                }

                var absolutePositions = transformedPositions.Select(p => p + offset);
                map.KnownScanners.Add(reporting.ScannerId, offset);
                map.KnownBeacons.Add(reporting.ScannerId, new HashSet<Vector3D>(absolutePositions));

                LogMatch(reporting.ScannerId, knownScannerId, offset);
                return true;
            }

            incongruent[reporting.ScannerId].Add(knownScannerId);
        }

        return false;
    }

    private static bool TryFindOffset(ISet<Vector3D> known, IList<Vector3D> reported, out Vector3D offset)
    {
        foreach (var knownPos in known)
        {
            foreach (var reportedPos in reported)
            {
                offset = knownPos - reportedPos;
                
                var offsetNoClosure = offset;
                var shifted = reported.Select(p => p + offsetNoClosure);

                if (known.Intersect(shifted).Count() >= IntersectionThreshold)
                {
                    return true;
                }
            }
        }

        offset = Vector3D.Zero;
        return false;
    }

    private static IEnumerable<(Rotation3D r1, Rotation3D r2)> GetTransformations()
    {
        return ScannerTransforms.SelectMany(transform => transform.GetRotations());
    }

    private static IList<Vector3D> TransformPositions(IEnumerable<Vector3D> pos, Rotation3D r1, Rotation3D r2)
    {
        return pos.Select(p => r2 * (r1 * p)).ToList();
    }

    private void LogMatch(int found, int against, Vector3D pos)
    {
        if (LogsEnabled)
        {
            Console.WriteLine($"Matched scanner #{found:D2} to known beacons from scanner #{against:D2}, pos => {pos}");   
        }
    }
}