using Utilities.Geometry.Euclidean;

namespace Solutions.Y2022.D22;

public readonly struct Instruction(int steps, Rot3D rot)
{
    public int Steps { get; } = steps;
    public Rot3D Rot { get; } = rot;
}