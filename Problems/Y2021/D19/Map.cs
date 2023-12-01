using Utilities.Geometry.Euclidean;

namespace Problems.Y2021.D19;

public class Map
{
    public Dictionary<int, Vector3D> KnownScanners { get; } = new();
    public Dictionary<int, ISet<Vector3D>> KnownBeacons { get; } = new();

    public Map(Reporting referenceReporting)
    {
        KnownScanners.Add(referenceReporting.ScannerId, Vector3D.Zero);
        KnownBeacons.Add(referenceReporting.ScannerId, new HashSet<Vector3D>(referenceReporting.Beacons));
    }
    
    public int GetDistinctBeaconCount()
    {
        return KnownBeacons
            .SelectMany(kvp => kvp.Value)
            .Distinct()
            .Count();
    }

    public int GetMaxScannerDistance(Metric metric)
    {
        var max = 0;
        var scanners = KnownScanners.Values.ToList();
        
        // ReSharper disable once LoopCanBeConvertedToQuery
        for (var i = 0; i < scanners.Count; i++)
        for (var j = 0; j < scanners.Count; j++)
        {
            if (i != j)
            {
                max = Math.Max(max, Vector3D.Distance(scanners[i], scanners[j], metric));
            }
        }

        return max;
    }
}