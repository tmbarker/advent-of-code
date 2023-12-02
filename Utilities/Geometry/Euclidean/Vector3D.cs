namespace Utilities.Geometry.Euclidean;

/// <summary>
/// A readonly integral 3D Vector value type
/// </summary>
public readonly struct Vector3D : IMetricSpaceVector<Vector3D>, IEquatable<Vector3D>
{
    private const string StringFormat = "[{0},{1},{2}]";

    public static readonly Vector3D Zero    = new(x:  0, y:  0, z:  0);
    public static readonly Vector3D Up      = new(x:  0, y:  1, z:  0);
    public static readonly Vector3D Down    = new(x:  0, y: -1, z:  0);
    public static readonly Vector3D Left    = new(x: -1, y:  0, z:  0);
    public static readonly Vector3D Right   = new(x:  1, y:  0, z:  0);
    public static readonly Vector3D Forward = new(x:  0, y:  0, z:  1);
    public static readonly Vector3D Back    = new(x:  0, y:  0, z: -1);

    private string Id { get; }
    public int X { get; }
    public int Y { get; }
    public int Z { get; }
    public int this[Axis axis] => GetComponent(axis);
    
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

    public static int Distance(Vector3D a, Vector3D b, Metric metric)
    {
        return metric switch
        {
            Metric.Chebyshev => ChebyshevDistance(a, b),
            Metric.Taxicab => TaxicabDistance(a, b),
            _ => throw new ArgumentOutOfRangeException(nameof(metric), metric, null)
        };
    }

    public static implicit operator Vector3D(Vector2D v) => new(v.X, v.Y, z: 0);
    public static implicit operator Vector3D(Vector4D v) => new(v.X, v.Y, v.Z);
    
    public static Vector3D operator +(Vector3D lhs, Vector3D rhs)
    {
        return new Vector3D(x: lhs.X + rhs.X, y: lhs.Y + rhs.Y, z: lhs.Z + rhs.Z);
    }

    public static Vector3D operator -(Vector3D lhs, Vector3D rhs)
    {
        return new Vector3D(x: lhs.X - rhs.X, y: lhs.Y - rhs.Y, z: lhs.Z - rhs.Z);
    }

    public static Vector3D operator *(int k, Vector3D rhs)
    {
        return new Vector3D(x: k * rhs.X, y: k * rhs.Y, z: k * rhs.Z);
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
    
    public int Magnitude(Metric metric)
    {
        return Distance(a: this, b: Zero, metric);
    }
    
    public ISet<Vector3D> GetAdjacentSet(Metric metric)
    {
        return metric switch
        {
            Metric.Chebyshev => GetChebyshevAdjacentSet(),
            Metric.Taxicab => GetTaxicabAdjacentSet(),
            _ => throw new ArgumentException(
                $"The {metric} distance metric is not well defined over {nameof(Vector3D)} space", nameof(metric))
        };
    }
    
    private ISet<Vector3D> GetTaxicabAdjacentSet()
    {
        return new HashSet<Vector3D>
        {
            this + Up,
            this + Down, 
            this + Left,
            this + Right,
            this + Forward,
            this + Back
        };
    }
    
    private ISet<Vector3D> GetChebyshevAdjacentSet()
    {
        var set = new HashSet<Vector3D>();
        
        for (var x = -1; x <= 1; x++)
        for (var y = -1; y <= 1; y++)
        for (var z = -1; z <= 1; z++)
        {
            set.Add(new Vector3D(
                x: X + x,
                y: Y + y,
                z: Z + z));
        }

        set.Remove(item:this);
        return set;
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