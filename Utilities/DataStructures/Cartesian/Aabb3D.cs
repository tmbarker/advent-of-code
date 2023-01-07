namespace Utilities.DataStructures.Cartesian;

/// <summary>
/// An axis aligned 3D Cuboid value type
/// </summary>
public readonly struct Aabb3D
{ 
    public static Aabb3D CubeCenteredAtOrigin(int extent)
    {
        if (extent < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(extent), extent, $"{nameof(extent)} must be positive");
        }
        
        return new Aabb3D(
            xMin: -extent,
            xMax:  extent,
            yMin: -extent,
            yMax:  extent,
            zMin: -extent,
            zMax:  extent);
    } 
    
    public Aabb3D(ICollection<Vector3D> extents, bool inclusive)
    {
        var add = inclusive ? 0 : 1;
        XMin = extents.Min(p => p.X) + add;
        XMax = extents.Max(p => p.X) + add;
        YMin = extents.Min(p => p.Y) + add;
        YMax = extents.Max(p => p.Y) + add;
        ZMin = extents.Min(p => p.Z) + add;
        ZMax = extents.Max(p => p.Z) + add;
    }

    public Aabb3D(int xMin, int xMax, int yMin, int yMax, int zMin, int zMax)
    {
        XMin = xMin;
        XMax = xMax;
        YMin = yMin;
        YMax = yMax;
        ZMin = zMin;
        ZMax = zMax;
    }
    
    public int XMin { get; }
    public int XMax { get; }
    public int YMin { get; }
    public int YMax { get; }
    public int ZMin { get; }
    public int ZMax { get; }

    public static bool FindOverlap(Aabb3D lhs, Aabb3D rhs, out  Aabb3D overlap)
    {
        var hasOverlap =
            lhs.XMax >= rhs.XMin && lhs.XMin <= rhs.XMax &&
            lhs.YMax >= rhs.YMin && lhs.YMin <= rhs.YMax &&
            lhs.ZMax >= rhs.ZMin && lhs.ZMin <= rhs.ZMax;

        if (!hasOverlap)
        {
            overlap = default;
            return false;
        }

        var xLimits = new[] { lhs.XMin, lhs.XMax, rhs.XMin, rhs.XMax }.OrderBy(n =>n).ToList();
        var yLimits = new[] { lhs.YMin, lhs.YMax, rhs.YMin, rhs.YMax }.OrderBy(n =>n).ToList();
        var zLimits = new[] { lhs.ZMin, lhs.ZMax, rhs.ZMin, rhs.ZMax }.OrderBy(n =>n).ToList();
        
        overlap = new Aabb3D(
            xMin: xLimits[1],
            xMax: xLimits[2],
            yMin: yLimits[1],
            yMax: yLimits[2],
            zMin: zLimits[1],
            zMax: zLimits[2]);
        return true;
    }

    public long GetVolume()
    {
        return (long)(XMax - XMin + 1) * (YMax - YMin + 1) * (ZMax - ZMin + 1);
    }
    
    public bool Contains(Vector3D pos, bool inclusive)
    {
        return inclusive
            ? ContainsInclusive(pos)
            : ContainsExclusive(pos);
    }
    
    private bool ContainsInclusive(Vector3D pos)
    {
        return 
            pos.X >= XMin && pos.X <= XMax && 
            pos.Y >= YMin && pos.Y <= YMax && 
            pos.Z >= ZMin && pos.Z <= ZMax;
    }
    
    private bool ContainsExclusive(Vector3D pos)
    {
        return
            pos.X > XMin && pos.X < XMax &&
            pos.Y > YMin && pos.Y < YMax &&
            pos.Z > ZMin && pos.Z < ZMax;
    }

    public override string ToString()
    {
        return $"[X={XMin}..{XMax}, Y={YMin}..{YMax}, Z={ZMin}..{ZMax}]";
    }
}