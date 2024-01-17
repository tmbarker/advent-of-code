using Utilities.Geometry.Euclidean;

namespace Solutions.Y2022.D15;

public readonly struct Reporting(Vec2D sensorPos, Vec2D beaconPos)
{
    public Vec2D SensorPos { get; } = sensorPos;
    public Vec2D BeaconPos { get; } = beaconPos;
    public int Range { get; } = Vec2D.Distance(a: sensorPos, b: beaconPos, Metric.Taxicab);
}