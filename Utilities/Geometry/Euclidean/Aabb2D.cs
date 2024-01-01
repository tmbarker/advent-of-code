using System.Collections;

namespace Utilities.Geometry.Euclidean;

/// <summary>
/// A readonly 2D axis aligned bounding box value type
/// </summary>
public readonly record struct Aabb2D : IEnumerable<Vector2D>
{
    public Aabb2D(Vector2D min, Vector2D max)
    {
        Min = min;
        Max = max;
    }
    
    public Aabb2D(int xMin, int xMax, int yMin, int yMax)
    {
        Min = new Vector2D(xMin, yMin);
        Max = new Vector2D(xMax, yMax);
    }
    
    public Aabb2D(ICollection<Vector2D> extents, bool inclusive = true)
    {
        var delta = inclusive ? 0 : 1;
        Min = new Vector2D(
            x: extents.Min(p => p.X) - delta,
            y: extents.Min(p => p.Y) - delta);
        Max = new Vector2D(
            x: extents.Max(p => p.X) + delta,
            y: extents.Max(p => p.Y) + delta);
    }
    
    public Vector2D Min { get; }
    public Vector2D Max { get; }
    
    public int Width => Max.X - Min.X + 1;
    public int Height => Max.Y - Min.Y + 1;
    public long Area => (long)Width * Height;

    public static bool Overlap(Aabb2D a, Aabb2D b, out  Aabb2D overlap)
    {
        var hasOverlap =
            a.Max.X >= b.Min.X && a.Min.X <= b.Max.X &&
            a.Max.Y >= b.Min.Y && a.Min.Y <= b.Max.Y;

        if (!hasOverlap)
        {
            overlap = default;
            return false;
        }
        
        overlap = new Aabb2D(
            xMin: int.Max(a.Min.X, b.Min.X),
            xMax: int.Min(a.Max.X, b.Max.X),
            yMin: int.Max(a.Min.Y, b.Min.Y),
            yMax: int.Min(a.Max.Y, b.Max.Y));
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
            pos.X >= Min.X && pos.X <= Max.X && 
            pos.Y >= Min.Y && pos.Y <= Max.Y;
    }
    
    private bool ContainsExclusive(Vector2D pos)
    {
        return 
            pos.X > Min.X && pos.X < Max.X && 
            pos.Y > Min.Y && pos.Y < Max.Y;
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
    
    public static Aabb2D operator ++(Aabb2D lhs) => lhs + 1;
    
    public static Aabb2D operator --(Aabb2D lhs) => lhs - 1;
    
    private Aabb2D Expand(int amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), amount,
                message: "Expansion amount must be a non-zero positive number");
        }
        
        return new Aabb2D(
            min: Min - amount * Vector2D.One,
            max: Max + amount * Vector2D.One);
    }

    private Aabb2D Contract(int amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), amount,
                message: "Contraction amount must be a non-zero positive number");
        }
        
        if (2 * amount >= Width)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), amount,
                message: $"Contraction amount [{amount}] must be less than half of width [{Width}]");
        }
        
        if (2 * amount >= Height)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), amount,
                message: $"Contraction amount [{amount}] must be less than half of height [{Height}]");
        }
        
        return new Aabb2D(
            min: Min + amount * Vector2D.One,
            max: Max - amount * Vector2D.One);
    }
    
    public override string ToString()
    {
        return $"[X={Min.X}..{Max.X}, Y={Min.Y}..{Max.X}]";
    }

    public IEnumerator<Vector2D> GetEnumerator()
    {
        for (var x = Min.X; x <= Max.X; x++)
        for (var y = Min.Y; y <= Max.Y; y++)
        {
            yield return new Vector2D(x, y);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}