using Utilities.Geometry.Euclidean;

namespace Solutions.Y2019.D17;

public readonly record struct State(IReadOnlySet<Vec2D> Scaffolding, Pose2D Robot);