using Utilities.Geometry.Euclidean;

namespace Problems.Y2019.D20;

public readonly record struct State(Vector2D Pos, int Depth);
public readonly record struct PortalEntrance(Vector2D Pos, EntranceType Type);