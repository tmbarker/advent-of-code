using Utilities.Geometry.Euclidean;

namespace Solutions.Y2021.D19;

public readonly struct ScannerTransform(Rotation3D facingRotation, IEnumerable<Rotation3D> orientationRotations)
{
    private Rotation3D FacingRotation { get; } = facingRotation;
    private IList<Rotation3D> OrientationRotations { get; } = new List<Rotation3D>(orientationRotations);

    public IEnumerable<(Rotation3D r1, Rotation3D r2)> GetRotations()
    {
        foreach (var orientationRotation in OrientationRotations)
        {
            yield return (FacingRotation, orientationRotation);
        }
    }
}