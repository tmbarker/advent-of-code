using Utilities.Extensions;

namespace Utilities.Geometry.Euclidean;

/// <summary>
/// A readonly integral 3D Vector value type
/// </summary>
public readonly record struct Vec3D
{
    public static readonly Vec3D Zero    = new(x:  0, y:  0, z:  0);
    public static readonly Vec3D One     = new(x:  1, y:  1, z:  1);
    public static readonly Vec3D Up      = new(x:  0, y:  1, z:  0);
    public static readonly Vec3D Down    = new(x:  0, y: -1, z:  0);
    public static readonly Vec3D Left    = new(x: -1, y:  0, z:  0);
    public static readonly Vec3D Right   = new(x:  1, y:  0, z:  0);
    public static readonly Vec3D Forward = new(x:  0, y:  0, z:  1);
    public static readonly Vec3D Back    = new(x:  0, y:  0, z: -1);
    
    public int X { get; }
    public int Y { get; }
    public int Z { get; }
    public int this[Axis axis] => GetComponent(axis);

    public Vec3D(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }
    
    public Vec3D(Vec2D xy, int z) : this(xy.X, xy.Y, z)
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
                throw VecThrowHelper<Vec3D>.InvalidComponent(component);
        }
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

    public static implicit operator Vec3D(Vec2D v) => new(v.X, v.Y, z: 0);
    public static implicit operator Vec3D(Vec4D v) => new(v.X, v.Y, v.Z);
    
    public static Vec3D operator +(Vec3D lhs, Vec3D rhs)
    {
        return new Vec3D(x: lhs.X + rhs.X, y: lhs.Y + rhs.Y, z: lhs.Z + rhs.Z);
    }

    public static Vec3D operator -(Vec3D lhs, Vec3D rhs)
    {
        return new Vec3D(x: lhs.X - rhs.X, y: lhs.Y - rhs.Y, z: lhs.Z - rhs.Z);
    }

    public static Vec3D operator *(int k, Vec3D rhs)
    {
        return new Vec3D(x: k * rhs.X, y: k * rhs.Y, z: k * rhs.Z);
    }
    
    public override string ToString()
    {
        return $"[{X},{Y},{Z}]";
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
                x: X + dx,
                y: Y + dy,
                z: Z + dz));
        }

        set.Remove(item:this);
        return set;
    }
    
    private static int ChebyshevDistance(Vec3D a, Vec3D b)
    {
        var dx = Math.Abs(a.X - b.X);
        var dy = Math.Abs(a.Y - b.Y);
        var dz = Math.Abs(a.Z - b.Z);

        return new[] { dx, dy, dz }.Max();
    }

    private static int TaxicabDistance(Vec3D a, Vec3D b)
    {
        var dx = Math.Abs(a.X - b.X);
        var dy = Math.Abs(a.Y - b.Y);
        var dz = Math.Abs(a.Z - b.Z);

        return dx + dy + dz;
    }

    public static Vec3D Parse(string s)
    {
        var numbers = s.ParseInts();
        return new Vec3D(x: numbers[0], y: numbers[1], z: numbers[2]);
    }
    
    public static bool TryParse(string? s, out Vec3D result)
    {
        var numbers = s?.ParseInts() ?? Array.Empty<int>();
        if (numbers.Length < 3)
        {
            result = Zero;
            return false;
        }
    
        result = new Vec3D(x: numbers[0], y: numbers[1], z: numbers[2]);
        return true;
    }
}