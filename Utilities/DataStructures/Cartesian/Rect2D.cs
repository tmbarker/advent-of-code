namespace Utilities.DataStructures.Cartesian;

public readonly struct Rect2D
{
    public Rect2D(ICollection<Vector2D> contained, bool inclusive)
    {
        var add = inclusive ? 0 : 1;
        XMin = contained.Min(p => p.X) + add;
        XMax = contained.Max(p => p.X) + add;
        YMin = contained.Min(p => p.Y) + add;
        YMax = contained.Max(p => p.Y) + add;
    }
    
    public Rect2D(int xMin, int xMax, int yMin, int yMax)
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

    public bool ContainsInclusive(Vector2D pos)
    {
        return pos.X >= XMin && pos.X <= XMax && pos.Y >= YMin && pos.Y <= YMax;
    }
    
    public bool ContainsExclusive(Vector2D pos)
    {
        return pos.X > XMin && pos.X < XMax && pos.Y > YMin && pos.Y < YMax;
    }

    public override string ToString()
    {
        return $"[X={XMin}..{XMax}, Y={YMin}..{YMax}]";
    }

    public static Rect2D operator +(Rect2D r, int amount)
    {
        return amount switch
        {
            0 => r,
            > 0 => r.Expand(amount),
            < 0 => r.Contract(-amount)
        };
    }
    
    public static Rect2D operator -(Rect2D lhs, int rhs)
    {
        return rhs switch
        {
            0 => lhs,
            < 0 => lhs.Expand(-rhs),
            > 0 => lhs.Contract(rhs)
        };
    }
    
    private Rect2D Expand(int amount)
    {
        return new Rect2D(xMin: XMin - amount, xMax: XMax + amount, yMin: YMin - amount, yMax: YMax + amount);
    }

    private Rect2D Contract(int amount)
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
        
        return new Rect2D(xMin: XMin + amount, xMax: XMax - amount, yMin: YMin + amount, yMax: YMax - amount);
    }
}