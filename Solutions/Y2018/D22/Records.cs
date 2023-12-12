using Utilities.Geometry.Euclidean;

namespace Solutions.Y2018.D22;

public readonly record struct Scan(int Depth, Vector2D Mouth, Vector2D Target);
public readonly record struct Region(long Erosion, RegionType Type);
public readonly record struct State(Vector2D Pos, EquippedTool Tool);
public readonly record struct Transition(State NextState, int Cost);