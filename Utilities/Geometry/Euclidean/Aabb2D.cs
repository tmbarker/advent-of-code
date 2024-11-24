using System.Collections;

namespace Utilities.Geometry.Euclidean;

/// <summary>
///     A readonly 2D axis aligned bounding box value type
/// </summary>
public readonly record struct Aabb2D : IEnumerable<Vec2D>
{
    public Aabb2D(Vec2D min, Vec2D max)
    {
        Min = min;
        Max = max;
    }

    public Aabb2D(int xMin, int xMax, int yMin, int yMax)
    {
        Min = new Vec2D(xMin, yMin);
        Max = new Vec2D(xMax, yMax);
    }

    public Aabb2D(ICollection<Vec2D> extents, bool inclusive = true)
    {
        var delta = inclusive ? 0 : 1;
        Min = new Vec2D(
            X: extents.Min(p => p.X) - delta,
            Y: extents.Min(p => p.Y) - delta);
        Max = new Vec2D(
            X: extents.Max(p => p.X) + delta,
            Y: extents.Max(p => p.Y) + delta);
    }

    public Vec2D Min { get; }
    public Vec2D Max { get; }

    public int Width => Max.X - Min.X + 1;
    public int Height => Max.Y - Min.Y + 1;
    public long Area => (long)Width * Height;

    public static bool Overlap(Aabb2D a, Aabb2D b, out Aabb2D overlap)
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

    public bool Contains(Vec2D pos, bool inclusive)
    {
        return inclusive
            ? ContainsInclusive(pos)
            : ContainsExclusive(pos);
    }

    private bool ContainsInclusive(Vec2D pos)
    {
        return
            pos.X >= Min.X && pos.X <= Max.X &&
            pos.Y >= Min.Y && pos.Y <= Max.Y;
    }

    private bool ContainsExclusive(Vec2D pos)
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
            min: Min - amount * Vec2D.One,
            max: Max + amount * Vec2D.One);
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
            min: Min + amount * Vec2D.One,
            max: Max - amount * Vec2D.One);
    }

    public override string ToString()
    {
        return $"[X={Min.X}..{Max.X}, Y={Min.Y}..{Max.X}]";
    }

    public IEnumerator<Vec2D> GetEnumerator()
    {
        for (var x = Min.X; x <= Max.X; x++)
        for (var y = Min.Y; y <= Max.Y; y++)
        {
            yield return new Vec2D(x, y);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}