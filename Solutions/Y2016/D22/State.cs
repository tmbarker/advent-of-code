using System.Collections.Immutable;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2016.D22;

public readonly struct State(Vec2D targetData, ICollection<Vec2D> emptyNodes) : IEquatable<State>
{
    private static readonly PosComparer PosComparer = new();
    private readonly string _key = BuildKey(targetData, emptyNodes);
    
    public Vec2D TargetData { get; } = targetData;
    public ImmutableHashSet<Vec2D> EmptyNodes { get; } = [..emptyNodes];

    public State AfterDataMove(Vec2D from, Vec2D to)
    {
        var resultingEmpty = EmptyNodes
            .Remove(to)
            .Append(from)
            .ToList();
        
        return new State(
            targetData: from == TargetData ? to : TargetData,
            emptyNodes: resultingEmpty);
    }
    
    private static string BuildKey(Vec2D targetData, IEnumerable<Vec2D> emptyNodes)
    {
        var ordered = emptyNodes.Order(PosComparer);
        return $"<target data: {targetData}><empty nodes: {string.Join(string.Empty, ordered)}>";
    }

    public bool Equals(State other)
    {
        return _key == other._key;
    }

    public override bool Equals(object? obj)
    {
        return obj is State other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _key.GetHashCode();
    }

    public static bool operator ==(State left, State right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(State left, State right)
    {
        return !left.Equals(right);
    }
}