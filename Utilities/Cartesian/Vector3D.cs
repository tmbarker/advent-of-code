namespace Utilities.Cartesian;

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
    
    public Vector3D(Vector2D xy, int z) : this(xy.X, xy.Y, z)
    {
    }

    public void Deconstruct(out int x, out int y, out int z)
    {
        x = X;
        y = Y;
        z = Z;
    }

    public int GetComponent(Axis component)
    {
        switch (component)
        {
            case Axis.X:
                return X;
            case Axis.Y:
                return Y;
            case Axis.Z:
                return Z;
            case Axis.W:
            default:
                throw new ArgumentOutOfRangeException(nameof(component), component, null);
        }
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
        return metric switch
        {
            DistanceMetric.Chebyshev => ChebyshevDistance(a, b),
            DistanceMetric.Taxicab => TaxicabDistance(a, b),
            _ => throw new ArgumentOutOfRangeException(nameof(metric), metric, null)
        };
    }

    public static implicit operator Vector3D(Vector2D v) => new(v.X, v.Y, 0);
    public static implicit operator Vector3D(Vector4D v) => new(v.X, v.Y, v.Z);
    
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

/// <summary>
/// Extension methods for <see cref="Vector3D"/> instances 
/// </summary>
public static class Vector3DExtensions
{
    /// <summary>
    /// Get the magnitude of the vector according to the specified distance metric
    /// </summary>
    public static int Magnitude(this Vector3D v, DistanceMetric metric)
    {
        return Vector3D.Distance(a: v, b: Vector3D.Zero, metric: metric);
    }
    
    /// <summary>
    /// Determine if two positions are adjacent, where adjacent means the specified distance metric is less than or equal to 1
    /// </summary>
    public static bool IsAdjacentTo(this Vector3D lhs, Vector3D rhs, DistanceMetric metric)
    {
        return Vector3D.Distance(lhs, rhs, metric) <= 1;
    }
    
    /// <summary>
    /// Get a set of vectors adjacent to <paramref name="vector"/>, depending on the <paramref name="metric"/> diagonally
    /// adjacent vectors may or may not be included in the returned set
    /// </summary>
    /// <exception cref="ArgumentException">This method does not support the Euclidean distance metric</exception>
    public static ISet<Vector3D> GetAdjacentSet(this Vector3D vector, DistanceMetric metric)
    {
        return metric switch
        {
            DistanceMetric.Chebyshev => GetChebyshevAdjacentSet(vector),
            DistanceMetric.Taxicab => GetTaxicabAdjacentSet(vector),
            _ => throw new ArgumentException(
                $"The {metric} distance metric is not well defined over integral vector space", nameof(metric))
        };
    }

    private static ISet<Vector3D> GetTaxicabAdjacentSet(Vector3D vector)
    {
        return new HashSet<Vector3D>
        {
            vector + Vector3D.Up,
            vector + Vector3D.Down, 
            vector + Vector3D.Left,
            vector + Vector3D.Right,
            vector + Vector3D.Forward,
            vector + Vector3D.Back,
        };
    }
    
    private static ISet<Vector3D> GetChebyshevAdjacentSet(Vector3D vector)
    {
        var set = new HashSet<Vector3D>();
        
        for (var x = -1; x <= 1; x++)
        for (var y = -1; y <= 1; y++)
        for (var z = -1; z <= 1; z++)
        {
            set.Add(new Vector3D(
                x: vector.X + x,
                y: vector.Y + y,
                z: vector.Z + z));
        }

        set.Remove(vector);
        return set;
    }
}