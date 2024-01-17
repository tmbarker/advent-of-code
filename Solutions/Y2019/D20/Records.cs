using Utilities.Geometry.Euclidean;

namespace Solutions.Y2019.D20;

public readonly record struct State(Vec2D Pos, int Depth);
public readonly record struct PortalEntrance(Vec2D Pos, EntranceType Type);