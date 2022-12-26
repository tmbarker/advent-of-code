namespace Utilities.DataStructures.Cartesian;

/// <summary>
/// A readonly Vector value type
/// </summary>
public readonly struct Vector2D : IPosition2D, IEquatable<Vector2D>
{
    private const string StringFormat = "[{0},{1}]";

    public static readonly Vector2D Zero = new(0, 0);
    public static readonly Vector2D Up = new(0, 1);
    public static readonly Vector2D Down = new(0, -1);
    public static readonly Vector2D Left = new(-1, 0);
    public static readonly Vector2D Right = new(1, 0);
    public static readonly Vector2D One = new(1, 1);
    public static readonly Vector2D PositiveInfinity = new (int.MaxValue, int.MaxValue);

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

    public static Vector2D Normalize(Vector2D vector)
    {
        return new Vector2D(Math.Sign(vector.X), Math.Sign(vector.Y));
    }
    
    public static int ChebyshevDistance(IPosition2D a, IPosition2D b)
    {
        var dx = Math.Abs(a.X - b.X);
        var dy = Math.Abs(a.Y - b.Y);

        return Math.Max(dx, dy);
    }
    
    public static int TaxicabDistance(IPosition2D a, IPosition2D b)
    {
        var dx = Math.Abs(a.X - b.X);
        var dy = Math.Abs(a.Y - b.Y);

        return dx + dy;
    }

    public static Vector2D operator +(Vector2D lhs, Vector2D rhs)
    {
        return new Vector2D(lhs.X + rhs.X, lhs.Y + rhs.Y);
    }

    public static Vector2D operator -(Vector2D lhs, Vector2D rhs)
    {
        return new Vector2D(lhs.X - rhs.X, lhs.Y - rhs.Y);
    }

    public static Vector2D operator *(int k, Vector2D rhs)
    {
        return new Vector2D(k * rhs.X, k * rhs.Y);
    }

    public static bool operator ==(Vector2D left, Vector2D right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Vector2D left, Vector2D right)
    {
        return !(left == right);
    }
    
    public bool Equals(Vector2D other)
    {
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object? obj)
    {
        return obj is Vector2D other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
    
    public override string ToString()
    {
        return Id;
    }
}