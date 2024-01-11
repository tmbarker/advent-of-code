using Utilities.Geometry.Euclidean;

namespace Solutions.Y2019.D12;

public readonly struct StateComp(Axis component, State state) : IEquatable<StateComp>
{
    private int Pos { get; } = state.Pos.GetComponent(component);
    private int Vel { get; } = state.Vel.GetComponent(component);

    public bool Equals(StateComp other)
    {
        return Pos == other.Pos && Vel == other.Vel;
    }

    public override bool Equals(object? obj)
    {
        return obj is StateComp other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Pos, Vel);
    }

    public static bool operator ==(StateComp left, StateComp right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(StateComp left, StateComp right)
    {
        return !left.Equals(right);
    }
}