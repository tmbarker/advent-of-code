using System.Collections;

namespace Utilities.Cartesian;

/// <summary>
/// A readonly axis aligned 2D Rect value type
/// </summary>
public readonly struct Aabb2D : IEnumerable<Vector2D>, IEquatable<Aabb2D>
{
    public Aabb2D(ICollection<Vector2D> extents, bool inclusive)
    {
        var delta = inclusive ? 0 : 1;
        XMin = extents.Min(p => p.X) - delta;
        XMax = extents.Max(p => p.X) + delta;
        YMin = extents.Min(p => p.Y) - delta;
        YMax = extents.Max(p => p.Y) + delta;
    }
    
    public Aabb2D(int xMin, int xMax, int yMin, int yMax)
    {
        XMin = xMin;
        XMax = xMax;
        YMin = yMin;
        YMax = yMax;
    }
    
    public int XMin { get; }
    public int XMax { get; }
    public int YMin { get; }
    public int YMax { get; }

    public int Width => XMax - XMin + 1;
    public int Height => YMax - YMin + 1;
    public long Area => (long)Width * Height;

    public static bool FindOverlap(Aabb2D lhs, Aabb2D rhs, out  Aabb2D overlap)
    {
        var hasOverlap =
            lhs.XMax >= rhs.XMin && lhs.XMin <= rhs.XMax &&
            lhs.YMax >= rhs.YMin && lhs.YMin <= rhs.YMax;

        if (!hasOverlap)
        {
            overlap = default;
            return false;
        }

        var xLimits = new[] { lhs.XMin, lhs.XMax, rhs.XMin, rhs.XMax }.Order().ToList();
        var yLimits = new[] { lhs.YMin, lhs.YMax, rhs.YMin, rhs.YMax }.Order().ToList();

        overlap = new Aabb2D(
            xMin: xLimits[1],
            xMax: xLimits[2],
            yMin: yLimits[1],
            yMax: yLimits[2]);
        return true;
    }
    
    public bool Contains(Vector2D pos, bool inclusive)
    {
        return inclusive 
            ? ContainsInclusive(pos) 
            : ContainsExclusive(pos);
    }
    
    private bool ContainsInclusive(Vector2D pos)
    {
        return 
            pos.X >= XMin && pos.X <= XMax && 
            pos.Y >= YMin && pos.Y <= YMax;
    }
    
    private bool ContainsExclusive(Vector2D pos)
    {
        return 
            pos.X > XMin && pos.X < XMax && 
            pos.Y > YMin && pos.Y < YMax;
    }

    public bool Intersects(Aabb2D other)
    {
        return
            XMin <= other.XMax && other.XMin <= XMax &&
            YMin <= other.YMax && other.YMin <= YMax;
    }

    public static Aabb2D operator +(Aabb2D lhs, int amount)
    {
        return amount switch
        {
            0 => lhs,
            > 0 => lhs.Expand(amount),
            < 0 => lhs.Contract(-amount)
        };
    }
    
    public static Aabb2D operator -(Aabb2D lhs, int rhs)
    {
        return rhs switch
        {
            0 => lhs,
            < 0 => lhs.Expand(-rhs),
            > 0 => lhs.Contract(rhs)
        };
    }
    
    private Aabb2D Expand(int amount)
    {
        return new Aabb2D(xMin: XMin - amount, xMax: XMax + amount, yMin: YMin - amount, yMax: YMax + amount);
    }

    private Aabb2D Contract(int amount)
    {
        var width = XMax - XMin;
        var height = YMax - YMin;
        
        if (2 * amount >= width)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), amount,
                $"Contraction amount [{amount}] must be less than half of width [{width}]");
        }
        
        if (2 * amount >= height)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), amount,
                $"Contraction amount [{amount}] must be less than half of height [{height}]");
        }
        
        return new Aabb2D(xMin: XMin + amount, xMax: XMax - amount, yMin: YMin + amount, yMax: YMax - amount);
    }
    
    public override string ToString()
    {
        return $"[X={XMin}..{XMax}, Y={YMin}..{YMax}]";
    }

    public IEnumerator<Vector2D> GetEnumerator()
    {
        for (var x = XMin; x <= XMax; x++)
        for (var y = YMin; y <= YMax; y++)
        {
            yield return new Vector2D(x, y);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public bool Equals(Aabb2D other)
    {
        return XMin == other.XMin && XMax == other.XMax && YMin == other.YMin && YMax == other.YMax;
    }

    public override bool Equals(object? obj)
    {
        return obj is Aabb2D other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(XMin, XMax, YMin, YMax);
    }

    public static bool operator ==(Aabb2D left, Aabb2D right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Aabb2D left, Aabb2D right)
    {
        return !left.Equals(right);
    }
}