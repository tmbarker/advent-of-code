using Utilities.Geometry.Euclidean;

namespace Solutions.Y2019.D18;

/// <summary>
/// Represents the state of a robot in the tunnels. Note that <see cref="Steps"/> is not
/// considered in <see cref="State"/> equality comparisons
/// </summary>
public readonly struct State : IEquatable<State>
{
    public Vector2D Pos { get; }
    public int Steps { get; }
    private uint KeysMask { get; }

    private State(Vector2D pos, int steps, uint keysMask)
    {
        Pos = pos;
        Steps = steps;
        KeysMask = keysMask;
    }

    public static State Initial(Vector2D startPos)
    {
        return new State(
            pos: startPos,
            steps: 0,
            keysMask: 0U);
    }

    public State AfterStep(Vector2D pos)
    {
        return new State(
            pos: pos,
            steps: Steps + 1,
            keysMask: KeysMask);
    }
    
    public State AfterPickup(Vector2D pos, char key)
    {
        return new State(
            pos: pos,
            steps: Steps + 1,
            keysMask: KeysMask | FormKeyMask(key));
    }

    public bool HasKey(char key)
    {
        return (KeysMask & FormKeyMask(key)) > 0U;
    }

    public bool Equals(State other)
    {
        return KeysMask == other.KeysMask && Pos == other.Pos;
    }

    public override bool Equals(object? obj)
    {
        return obj is State other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(KeysMask, Pos);
    }

    public static bool operator ==(State left, State right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(State left, State right)
    {
        return !left.Equals(right);
    }
    
    private static uint FormKeyMask(char key)
    {
        return 1U << (key - 'a' + 1);
    }
}