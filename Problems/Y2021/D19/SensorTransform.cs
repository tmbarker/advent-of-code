using Utilities.DataStructures.Cartesian;

namespace Problems.Y2021.D19;

public readonly struct SensorTransform
{
    public SensorTransform(Rotation3D facingRotation, IEnumerable<Rotation3D> orientationRotations)
    {
        FacingRotation = facingRotation;
        OrientationRotations = new List<Rotation3D>(orientationRotations);
    }
    
    public Rotation3D FacingRotation { get; }
    public IList<Rotation3D> OrientationRotations { get; }
}