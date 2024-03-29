using Utilities.Extensions;

namespace Utilities.Geometry.Euclidean;

/// <summary>
/// A readonly integral 4D Vector value type
/// </summary>
public readonly record struct Vec4D
{
    private static readonly Vec4D Zero = new(x:0, y:0, z:0, w:0);
    
    public int X { get; }
    public int Y { get; }
    public int Z { get; }
    public int W { get; }
    public int this[Axis axis] => GetComponent(axis);

    public Vec4D(int x, int y, int z, int w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }
    
    public int GetComponent(Axis component)
    {
        return component switch
        {
            Axis.X => X,
            Axis.Y => Y,
            Axis.Z => Z,
            Axis.W => W,
            _ => throw VecThrowHelper<Vec4D>.InvalidComponent(component)
        };
    }
    
    public static int Distance(Vec4D a, Vec4D b, Metric metric)
    {
        return metric switch
        {
            Metric.Chebyshev => ChebyshevDistance(a, b),
            Metric.Taxicab => TaxicabDistance(a, b),
            _ => throw VecThrowHelper<Vec4D>.InvalidMetric(metric)
        };
    }

    public static implicit operator Vec4D(Vec3D v) => new(v.X, v.Y, v.Z, w: 0);
    
    public static Vec4D operator +(Vec4D lhs, Vec4D rhs)
    {
        return new Vec4D(x: lhs.X + rhs.X, y: lhs.Y + rhs.Y, z: lhs.Z + rhs.Z, w: lhs.W + rhs.W);
    }

    public static Vec4D operator -(Vec4D lhs, Vec4D rhs)
    {
        return new Vec4D(x: lhs.X - rhs.X, y: lhs.Y - rhs.Y, z: lhs.Z - rhs.Z, w: lhs.W - rhs.W);
    }

    public static Vec4D operator *(int k, Vec4D rhs)
    {
        return new Vec4D(x: k * rhs.X, y: k * rhs.Y, z: k * rhs.Z, w: k * rhs.W);
    }
    
    public override string ToString()
    {
        return $"[{X},{Y},{Z},{W}]";
    }
    
    private static int ChebyshevDistance(Vec4D a, Vec4D b)
    {
        var dx = Math.Abs(a.X - b.X);
        var dy = Math.Abs(a.Y - b.Y);
        var dz = Math.Abs(a.Z - b.Z);
        var dw = Math.Abs(a.W - b.W);

        return new[] { dx, dy, dz, dw }.Max();
    }

    private static int TaxicabDistance(Vec4D a, Vec4D b)
    {
        var dx = Math.Abs(a.X - b.X);
        var dy = Math.Abs(a.Y - b.Y);
        var dz = Math.Abs(a.Z - b.Z);
        var dw = Math.Abs(a.W - b.W);

        return dx + dy + dz + dw;
    }
    
    public int Magnitude(Metric metric)
    {
        return Distance(a: this, b: Zero, metric);
    }
    
    public ISet<Vec4D> GetAdjacentSet(Metric metric)
    {
        return metric switch
        {
            Metric.Chebyshev => GetChebyshevAdjacentSet(),
            Metric.Taxicab => GetTaxicabAdjacentSet(),
            _ => throw VecThrowHelper<Vec4D>.InvalidMetric(metric)
        };
    }
    
    private HashSet<Vec4D> GetTaxicabAdjacentSet()
    {
        return
        [
            this + new Vec4D(x:  1, y:  0, z:  0, w:  0),
            this + new Vec4D(x: -1, y:  0, z:  0, w:  0),
            this + new Vec4D(x:  0, y:  1, z:  0, w:  0),
            this + new Vec4D(x:  0, y: -1, z:  0, w:  0),
            this + new Vec4D(x:  0, y:  0, z:  1, w:  0),
            this + new Vec4D(x:  0, y:  0, z: -1, w:  0),
            this + new Vec4D(x:  0, y:  0, z:  0, w:  1),
            this + new Vec4D(x:  0, y:  0, z:  0, w: -1)
        ];
    }
    
    private HashSet<Vec4D> GetChebyshevAdjacentSet()
    {
        var set = new HashSet<Vec4D>();
        
        for (var dx = -1; dx <= 1; dx++)
        for (var dy = -1; dy <= 1; dy++)
        for (var dz = -1; dz <= 1; dz++)
        for (var dw = -1; dw <= 1; dw++)
        {
            set.Add(new Vec4D(
                x: X + dx,
                y: Y + dy,
                z: Z + dz,
                w: W + dw));
        }

        set.Remove(item: this);
        return set;
    }

    public static Vec4D Parse(string s)
    {
        var numbers = s.ParseInts();
        return new Vec4D(x: numbers[0], y: numbers[1], z: numbers[2], w: numbers[3]);
    }
    
    public static bool TryParse(string? s, out Vec4D result)
    {
        var numbers = s?.ParseInts() ?? Array.Empty<int>();
        if (numbers.Length < 4)
        {
            result = Zero;
            return false;
        }
    
        result = new Vec4D(x: numbers[0], y: numbers[1], z: numbers[2], w: numbers[3]);
        return true;
    }
}