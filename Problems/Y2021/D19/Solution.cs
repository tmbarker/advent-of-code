using Problems.Y2021.Common;
using Utilities.DataStructures.Cartesian;

namespace Problems.Y2021.D19;

/// <summary>
/// Beacon Scanner: https://adventofcode.com/2021/day/19
/// </summary>
public class Solution : SolutionBase2021
{
    // TODO: Are these constants needed
    private const int SensorRange = 1000;
    private const int BeaconUnionThreshold = 12;

    // TODO: Need Camera View Transform Matrix/Perspective Projection Matrix/Perspective Matrix
    private static readonly IReadOnlySet<Rotation3D> Transforms = new HashSet<Rotation3D>()
    {
        Rotation3D.Zero,            // +x -> +x
        Rotation3D.Negative90Y,     // +x -> -z
        Rotation3D.Positive90Y,     // +x -> +z
        Rotation3D.Positive180Y,    // +x -> -x
        Rotation3D.Negative90Z,     // +x -> -y
        Rotation3D.Positive90Z,     // +x -> +y
    };
    
    public override int Day => 19;
    
    public override object Run(int part)
    {
        Report.Parse(GetInputLines(), out var reportings);
        return part switch
        {
            0 => Scratch(reportings),
            _ => ProblemNotSolvedString,
        };
    }

    private string Scratch(IList<Reporting> reportings)
    {
        var beacons = reportings[0].Beacons;
        foreach (var rotation in Transforms)
        {
            Print(beacons, rotation);
        }

        return ProblemNotSolvedString;
    }

    private static void Print(IEnumerable<Vector3D> positions, Rotation3D rot)
    {
        Console.WriteLine(rot);
        foreach (var p in positions)
        {
            Console.WriteLine(rot * p);
        }
    }
}