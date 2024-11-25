using Utilities.Geometry.Euclidean;

namespace Solutions.Y2021.D19;

public sealed class Map
{
    public Dictionary<int, Vec3D> KnownScanners { get; } = new();
    public Dictionary<int, HashSet<Vec3D>> KnownBeacons { get; } = new();

    public Map(Reporting referenceReporting)
    {
        KnownScanners.Add(referenceReporting.ScannerId, Vec3D.Zero);
        KnownBeacons.Add(referenceReporting.ScannerId, [..referenceReporting.Beacons]);
    }
    
    public int GetDistinctBeaconCount()
    {
        return KnownBeacons
            .SelectMany(kvp => kvp.Value)
            .Distinct()
            .Count();
    }

    public int GetMaxScannerDistance()
    {
        var max = 0;
        var scanners = KnownScanners.Values.ToList();
        
        // ReSharper disable once LoopCanBeConvertedToQuery
        for (var i = 0; i < scanners.Count; i++)
        for (var j = 0; j < scanners.Count; j++)
        {
            if (i != j)
            {
                max = Math.Max(max, Vec3D.Distance(scanners[i], scanners[j], metric: Metric.Taxicab));
            }
        }

        return max;
    }
}