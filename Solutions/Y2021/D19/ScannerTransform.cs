using Utilities.Geometry.Euclidean;

namespace Solutions.Y2021.D19;

public readonly struct ScannerTransform(Rot3D facingRot, IEnumerable<Rot3D> orientationRotations)
{
    private Rot3D FacingRot { get; } = facingRot;
    private IList<Rot3D> OrientationRotations { get; } = new List<Rot3D>(orientationRotations);

    public IEnumerable<(Rot3D r1, Rot3D r2)> GetRotations()
    {
        foreach (var orientationRotation in OrientationRotations)
        {
            yield return (FacingRot, orientationRotation);
        }
    }
}