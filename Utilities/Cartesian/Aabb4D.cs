using System.Collections;

namespace Utilities.Cartesian;

/// <summary>
/// A readonly axis aligned 4D AABB value type
/// </summary>
public readonly struct Aabb4D : IEnumerable<Vector4D>
{
    public Aabb4D(ICollection<Vector4D> extents, bool inclusive)
    {
        var delta = inclusive ? 0 : 1;
        XMin = extents.Min(p => p.X) - delta;
        XMax = extents.Max(p => p.X) + delta;
        YMin = extents.Min(p => p.Y) - delta;
        YMax = extents.Max(p => p.Y) + delta;
        ZMin = extents.Min(p => p.Z) - delta;
        ZMax = extents.Max(p => p.Z) + delta;
        WMin = extents.Min(p => p.W) - delta;
        WMax = extents.Max(p => p.W) + delta;
    }

    public Aabb4D(int xMin, int xMax, int yMin, int yMax, int zMin, int zMax, int tMin, int tMax)
    {
        XMin = xMin;
        XMax = xMax;
        YMin = yMin;
        YMax = yMax;
        ZMin = zMin;
        ZMax = zMax;
        WMin = tMin;
        WMax = tMax;
    }
    
    public int XMin { get; }
    public int XMax { get; }
    public int YMin { get; }
    public int YMax { get; }
    public int ZMin { get; }
    public int ZMax { get; }
    public int WMin { get; }
    public int WMax { get; }
    
    public bool Contains(Vector4D pos, bool inclusive)
    {
        return inclusive
            ? ContainsInclusive(pos)
            : ContainsExclusive(pos);
    }
    
    private bool ContainsInclusive(Vector4D pos)
    {
        return 
            pos.X >= XMin && pos.X <= XMax && 
            pos.Y >= YMin && pos.Y <= YMax && 
            pos.Z >= ZMin && pos.Z <= ZMax &&
            pos.W >= WMin && pos.W <= WMax;
    }
    
    private bool ContainsExclusive(Vector4D pos)
    {
        return
            pos.X > XMin && pos.X < XMax &&
            pos.Y > YMin && pos.Y < YMax &&
            pos.Z > ZMin && pos.Z < ZMax &&
            pos.W > WMin && pos.W < WMax;
    }

    public override string ToString()
    {
        return $"[X={XMin}..{XMax}, Y={YMin}..{YMax}, Z={ZMin}..{ZMax}, W={WMin}..{WMax}]";
    }
    
    public IEnumerator<Vector4D> GetEnumerator()
    {
        for (var x = XMin; x <= XMax; x++)
        for (var y = YMin; y <= YMax; y++)
        for (var z = ZMin; z <= ZMax; z++)
        for (var w = WMin; w <= WMax; w++)
        {
            yield return new Vector4D(x, y, z, w);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}