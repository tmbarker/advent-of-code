using Utilities.Extensions;

namespace Utilities.Geometry.Euclidean;

/// <summary>
///     A readonly integral 2D Vector value type.
/// </summary>
public readonly record struct Vec2D(int X, int Y)
{
    public static readonly Vec2D Zero  = new(X:  0, Y:  0);
    public static readonly Vec2D One   = new(X:  1, Y:  1);
    public static readonly Vec2D Up    = new(X:  0, Y:  1);
    public static readonly Vec2D Down  = new(X:  0, Y: -1);
    public static readonly Vec2D Left  = new(X: -1, Y:  0);
    public static readonly Vec2D Right = new(X:  1, Y:  0);
    public static readonly Vec2D PositiveInfinity = new (X: int.MaxValue, Y:int.MaxValue);

    public int this[Axis component] => GetComponent(component);

    public static Vec2D Normalize(Vec2D vec)
    {
        return new Vec2D(X: Math.Sign(vec.X), Y: Math.Sign(vec.Y));
    }

    public static int Distance(Vec2D a, Vec2D b, Metric metric)
    {
        return metric switch
        {
            Metric.Chebyshev => ChebyshevDistance(a, b),
            Metric.Taxicab => TaxicabDistance(a, b),
            _ => throw VecThrowHelper<Vec2D>.InvalidMetric(metric)
        };
    }

    public static bool IsAdjacent(Vec2D a, Vec2D b, Metric metric)
    {
        return Distance(a, b, metric) <= 1;
    }
    
    public static Vec2D MinCollinear(Vec2D vec)
    {
        var maxDivisor = ChebyshevDistance(a: Zero, b: vec);
        for (var k = maxDivisor; k > 1; k--)
        {
            var candidate = vec / k;
            if (k * candidate == vec)
            {
                return candidate;
            }
        }

        return vec;
    }

    public static double AngleBetweenDeg(Vec2D from, Vec2D to)
    {
        var sin = to.X * from.Y - from.X * to.Y;
        var cos = to.X * from.X + from.Y * to.Y;

        return Math.Atan2(sin, cos) * (180 / Math.PI);
    }
    
    public static implicit operator Vec2D(Vec3D v) => new (v.X, v.Y);
    
    public static Vec2D operator +(Vec2D lhs, Vec2D rhs)
    {
        return new Vec2D(X: lhs.X + rhs.X, Y: lhs.Y + rhs.Y);
    }

    public static Vec2D operator -(Vec2D lhs, Vec2D rhs)
    {
        return new Vec2D(X: lhs.X - rhs.X, Y: lhs.Y - rhs.Y);
    }

    public static Vec2D operator *(int k, Vec2D rhs)
    {
        return new Vec2D(X: k * rhs.X, Y: k * rhs.Y);
    }
    
    public static Vec2D operator /(Vec2D lhs, int k)
    {
        return new Vec2D(X:lhs.X / k, Y:lhs.Y / k);
    }
    
    public override string ToString()
    {
        return $"[{X},{Y}]";
    }
    
    public int Magnitude(Metric metric)
    {
        return Distance(a: this, b: Zero, metric);
    }
    
    public ISet<Vec2D> GetAdjacentSet(Metric metric)
    {
        return metric switch
        {
            Metric.Chebyshev => GetChebyshevAdjacentSet(),
            Metric.Taxicab => GetTaxicabAdjacentSet(),
            _ => throw VecThrowHelper<Vec2D>.InvalidMetric(metric)
        };
    }
    
    private HashSet<Vec2D> GetTaxicabAdjacentSet()
    {
        return
        [
            this + Up,
            this + Down,
            this + Left,
            this + Right
        ];
    }
    
    private HashSet<Vec2D> GetChebyshevAdjacentSet()
    {
        var set = new HashSet<Vec2D>();
        
        for (var dx = -1; dx <= 1; dx++)
        for (var dy = -1; dy <= 1; dy++)
        {
            set.Add(new Vec2D(
                X: X + dx,
                Y: Y + dy));
        }

        set.Remove(item:this);
        return set;
    }
    
    private static int ChebyshevDistance(Vec2D a, Vec2D b)
    {
        var dx = Math.Abs(a.X - b.X);
        var dy = Math.Abs(a.Y - b.Y);

        return Math.Max(dx, dy);
    }
    
    private static int TaxicabDistance(Vec2D a, Vec2D b)
    {
        var dx = Math.Abs(a.X - b.X);
        var dy = Math.Abs(a.Y - b.Y);

        return dx + dy;
    }
    
    private int GetComponent(Axis component)
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
                throw VecThrowHelper<Vec2D>.InvalidComponent(component);
        }
    }
    
    public static Vec2D Parse(string s)
    {
        var numbers = s.ParseInts();
        return new Vec2D(X: numbers[0], Y: numbers[1]);
    }
    
    public static bool TryParse(string? s, out Vec2D result)
    {
        var numbers = s?.ParseInts() ?? [];
        if (numbers.Length < 2)
        {
            result = Zero;
            return false;
        }
    
        result = new Vec2D(X: numbers[0], Y: numbers[1]);
        return true;
    }
}