using Utilities.Geometry.Euclidean;

namespace Solutions.Y2022.D22;

public readonly struct Instruction(int steps, Quaternion rot)
{
    public int Steps { get; } = steps;
    public Quaternion Rot { get; } = rot;
}