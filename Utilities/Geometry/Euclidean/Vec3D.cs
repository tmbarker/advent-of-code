using Utilities.Extensions;

namespace Utilities.Geometry.Euclidean;

/// <summary>
///     A readonly integral 3D Vector value type.
/// </summary>
public readonly record struct Vec3D(int X, int Y, int Z)
{
    public static readonly Vec3D Zero    = new(X:  0, Y:  0, Z:  0);
    public static readonly Vec3D One     = new(X:  1, Y:  1, Z:  1);
    public static readonly Vec3D Up      = new(X:  0, Y:  1, Z:  0);
    public static readonly Vec3D Down    = new(X:  0, Y: -1, Z:  0);
    public static readonly Vec3D Left    = new(X: -1, Y:  0, Z:  0);
    public static readonly Vec3D Right   = new(X:  1, Y:  0, Z:  0);
    public static readonly Vec3D Forward = new(X:  0, Y:  0, Z:  1);
    public static readonly Vec3D Back    = new(X:  0, Y:  0, Z: -1);

    public int this[Axis component] => GetComponent(component);

    public Vec3D(Vec2D xy, int z) : this(xy.X, xy.Y, z)
    {
    }

    public static int Distance(Vec3D a, Vec3D b, Metric metric)
    {
        return metric switch
        {
            Metric.Chebyshev => ChebyshevDistance(a, b),
            Metric.Taxicab => TaxicabDistance(a, b),
            _ => throw VecThrowHelper<Vec3D>.InvalidMetric(metric)
        };
    }

    public static implicit operator Vec3D(Vec2D v) => new(v.X, v.Y, Z: 0);
    public static implicit operator Vec3D(Vec4D v) => new(v.X, v.Y, v.Z);
    
    public static Vec3D operator +(Vec3D lhs, Vec3D rhs)
    {
        return new Vec3D(X: lhs.X + rhs.X, Y: lhs.Y + rhs.Y, Z: lhs.Z + rhs.Z);
    }

    public static Vec3D operator -(Vec3D lhs, Vec3D rhs)
    {
        return new Vec3D(X: lhs.X - rhs.X, Y: lhs.Y - rhs.Y, Z: lhs.Z - rhs.Z);
    }

    public static Vec3D operator *(int k, Vec3D rhs)
    {
        return new Vec3D(X: k * rhs.X, Y: k * rhs.Y, Z: k * rhs.Z);
    }
    
    public override string ToString()
    {
        return $"<{X},{Y},{Z}>";
    }
    
    public int Magnitude(Metric metric)
    {
        return Distance(a: this, b: Zero, metric);
    }
    
    public ISet<Vec3D> GetAdjacentSet(Metric metric)
    {
        return metric switch
        {
            Metric.Chebyshev => GetChebyshevAdjacentSet(),
            Metric.Taxicab => GetTaxicabAdjacentSet(),
            _ => throw VecThrowHelper<Vec3D>.InvalidMetric(metric)
        };
    }
    
    private HashSet<Vec3D> GetTaxicabAdjacentSet()
    {
        return
        [
            this + Up,
            this + Down,
            this + Left,
            this + Right,
            this + Forward,
            this + Back
        ];
    }
    
    private HashSet<Vec3D> GetChebyshevAdjacentSet()
    {
        var set = new HashSet<Vec3D>();
        
        for (var dx = -1; dx <= 1; dx++)
        for (var dy = -1; dy <= 1; dy++)
        for (var dz = -1; dz <= 1; dz++)
        {
            set.Add(new Vec3D(
                X: X + dx,
                Y: Y + dy,
                Z: Z + dz));
        }

        set.Remove(item:this);
        return set;
    }
    
    private static int ChebyshevDistance(Vec3D a, Vec3D b)
    {
        var dx = Math.Abs(a.X - b.X);
        var dy = Math.Abs(a.Y - b.Y);
        var dz = Math.Abs(a.Z - b.Z);

        return Math.Max(dx, Math.Max(dy, dz));
    }

    private static int TaxicabDistance(Vec3D a, Vec3D b)
    {
        var dx = Math.Abs(a.X - b.X);
        var dy = Math.Abs(a.Y - b.Y);
        var dz = Math.Abs(a.Z - b.Z);

        return dx + dy + dz;
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
                return Z;
            case Axis.W:
            default:
                throw VecThrowHelper<Vec3D>.InvalidComponent(component);
        }
    }
    
    public static Vec3D Parse(string s)
    {
        var numbers = s.ParseInts();
        return new Vec3D(X: numbers[0], Y: numbers[1], Z: numbers[2]);
    }
    
    public static bool TryParse(string? s, out Vec3D result)
    {
        var numbers = s?.ParseInts() ?? [];
        if (numbers.Length < 3)
        {
            result = Zero;
            return false;
        }
    
        result = new Vec3D(X: numbers[0], Y: numbers[1], Z: numbers[2]);
        return true;
    }
}