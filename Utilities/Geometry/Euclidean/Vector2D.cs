using Utilities.Extensions;

namespace Utilities.Geometry.Euclidean;

/// <summary>
/// A readonly integral 2D Vector value type
/// </summary>
public readonly record struct Vector2D
{
    public static readonly Vector2D Zero  = new(x:  0, y:  0);
    public static readonly Vector2D One   = new(x:  1, y:  1);
    public static readonly Vector2D Up    = new(x:  0, y:  1);
    public static readonly Vector2D Down  = new(x:  0, y: -1);
    public static readonly Vector2D Left  = new(x: -1, y:  0);
    public static readonly Vector2D Right = new(x:  1, y:  0);
    public static readonly Vector2D PositiveInfinity = new (x: int.MaxValue, y:int.MaxValue);
    
    public int X { get; }
    public int Y { get; }
    public int this[Axis axis] => GetComponent(axis);

    public Vector2D(int x, int y)
    {
        X = x;
        Y = y;
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
    
    public override string ToString()
    {
        return $"[{X},{Y}]";
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
    
    private HashSet<Vector2D> GetTaxicabAdjacentSet()
    {
        return
        [
            this + Up,
            this + Down,
            this + Left,
            this + Right
        ];
    }
    
    private HashSet<Vector2D> GetChebyshevAdjacentSet()
    {
        var set = new HashSet<Vector2D>();
        
        for (var dx = -1; dx <= 1; dx++)
        for (var dy = -1; dy <= 1; dy++)
        {
            set.Add(new Vector2D(
                x: X + dx,
                y: Y + dy));
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