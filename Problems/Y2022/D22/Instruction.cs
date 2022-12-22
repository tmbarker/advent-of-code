using Utilities.DataStructures.Cartesian;

namespace Problems.Y2022.D22;

public readonly struct Instruction
{
    public Instruction(int steps, Rotation2D rotation)
    {
        Steps = steps;
        Rotation = rotation;
    }

    public int Steps { get; }
    public Rotation2D Rotation { get; }
}