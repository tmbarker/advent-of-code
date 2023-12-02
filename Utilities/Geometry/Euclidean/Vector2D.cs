namespace Utilities.Geometry.Euclidean;

/// <summary>
/// A readonly integral 2D Vector value type
/// </summary>
public readonly struct Vector2D : IMetricSpaceVector<Vector2D>, IEquatable<Vector2D>
{
    private const string StringFormat = "[{0},{1}]";

    public static readonly Vector2D Zero  = new(x:  0, y:  0);
    public static readonly Vector2D Up    = new(x:  0, y:  1);
    public static readonly Vector2D Down  = new(x:  0, y: -1);
    public static readonly Vector2D Left  = new(x: -1, y:  0);
    public static readonly Vector2D Right = new(x:  1, y:  0);
    public static readonly Vector2D PositiveInfinity = new (x: int.MaxValue, y:int.MaxValue);

    private string Id { get; }
    public int X { get; }
    public int Y { get; }
    public int this[Axis axis] => GetComponent(axis);
    
    public Vector2D(int x, int y)
    {
        X = x;
        Y = y;
        
        Id = string.Format(StringFormat, X, Y);
    }

    public void Deconstruct(out int x, out int y)
    {
        x = X;
        y = Y;
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
            case Axis.W:
            default:
                throw new ArgumentOutOfRangeException(nameof(component), component, null);
        }
    }
    
    public static Vector2D Normalize(Vector2D vector)
    {
        return new Vector2D(x: Math.Sign(vector.X), y: Math.Sign(vector.Y));
    }

    public static int Distance(Vector2D a, Vector2D b, Metric metric)
    {
        return metric switch
        {
            Metric.Chebyshev => ChebyshevDistance(a, b),
            Metric.Taxicab => TaxicabDistance(a, b),
            _ => throw new ArgumentOutOfRangeException(nameof(metric), metric, null)
        };
    }

    public static Vector2D MinCollinear(Vector2D vector)
    {
        var maxDivisor = ChebyshevDistance(a: Zero, b: vector);
        for (var k = maxDivisor; k > 1; k--)
        {
            var candidate = vector / k;
            if (k * candidate == vector)
            {
                return candidate;
            }
        }

        return vector;
    }

    public static double AngleBetweenDeg(Vector2D from, Vector2D to)
    {
        var sin = to.X * from.Y - from.X * to.Y;
        var cos = to.X * from.X + from.Y * to.Y;

        return Math.Atan2(sin, cos) * (180 / Math.PI);
    }
    
    public static implicit operator Vector2D(Vector3D v) => new (v.X, v.Y);
    
    public static Vector2D operator +(Vector2D lhs, Vector2D rhs)
    {
        return new Vector2D(x: lhs.X + rhs.X, y: lhs.Y + rhs.Y);
    }

    public static Vector2D operator -(Vector2D lhs, Vector2D rhs)
    {
        return new Vector2D(x: lhs.X - rhs.X, y: lhs.Y - rhs.Y);
    }

    public static Vector2D operator *(int k, Vector2D rhs)
    {
        return new Vector2D(x: k * rhs.X, y: k * rhs.Y);
    }
    
    public static Vector2D operator /(Vector2D lhs, int k)
    {
        return new Vector2D(x:lhs.X / k, y:lhs.Y / k);
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
    
    public int Magnitude(Metric metric)
    {
        return Distance(a: this, b: Zero, metric);
    }
    
    public ISet<Vector2D> GetAdjacentSet(Metric metric)
    {
        return metric switch
        {
            Metric.Chebyshev => GetChebyshevAdjacentSet(),
            Metric.Taxicab => GetTaxicabAdjacentSet(),
            _ => throw new ArgumentException(
                $"The {metric} distance metric is not well defined over {nameof(Vector2D)} space", nameof(metric))
        };
    }
    
    private ISet<Vector2D> GetTaxicabAdjacentSet()
    {
        return new HashSet<Vector2D>
        {
            this + Up,
            this + Down, 
            this + Left,
            this + Right
        };
    }
    
    private ISet<Vector2D> GetChebyshevAdjacentSet()
    {
        var set = new HashSet<Vector2D>();
        
        for (var x = -1; x <= 1; x++)
        for (var y = -1; y <= 1; y++)
        {
            set.Add(new Vector2D(
                x: X + x,
                y: Y + y));
        }

        set.Remove(item:this);
        return set;
    }
    
    private static int ChebyshevDistance(Vector2D a, Vector2D b)
    {
        var dx = Math.Abs(a.X - b.X);
        var dy = Math.Abs(a.Y - b.Y);

        return Math.Max(dx, dy);
    }
    
    private static int TaxicabDistance(Vector2D a, Vector2D b)
    {
        var dx = Math.Abs(a.X - b.X);
        var dy = Math.Abs(a.Y - b.Y);

        return dx + dy;
    }
}

/// <summary>
/// Extension methods for <see cref="Vector2D"/> instances 
/// </summary>
public static class Vector2DExtensions
{
    /// <summary>
    /// Determine if two positions are diagonal, where diagonal means that they do not share a common value for
    /// either component
    /// </summary>
    public static bool IsDiagonalTo(this Vector2D lhs, Vector2D rhs)
    {
        return lhs.X != rhs.X && lhs.Y != rhs.Y;
    }
    
    /// <summary>
    /// Determine if two positions are adjacent, where adjacent means the specified distance metric is less than or equal to 1
    /// </summary>
    public static bool IsAdjacentTo(this Vector2D lhs, Vector2D rhs, Metric metric)
    {
        return Vector2D.Distance(a: lhs, b: rhs, metric) <= 1;
    }
}