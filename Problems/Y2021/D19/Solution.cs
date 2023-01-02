using Problems.Y2021.Common;
using Utilities.DataStructures.Cartesian;
using Utilities.Extensions;

namespace Problems.Y2021.D19;

/// <summary>
/// Beacon Scanner: https://adventofcode.com/2021/day/19
/// </summary>
public class Solution : SolutionBase2021
{
    private const int IntersectionThreshold = 12;
    
    private static readonly IReadOnlySet<ScannerTransform> ScannerTransforms = new HashSet<ScannerTransform>
    {
        new(Rotation3D.Zero,         Rotation3D.RotationsAroundAxis(Rotation3D.Axis.X)), // +x -> +x
        new(Rotation3D.Positive180Y, Rotation3D.RotationsAroundAxis(Rotation3D.Axis.X)), // +x -> -x
        new(Rotation3D.Positive90Y,  Rotation3D.RotationsAroundAxis(Rotation3D.Axis.Z)), // +x -> +z
        new(Rotation3D.Negative90Y,  Rotation3D.RotationsAroundAxis(Rotation3D.Axis.Z)), // +x -> -z
        new(Rotation3D.Positive90Z,  Rotation3D.RotationsAroundAxis(Rotation3D.Axis.Y)), // +x -> +y
        new(Rotation3D.Negative90Z,  Rotation3D.RotationsAroundAxis(Rotation3D.Axis.Y)), // +x -> -y
    };

    public override int Day => 19;
    
    public override object Run(int part)
    {
        var reportings = Report.Parse(GetInputLines());
        var (scanners, beacons) = CreateMap(reportings);

        return part switch
        {
            0 => beacons.Count,
            1 => ComputeMaxTaxicabDistance(scanners),
            _ => ProblemNotSolvedString,
        };
    }

    private static int ComputeMaxTaxicabDistance(ISet<Vector3D> scanners)
    {
        return scanners
            .Aggregate(0, (max, p1) => scanners.Select(p2 => Vector3D.TaxicabDistance(p1, p2))
            .Prepend(max)
            .Max());
    }
    
    private static (ISet<Vector3D> scanners, ISet<Vector3D> beacons) CreateMap(IList<Reporting> reportings)
    {
        var knownScanners = new HashSet<Vector3D> { Vector3D.Zero };
        var knownBeacons = new HashSet<Vector3D>(reportings[0].Beacons);
        var unmatchedReportings = new List<Reporting>(reportings.Skip(1));
        
        var i = 0;
        while (unmatchedReportings.Count > 0)
        {
            i = ++i % unmatchedReportings.Count;
            if (TryMatchReporting(knownScanners, knownBeacons, unmatchedReportings[i]))
            {
                unmatchedReportings.RemoveAt(i);
            }
        }

        return (knownScanners, knownBeacons);
    }

    private static bool TryMatchReporting(ISet<Vector3D> knownScanners, ISet<Vector3D> knownBeacons, Reporting reporting)
    {
        foreach (var transform in ScannerTransforms)
        {
            foreach (var (r1, r2) in transform.GetRotations())
            {
                if (TryMatchPositions(knownScanners, knownBeacons, TransformPositions(reporting.Beacons, r1, r2)))
                {
                    return true;
                }
            }
        }
        
        return false;
    }

    private static IList<Vector3D> TransformPositions(IEnumerable<Vector3D> pos, Rotation3D r1, Rotation3D r2)
    {
        return pos.Select(p => r2 * (r1 * p)).ToList();
    }
    
    private static bool TryMatchPositions(ISet<Vector3D> knownScanners, ISet<Vector3D> knownBeacons, IList<Vector3D> reportedBeacons)
    {
        foreach (var knownPos in knownBeacons)
        {
            foreach (var reportedPos in reportedBeacons)
            {
                var offset = reportedPos - knownPos;
                var shifted = reportedBeacons.Select(p => p - offset).ToList();

                if (knownBeacons.Intersect(shifted).Count() < IntersectionThreshold)
                {
                    continue;
                }
                
                foreach (var shiftedPos in shifted)
                {
                    knownBeacons.EnsureContains(shiftedPos);
                }

                knownScanners.EnsureContains(Vector3D.Zero - offset);
                return true;
            }
        }

        return false;
    }
}