using Utilities.Geometry.Euclidean;

namespace Solutions.Y2017.D20;

public readonly record struct Particle(Vec3D Pos, Vec3D Vel, Vec3D Acc);
public readonly record struct Collision(int Tick, Vec3D Pos);