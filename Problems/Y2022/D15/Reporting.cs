using Utilities.DataStructures.Cartesian;

namespace Problems.Y2022.D15;

public readonly struct Reporting
{
    public Reporting(Vector2D sensorPos, Vector2D beaconPos)
    {
        SensorPos = sensorPos;
        BeaconPos = beaconPos;
    }
    
    public Vector2D SensorPos { get; }
    public Vector2D BeaconPos { get; }
}