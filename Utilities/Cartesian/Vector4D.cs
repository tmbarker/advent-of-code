namespace Utilities.Cartesian;

/// <summary>
/// A readonly integral 4D Vector value type
/// </summary>
public readonly struct Vector4D : IEquatable<Vector4D>
{
    private const string StringFormat = "[{0},{1},{2},{3}]";

    public static readonly Vector4D Zero = new(x:0, y:0, z:0, w:0);

    private string Id { get; }
    public int X { get; }
    public int Y { get; }
    public int Z { get; }
    public int W { get; }
    
    public Vector4D(int x, int y, int z, int w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;

        Id = string.Format(StringFormat, X, Y, Z, W);
    }

    public void Deconstruct(out int x, out int y, out int z, out int w)
    {
        x = X;
        y = Y;
        z = Z;
        w = W;
    }

    public int GetComponent(Axis component)
    {
        return component switch
        {
            Axis.X => X,
            Axis.Y => Y,
            Axis.Z => Z,
            Axis.W => W,
            _ => throw new ArgumentOutOfRangeException(nameof(component), component, null)
        };
    }
    
    public static int Distance(Vector4D a, Vector4D b, Metric metric)
    {
        return metric switch
        {
            Metric.Chebyshev => ChebyshevDistance(a, b),
            Metric.Taxicab => TaxicabDistance(a, b),
            _ => throw new ArgumentOutOfRangeException(nameof(metric), metric, null)
        };
    }

    public static implicit operator Vector4D(Vector3D v) => new(v.X, v.Y, v.Z, 0);
    
    public static Vector4D operator +(Vector4D lhs, Vector4D rhs)
    {
        return new Vector4D(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z, lhs.W + rhs.W);
    }

    public static Vector4D operator -(Vector4D lhs, Vector4D rhs)
    {
        return new Vector4D(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z, lhs.W - rhs.W);
    }

    public static Vector4D operator *(int k, Vector4D rhs)
    {
        return new Vector4D(k * rhs.X, k * rhs.Y, k * rhs.Z, k * rhs.W);
    }

    public static bool operator ==(Vector4D left, Vector4D right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Vector4D left, Vector4D right)
    {
        return !(left == right);
    }
    
    public bool Equals(Vector4D other)
    {
        return X == other.X && Y == other.Y && Z == other.Z && W == other.W;
    }

    public override bool Equals(object? obj)
    {
        return obj is Vector4D other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z, W);
    }
    
    public override string ToString()
    {
        return Id;
    }
    
    private static int ChebyshevDistance(Vector4D a, Vector4D b)
    {
        var dx = Math.Abs(a.X - b.X);
        var dy = Math.Abs(a.Y - b.Y);
        var dz = Math.Abs(a.Z - b.Z);
        var dw = Math.Abs(a.W - b.W);

        return new[] { dx, dy, dz, dw }.Max();
    }

    private static int TaxicabDistance(Vector4D a, Vector4D b)
    {
        var dx = Math.Abs(a.X - b.X);
        var dy = Math.Abs(a.Y - b.Y);
        var dz = Math.Abs(a.Z - b.Z);
        var dw = Math.Abs(a.W - b.W);

        return dx + dy + dz + dw;
    }
}

public static class Vector4DExtensions
{
    /// <summary>
    /// Get a set of vectors adjacent to <paramref name="vector"/>, depending on the <paramref name="metric"/> diagonally
    /// adjacent vectors may or may not be included in the returned set
    /// </summary>
    /// <exception cref="ArgumentException">This method does not support the Euclidean distance metric</exception>
    public static ISet<Vector4D> GetAdjacentSet(this Vector4D vector, Metric metric)
    {
        return metric switch
        {
            Metric.Chebyshev => GetChebyshevAdjacentSet(vector),
            Metric.Taxicab => GetTaxicabAdjacentSet(vector),
            _ => throw new ArgumentException(
                $"The {metric} distance metric is not well defined over integral vector space", nameof(metric))
        };
    }

    private static ISet<Vector4D> GetTaxicabAdjacentSet(Vector4D vector)
    {
        return new HashSet<Vector4D>
        {
            vector + new Vector4D(x:  1, y:  0, z:  0, w:  0),
            vector + new Vector4D(x: -1, y:  0, z:  0, w:  0),
            vector + new Vector4D(x:  0, y:  1, z:  0, w:  0),
            vector + new Vector4D(x:  0, y: -1, z:  0, w:  0),
            vector + new Vector4D(x:  0, y:  0, z:  1, w:  0),
            vector + new Vector4D(x:  0, y:  0, z: -1, w:  0),
            vector + new Vector4D(x:  0, y:  0, z:  0, w:  1),
            vector + new Vector4D(x:  0, y:  0, z:  0, w: -1)
        };
    }
    
    private static ISet<Vector4D> GetChebyshevAdjacentSet(Vector4D vector)
    {
        var set = new HashSet<Vector4D>();
        
        for (var x = -1; x <= 1; x++)
        for (var y = -1; y <= 1; y++)
        for (var z = -1; z <= 1; z++)
        for (var w = -1; w <= 1; w++)
        {
            set.Add(new Vector4D(
                x: vector.X + x,
                y: vector.Y + y,
                z: vector.Z + z,
                w: vector.W + w));
        }

        set.Remove(vector);
        return set;
    }
}