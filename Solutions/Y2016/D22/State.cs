using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2016.D22;

public readonly struct State(Vector2D targetData, ICollection<Vector2D> emptyNodes) : IEquatable<State>
{
    private static readonly PosComparer PosComparer = new();
    private readonly string _key = BuildKey(targetData, emptyNodes);
    
    public Vector2D TargetData { get; } = targetData;
    public HashSet<Vector2D> EmptyNodes { get; } = emptyNodes.ToHashSet();

    public State AfterDataMove(Vector2D from, Vector2D to)
    {
        var resultingEmpty = EmptyNodes
            .Except(to)
            .Append(from)
            .ToList();
        
        return new State(
            targetData: from == TargetData ? to : TargetData,
            emptyNodes: resultingEmpty);
    }
    
    private static string BuildKey(Vector2D targetData, IEnumerable<Vector2D> emptyNodes)
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