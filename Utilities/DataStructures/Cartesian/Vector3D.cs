namespace Utilities.DataStructures.Cartesian;

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

    public string Id { get; }
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

    public Vector3D(Vector3D other) : this(other.X, other.Y, other.Z)
    {
    }

    public static Vector3D Normalize(Vector3D vector)
    {
        return new Vector3D(Math.Sign(vector.X), Math.Sign(vector.Y), Math.Sign(vector.Z));
    }

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
}