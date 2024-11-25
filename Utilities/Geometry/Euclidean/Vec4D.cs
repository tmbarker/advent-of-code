using Utilities.Extensions;

namespace Utilities.Geometry.Euclidean;

/// <summary>
///     A readonly integral 4D Vector value type.
/// </summary>
public readonly record struct Vec4D(int X, int Y, int Z, int W)
{
    private static readonly Vec4D Zero = new(X:0, Y:0, Z:0, W:0);

    public int this[Axis component] => GetComponent(component);

    public static int Distance(Vec4D a, Vec4D b, Metric metric)
    {
        return metric switch
        {
            Metric.Chebyshev => ChebyshevDistance(a, b),
            Metric.Taxicab => TaxicabDistance(a, b),
            _ => throw VecThrowHelper<Vec4D>.InvalidMetric(metric)
        };
    }

    public static implicit operator Vec4D(Vec3D v) => new(v.X, v.Y, v.Z, W: 0);
    
    public static Vec4D operator +(Vec4D lhs, Vec4D rhs)
    {
        return new Vec4D(X: lhs.X + rhs.X, Y: lhs.Y + rhs.Y, Z: lhs.Z + rhs.Z, W: lhs.W + rhs.W);
    }

    public static Vec4D operator -(Vec4D lhs, Vec4D rhs)
    {
        return new Vec4D(X: lhs.X - rhs.X, Y: lhs.Y - rhs.Y, Z: lhs.Z - rhs.Z, W: lhs.W - rhs.W);
    }

    public static Vec4D operator *(int k, Vec4D rhs)
    {
        return new Vec4D(X: k * rhs.X, Y: k * rhs.Y, Z: k * rhs.Z, W: k * rhs.W);
    }
    
    public override string ToString()
    {
        return $"<{X},{Y},{Z},{W}>";
    }
    
    private static int ChebyshevDistance(Vec4D a, Vec4D b)
    {
        var dx = Math.Abs(a.X - b.X);
        var dy = Math.Abs(a.Y - b.Y);
        var dz = Math.Abs(a.Z - b.Z);
        var dw = Math.Abs(a.W - b.W);

        return Math.Max(dx, Math.Max(dy, Math.Max(dz, dw)));
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
            this + new Vec4D(X:  1, Y:  0, Z:  0, W:  0),
            this + new Vec4D(X: -1, Y:  0, Z:  0, W:  0),
            this + new Vec4D(X:  0, Y:  1, Z:  0, W:  0),
            this + new Vec4D(X:  0, Y: -1, Z:  0, W:  0),
            this + new Vec4D(X:  0, Y:  0, Z:  1, W:  0),
            this + new Vec4D(X:  0, Y:  0, Z: -1, W:  0),
            this + new Vec4D(X:  0, Y:  0, Z:  0, W:  1),
            this + new Vec4D(X:  0, Y:  0, Z:  0, W: -1)
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
                X: X + dx,
                Y: Y + dy,
                Z: Z + dz,
                W: W + dw));
        }

        set.Remove(item: this);
        return set;
    }
    
    private int GetComponent(Axis component)
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
    
    public static Vec4D Parse(string s)
    {
        var numbers = s.ParseInts();
        return new Vec4D(X: numbers[0], Y: numbers[1], Z: numbers[2], W: numbers[3]);
    }
    
    public static bool TryParse(string? s, out Vec4D result)
    {
        var numbers = s?.ParseInts() ?? [];
        if (numbers.Length < 4)
        {
            result = Zero;
            return false;
        }
    
        result = new Vec4D(X: numbers[0], Y: numbers[1], Z: numbers[2], W: numbers[3]);
        return true;
    }
}