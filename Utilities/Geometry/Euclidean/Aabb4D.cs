using System.Collections;

namespace Utilities.Geometry.Euclidean;

/// <summary>
/// A readonly 4D AABB value type
/// </summary>
public readonly record struct Aabb4D : IEnumerable<Vec4D>
{
    public Aabb4D(Vec4D min, Vec4D max)
    {
        Min = min;
        Max = max;
    }

    public Aabb4D(int xMin, int xMax, int yMin, int yMax, int zMin, int zMax, int wMin, int wMax)
    {
        Min = new Vec4D(xMin, yMin, zMin, wMin);
        Max = new Vec4D(xMax, yMax, zMax, wMax);
    }
    
    public Aabb4D(ICollection<Vec4D> extents, bool inclusive)
    {
        var delta = inclusive ? 0 : 1;
        Min = new Vec4D(
            x: extents.Min(p => p.X) - delta,
            y: extents.Min(p => p.Y) - delta,
            z: extents.Min(p => p.Z) - delta,
            w: extents.Min(p => p.W) - delta);
        Max = new Vec4D(
            x: extents.Max(p => p.X) + delta,
            y: extents.Max(p => p.Y) + delta,
            z: extents.Max(p => p.Z) + delta,
            w: extents.Max(p => p.W) + delta);
    }
    
    public Vec4D Min { get; }
    public Vec4D Max { get; }
    
    public static bool Overlap(Aabb4D a, Aabb4D b, out  Aabb4D overlap)
    {
        var hasOverlap =
            a.Max.X >= b.Min.X && a.Min.X <= b.Max.X &&
            a.Max.Y >= b.Min.Y && a.Min.Y <= b.Max.Y &&
            a.Max.Z >= b.Min.Z && a.Min.Z <= b.Max.Z &&
            a.Max.W >= b.Min.W && a.Min.W <= b.Max.W;

        if (!hasOverlap)
        {
            overlap = default;
            return false;
        }
        
        overlap = new Aabb4D(
            xMin: int.Max(a.Min.X, b.Min.X),
            xMax: int.Min(a.Max.X, b.Max.X),
            yMin: int.Max(a.Min.Y, b.Min.Y),
            yMax: int.Min(a.Max.Y, b.Max.Y),
            zMin: int.Max(a.Min.Z, b.Min.Z),
            zMax: int.Min(a.Max.Z, b.Max.Z),
            wMin: int.Max(a.Min.W, b.Min.W),
            wMax: int.Min(a.Max.W, b.Max.W));
        return true;
    }
    
    public bool Contains(Vec4D pos, bool inclusive)
    {
        return inclusive
            ? ContainsInclusive(pos)
            : ContainsExclusive(pos);
    }
    
    private bool ContainsInclusive(Vec4D pos)
    {
        return 
            pos.X >= Min.X && pos.X <= Max.X && 
            pos.Y >= Min.Y && pos.Y <= Max.Y && 
            pos.Z >= Min.Z && pos.Z <= Max.Z &&
            pos.W >= Min.W && pos.W <= Max.W;
    }
    
    private bool ContainsExclusive(Vec4D pos)
    {
        return
            pos.X > Min.X && pos.X < Max.X &&
            pos.Y > Min.Y && pos.Y < Max.Y &&
            pos.Z > Min.Z && pos.Z < Max.Z &&
            pos.W > Min.W && pos.W < Max.W;
    }

    public override string ToString()
    {
        return $"[X={Min.X}..{Max.X}, Y={Min.Y}..{Max.Y}, Z={Min.Z}..{Max.Z}, W={Min.W}..{Max.W}]";
    }
    
    public IEnumerator<Vec4D> GetEnumerator()
    {
        for (var x = Min.X; x <= Max.X; x++)
        for (var y = Min.Y; y <= Max.Y; y++)
        for (var z = Min.Z; z <= Max.Z; z++)
        for (var w = Min.W; w <= Max.W; w++)
        {
            yield return new Vec4D(x, y, z, w);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}