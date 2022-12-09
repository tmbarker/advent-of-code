namespace Utilities.DataStructures.Grid;

/// <summary>
/// A positional value type which can be used to index <see cref="Grid2D{T}"/> instances
/// </summary>
public readonly struct Vector2D : IPosition2D
{
    private const string StringFormat = "[{0},{1}]";

    public static readonly Vector2D Zero = new(0, 0);
    public static readonly Vector2D Up = new(0, 1);
    public static readonly Vector2D Down = new(0, -1);
    public static readonly Vector2D Left = new(-1, 0);
    public static readonly Vector2D Right = new(1, 0);
    
    public string Id { get; }
    public int X { get; }
    public int Y { get; }
    
    public Vector2D(int x, int y)
    {
        X = x;
        Y = y;
        
        Id = string.Format(StringFormat, X, Y);
    }

    public Vector2D(Vector2D other) : this(other.X, other.Y)
    {
    }

    public static Vector2D operator + (Vector2D lhs, Vector2D rhs)
    {
        return new Vector2D(lhs.X + rhs.X, lhs.Y + rhs.Y);
    }
    
    public static Vector2D operator - (Vector2D lhs, Vector2D rhs)
    {
        return new Vector2D(lhs.X - rhs.X, lhs.Y - rhs.Y);
    }

    public static Vector2D operator * (int k, Vector2D rhs)
    {
        return new Vector2D(k * rhs.X, k * rhs.Y);
    }

    public static bool operator == (Vector2D lhs, Vector2D rhs)
    {
        return lhs.X == rhs.X && lhs.Y == rhs.Y;
    }

    public static bool operator !=(Vector2D lhs, Vector2D rhs)
    {
        return !(lhs == rhs);
    }

    public override string ToString()
    {
        return Id;
    }
}