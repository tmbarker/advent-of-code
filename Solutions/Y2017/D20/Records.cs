using Utilities.Geometry.Euclidean;

namespace Solutions.Y2017.D20;

public readonly record struct Particle(Vector3D Pos, Vector3D Vel, Vector3D Acc);
public readonly record struct Collision(int Tick, Vector3D Pos);