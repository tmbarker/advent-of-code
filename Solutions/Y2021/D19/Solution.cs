using Utilities.Collections;
using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2021.D19;

[PuzzleInfo("Beacon Scanner", Topics.Vectors, Difficulty.Hard, favourite: true)]
public sealed class Solution : SolutionBase
{
    private static readonly HashSet<Quaternion> PossibleTransforms = EvaluatePossibleTransforms();
    
    public override object Run(int part)
    {
        var reportings = ParseReportings();
        var map = BuildMap(reportings);

        return part switch
        {
            1 => map.GetDistinctBeaconCount(),
            2 => map.GetMaxScannerDistance(),
            _ => PuzzleNotSolvedString
        };
    }

    private static HashSet<Quaternion> EvaluatePossibleTransforms()
    {
        Transform[] possibleTransforms =
        [
            new(FacingTransform: Rot3D.Zero,  RotationAxis: Axis.X), // +x -> +x
            new(FacingTransform: Rot3D.P180Y, RotationAxis: Axis.X), // +x -> -x
            new(FacingTransform: Rot3D.P90Y,  RotationAxis: Axis.Z), // +x -> +z
            new(FacingTransform: Rot3D.N90Y,  RotationAxis: Axis.Z), // +x -> -z
            new(FacingTransform: Rot3D.P90Z,  RotationAxis: Axis.Y), // +x -> +y
            new(FacingTransform: Rot3D.N90Z,  RotationAxis: Axis.Y)  // +x -> -y
        ];

        var set = new HashSet<Quaternion>(capacity: 4 * possibleTransforms.Length);
        foreach (var (face, axis) in possibleTransforms)
        {
            set.Add(face * Rot3D.FromAxisAngle(axis, angleDeg: Degrees.Zero));
            set.Add(face * Rot3D.FromAxisAngle(axis, angleDeg: Degrees.P90));
            set.Add(face * Rot3D.FromAxisAngle(axis, angleDeg: Degrees.P180));
            set.Add(face * Rot3D.FromAxisAngle(axis, angleDeg: Degrees.N90));
        }

        return set;
    }
    
    private Map BuildMap(Reporting[] reportings)
    {
        var map = new Map(reportings[0]);
        var unmatched = reportings[1..].ToList();
        var incongruentMemo = new DefaultDict<int, HashSet<int>>(defaultSelector: _ => []);

        var i = 0;
        while (unmatched.Count > 0)
        {
            if (TryMapReporting(unmatched[i], map, incongruentMemo))
            {
                unmatched.RemoveAt(i);
            }

            if (unmatched.Count != 0)
            {
                i = ++i % unmatched.Count;
            }
        }

        return map;
    }

    private bool TryMapReporting(Reporting reporting, Map map, DefaultDict<int, HashSet<int>> incongruentMemo)
    {
        foreach (var (knownScannerId, knownBeacons) in map.KnownBeacons)
        {
            if (incongruentMemo[reporting.ScannerId].Contains(knownScannerId))
            {
                continue;
            }
            
            foreach (var rotation in PossibleTransforms)
            {
                var transformedPositions = reporting.Beacons.Select(rotation.Transform).ToArray();
                if (!TryFindOffset(knownBeacons, transformedPositions, out var offset))
                {
                    continue;
                }

                var absolutePositions = transformedPositions.Select(p => p + offset);
                map.KnownScanners.Add(reporting.ScannerId, offset);
                map.KnownBeacons.Add(reporting.ScannerId, [..absolutePositions]);

                LogMatch(reporting.ScannerId, knownScannerId, offset);
                return true;
            }

            incongruentMemo[reporting.ScannerId].Add(knownScannerId);
        }

        return false;
    }

    private static bool TryFindOffset(HashSet<Vec3D> known, Vec3D[] reported, out Vec3D offset)
    {
        int matched;
        
        foreach (var knownPos in known)
        foreach (var reportedPos in reported)
        {
            offset = knownPos - reportedPos;
            matched = 0;
            
            foreach (var pos in reported)
            {
                if (known.Contains(pos + offset) && ++matched >= 12)
                {
                    return true;
                }
            }
        }

        offset = Vec3D.Zero;
        return false;
    }
    
    private Reporting[] ParseReportings()
    {
        return GetInputLines()
            .ChunkByNonEmpty()
            .Select(chunk => new Reporting(
                ScannerId: chunk[0].ParseInt(),
                Beacons:   chunk[1..].Select(Vec3D.Parse).ToArray()))
            .ToArray();
    }
    
    private void LogMatch(int found, int against, Vec3D pos)
    {
        Log($"Matched scanner #{found:D2} to known beacons from scanner #{against:D2}, pos => {pos}");
    }
}