using Utilities.Geometry.Euclidean;

namespace Solutions.Y2021.D19;

public sealed class Reporting(int scannerId, IEnumerable<Vec3D> beacons)
{
    public int ScannerId { get; } = scannerId;
    public IReadOnlySet<Vec3D> Beacons { get; } = new HashSet<Vec3D>(beacons);
}