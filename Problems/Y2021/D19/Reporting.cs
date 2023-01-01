using Utilities.DataStructures.Cartesian;

namespace Problems.Y2021.D19;

public class Reporting
{
    public Reporting(int scannerId, IEnumerable<Vector3D> beacons)
    {
        ScannerId = scannerId;
        Beacons = new HashSet<Vector3D>(beacons);
    }
    
    public int ScannerId { get; }
    public IReadOnlySet<Vector3D> Beacons { get; }
}