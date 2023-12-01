using Utilities.Geometry.Euclidean;

namespace Problems.Y2022.D15;

public readonly struct Reporting
{
    public Reporting(Vector2D sensorPos, Vector2D beaconPos)
    {
        SensorPos = sensorPos;
        BeaconPos = beaconPos;
        Range = Vector2D.Distance(sensorPos, beaconPos, Metric.Taxicab);
    }
    
    public Vector2D SensorPos { get; }
    public Vector2D BeaconPos { get; }
    public int Range { get; }
}