using Utilities.Cartesian;

namespace Problems.Y2021.D23;

public readonly record struct Move(Vector2D From, Vector2D To, int Cost, MoveType Type);

public enum MoveType
{
    ToHallway,
    ToSideRoom
}