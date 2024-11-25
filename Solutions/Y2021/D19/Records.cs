using Utilities.Geometry.Euclidean;

namespace Solutions.Y2021.D19;

public readonly record struct Transform(Quaternion FacingTransform, Axis RotationAxis);
public readonly record struct Reporting(int ScannerId, Vec3D[] Beacons);