using Utilities.Geometry.Euclidean;

namespace Solutions.Y2022.D22;

public readonly struct Instruction(int steps, Rotation3D rotation)
{
    public int Steps { get; } = steps;
    public Rotation3D Rotation { get; } = rotation;
}