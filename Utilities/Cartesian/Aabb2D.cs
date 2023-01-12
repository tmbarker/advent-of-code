namespace Utilities.Cartesian;

/// <summary>
/// An axis aligned 2D Rect value type
/// </summary>
public readonly struct Aabb2D
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
    
    public long GetArea()
    {
        return (long)Width * Height;
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
}