using Utilities.Geometry;
using Utilities.Geometry.Euclidean;

namespace Problems.Y2021.D23;

public readonly struct State : IEquatable<State>
{
    private readonly string _key;
    
    public int Cost { get; }
    public Dictionary<Vector2D, char> ActorsMap { get; }

    private State (int cost, IDictionary<Vector2D, char> positions)
    {
        _key = BuildActorsMapHash(positions);

        Cost = cost;
        ActorsMap = new Dictionary<Vector2D, char>(positions);
    }

    public static State FromInitialPositions(IDictionary<Vector2D, char> positions)
    {
        return new State(cost: 0, positions);
    }
    
    public State AfterMove(Move move)
    {
        var nextPositions = new Dictionary<Vector2D, char>();
        foreach (var (pos, actor) in ActorsMap)
        {
            nextPositions.Add(pos == move.From ? move.To : pos, actor);
        }

        return new State(Cost + move.Cost, nextPositions);
    }

    private static string BuildActorsMapHash(IDictionary<Vector2D, char> positions)
    {
        var coords = positions
            .Select(kvp => $"{kvp.Value}=({kvp.Key.X},{kvp.Key.Y})")
            .Order();
        
        return string.Join(',', coords);
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