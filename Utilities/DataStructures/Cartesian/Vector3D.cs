namespace Utilities.DataStructures.Cartesian;

/// <summary>
/// A readonly integral 3D Vector value type
/// </summary>
public readonly struct Vector3D : IEquatable<Vector3D>
{
    private const string StringFormat = "[{0},{1},{2}]";

    public static readonly Vector3D Zero = new(0, 0, 0);
    public static readonly Vector3D Up = new(0, 1, 0);
    public static readonly Vector3D Down = new(0, -1, 0);
    public static readonly Vector3D Left = new(-1, 0, 0);
    public static readonly Vector3D Right = new(1, 0, 0);
    public static readonly Vector3D Forward = new(0, 0, 1);
    public static readonly Vector3D Back = new(0, 0, -1);

    private string Id { get; }
    public int X { get; }
    public int Y { get; }
    public int Z { get; }
    
    public Vector3D(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
        
        Id = string.Format(StringFormat, X, Y, Z);
    }

    public void Deconstruct(out int x, out int y, out int z)
    {
        x = X;
        y = Y;
        z = Z;
    }

    public static Vector3D Normalize(Vector3D vector)
    {
        return new Vector3D(Math.Sign(vector.X), Math.Sign(vector.Y), Math.Sign(vector.Z));
    }

    public int Dot(Vector3D other)
    {
        return X * other.X + Y * other.Y + Z * other.Z;
    }

    public static int Distance(Vector3D a, Vector3D b, DistanceMetric metric)
    {
        switch (metric)
        {
            case DistanceMetric.Chebyshev:
                return ChebyshevDistance(a, b);
            case DistanceMetric.Taxicab:
                return TaxicabDistance(a, b);
            case DistanceMetric.Euclidean:
            default:
                throw new ArgumentOutOfRangeException(nameof(metric), metric, null);
        }
    }

    public static implicit operator Vector3D(Vector2D v) => new(v.X, v.Y, 0);
    
    public static Vector3D operator +(Vector3D lhs, Vector3D rhs)
    {
        return new Vector3D(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);
    }

    public static Vector3D operator -(Vector3D lhs, Vector3D rhs)
    {
        return new Vector3D(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);
    }

    public static Vector3D operator *(int k, Vector3D rhs)
    {
        return new Vector3D(k * rhs.X, k * rhs.Y, k * rhs.Z);
    }

    public static bool operator ==(Vector3D left, Vector3D right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Vector3D left, Vector3D right)
    {
        return !(left == right);
    }
    
    public bool Equals(Vector3D other)
    {
        return X == other.X && Y == other.Y && Z == other.Z;
    }

    public override bool Equals(object? obj)
    {
        return obj is Vector3D other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }
    
    public override string ToString()
    {
        return Id;
    }
    
    private static int ChebyshevDistance(Vector3D a, Vector3D b)
    {
        var dx = Math.Abs(a.X - b.X);
        var dy = Math.Abs(a.Y - b.Y);
        var dz = Math.Abs(a.Z - b.Z);

        return new[] { dx, dy, dz }.Max();
    }

    private static int TaxicabDistance(Vector3D a, Vector3D b)
    {
        var dx = Math.Abs(a.X - b.X);
        var dy = Math.Abs(a.Y - b.Y);
        var dz = Math.Abs(a.Z - b.Z);

        return dx + dy + dz;
    }
}