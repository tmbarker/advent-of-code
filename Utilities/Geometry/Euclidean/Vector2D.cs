using Utilities.Extensions;

namespace Utilities.Geometry.Euclidean;

/// <summary>
/// A readonly integral 2D Vector value type
/// </summary>
public readonly struct Vector2D : IEquatable<Vector2D>
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
                throw VectorThrowHelper<Vector2D>.InvalidComponent(component);
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
            _ => throw VectorThrowHelper<Vector2D>.InvalidMetric(metric)
        };
    }

    public static bool IsAdjacent(Vector2D a, Vector2D b, Metric metric)
    {
        return Distance(a, b, metric) <= 1;
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
            _ => throw VectorThrowHelper<Vector2D>.InvalidMetric(metric)
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

    public static Vector2D Parse(string s)
    {
        var numbers = s.ParseInts();
        return new Vector2D(x: numbers[0], y: numbers[1]);
    }
    
    public static bool TryParse(string? s, out Vector2D result)
    {
        var numbers = s?.ParseInts() ?? Array.Empty<int>();
        if (numbers.Length < 2)
        {
            result = Zero;
            return false;
        }
    
        result = new Vector2D(x: numbers[0], y: numbers[1]);
        return true;
    }
}