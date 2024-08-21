using Utilities.Geometry.Euclidean;

namespace Solutions.Y2021.D19;

[PuzzleInfo("Beacon Scanner", Topics.Vectors, Difficulty.Hard, favourite: true)]
public sealed class Solution : SolutionBase
{
    private const int IntersectionThreshold = 12;

    private static readonly List<(Rot3D r1, Rot3D r2)> TransformPermutations;
    private static readonly ScannerTransform[] ScannerTransforms =
    [
        new ScannerTransform(Rot3D.Zero,  Rot3D.RotationsAroundAxis(Axis.X)), // +x -> +x
        new ScannerTransform(Rot3D.P180Y, Rot3D.RotationsAroundAxis(Axis.X)), // +x -> -x
        new ScannerTransform(Rot3D.P90Y,  Rot3D.RotationsAroundAxis(Axis.Z)), // +x -> +z
        new ScannerTransform(Rot3D.N90Y,  Rot3D.RotationsAroundAxis(Axis.Z)), // +x -> -z
        new ScannerTransform(Rot3D.P90Z,  Rot3D.RotationsAroundAxis(Axis.Y)), // +x -> +y
        new ScannerTransform(Rot3D.N90Z,  Rot3D.RotationsAroundAxis(Axis.Y))  // +x -> -y
    ];

    static Solution()
    {
        TransformPermutations = EvaluateTransformPermutations();
    }
    
    public override object Run(int part)
    {
        var reportings = Report.Parse(GetInputLines());
        var map = BuildMap(reportings);

        return part switch
        {
            1 => map.GetDistinctBeaconCount(),
            2 => map.GetMaxScannerDistance(Metric.Taxicab),
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

            if (unmatchedReportings.Count != 0)
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
            
            foreach (var (r1, r2) in TransformPermutations)
            {
                var transformedPositions = TransformPositions(reporting.Beacons, r1, r2);
                if (!TryFindOffset(knownBeacons, transformedPositions, out var offset))
                {
                    continue;
                }

                var absolutePositions = transformedPositions.Select(p => p + offset);
                map.KnownScanners.Add(reporting.ScannerId, offset);
                map.KnownBeacons.Add(reporting.ScannerId, new HashSet<Vec3D>(absolutePositions));

                LogMatch(reporting.ScannerId, knownScannerId, offset);
                return true;
            }

            incongruent[reporting.ScannerId].Add(knownScannerId);
        }

        return false;
    }

    private static bool TryFindOffset(ISet<Vec3D> known, IList<Vec3D> reported, out Vec3D offset)
    {
        int matched;
        
        foreach (var knownPos in known)
        foreach (var reportedPos in reported)
        {
            offset = knownPos - reportedPos;
            matched = 0;
            
            for (var i = 0; i < reported.Count; i++)
            {
                if (known.Contains(reported[i] + offset) && ++matched >= IntersectionThreshold)
                {
                    return true;
                }
            }
        }

        offset = Vec3D.Zero;
        return false;
    }

    private static List<(Rot3D r1, Rot3D r2)> EvaluateTransformPermutations()
    {
        return ScannerTransforms
            .SelectMany(transform => transform.GetRotations())
            .ToList();
    }

    private static List<Vec3D> TransformPositions(IEnumerable<Vec3D> pos, Rot3D r1, Rot3D r2)
    {
        return pos.Select(p => r2 * (r1 * p)).ToList();
    }

    private void LogMatch(int found, int against, Vec3D pos)
    {
        if (LogsEnabled)
        {
            Console.WriteLine($"Matched scanner #{found:D2} to known beacons from scanner #{against:D2}, pos => {pos}");   
        }
    }
}