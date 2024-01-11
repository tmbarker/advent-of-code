using Utilities.Geometry.Euclidean;

namespace Solutions.Y2022.D15;

public readonly struct Reporting(Vector2D sensorPos, Vector2D beaconPos)
{
    public Vector2D SensorPos { get; } = sensorPos;
    public Vector2D BeaconPos { get; } = beaconPos;
    public int Range { get; } = Vector2D.Distance(a: sensorPos, b: beaconPos, Metric.Taxicab);
}