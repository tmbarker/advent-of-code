using Utilities.Geometry.Euclidean;

namespace Solutions.Y2021.D19;

public sealed class Reporting
{
    public Reporting(int scannerId, IEnumerable<Vec3D> beacons)
    {
        ScannerId = scannerId;
        Beacons = new HashSet<Vec3D>(beacons);
    }
    
    public int ScannerId { get; }
    public IReadOnlySet<Vec3D> Beacons { get; }
}