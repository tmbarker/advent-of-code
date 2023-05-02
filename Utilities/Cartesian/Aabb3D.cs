using System.Collections;

namespace Utilities.Cartesian;

/// <summary>
/// A readonly axis aligned 3D Cuboid value type
/// </summary>
public readonly struct Aabb3D : IEnumerable<Vector3D>, IEquatable<Aabb3D>
{ 
    public static Aabb3D CubeCenteredAt(Vector3D center, int extent)
    {
        if (extent < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(extent), extent, $"{nameof(extent)} must be positive");
        }

        return new Aabb3D(
            xMin: center.X - extent,
            xMax: center.X + extent,
            yMin: center.Y - extent,
            yMax: center.Y + extent,
            zMin: center.Z - extent,
            zMax: center.Z + extent);
    } 
    
    public Aabb3D(ICollection<Vector3D> extents, bool inclusive)
    {
        var delta = inclusive ? 0 : 1;
        XMin = extents.Min(p => p.X) - delta;
        XMax = extents.Max(p => p.X) + delta;
        YMin = extents.Min(p => p.Y) - delta;
        YMax = extents.Max(p => p.Y) + delta;
        ZMin = extents.Min(p => p.Z) - delta;
        ZMax = extents.Max(p => p.Z) + delta;
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

    public int XLength => XMax - XMin + 1;
    public int YLength => YMax - YMin + 1;
    public int ZLength => ZMax - ZMin + 1;
    public long Volume => (long)XLength * YLength * ZLength;
    public Vector3D Center => new(x: (XMin + XMax) / 2, y: (YMin + YMax) / 2, z: (ZMin + ZMax) / 2);
    
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

        var xLimits = new[] { lhs.XMin, lhs.XMax, rhs.XMin, rhs.XMax }.Order().ToList();
        var yLimits = new[] { lhs.YMin, lhs.YMax, rhs.YMin, rhs.YMax }.Order().ToList();
        var zLimits = new[] { lhs.ZMin, lhs.ZMax, rhs.ZMin, rhs.ZMax }.Order().ToList();
        
        overlap = new Aabb3D(
            xMin: xLimits[1],
            xMax: xLimits[2],
            yMin: yLimits[1],
            yMax: yLimits[2],
            zMin: zLimits[1],
            zMax: zLimits[2]);
        return true;
    }

    public long GetSurfaceArea()
    {
        return 2L * (ZLength * XLength + YLength * XLength + YLength * ZLength);
    }

    public Vector3D GetMinVertex()
    {
        return new Vector3D(XMin, YMin, ZMin);
    }
    
    public Vector3D GetMaxVertex()
    {
        return new Vector3D(XMax, YMax, ZMax);
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

    public IEnumerator<Vector3D> GetEnumerator()
    {
        for (var x = XMin; x <= XMax; x++)
        for (var y = YMin; y <= YMax; y++)
        for (var z = ZMin; z <= ZMax; z++)
        {
            yield return new Vector3D(x, y, z);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public bool Equals(Aabb3D other)
    {
        return 
            XMin == other.XMin && XMax == other.XMax && 
            YMin == other.YMin && YMax == other.YMax &&
            ZMin == other.ZMin && ZMax == other.ZMax;
    }

    public override bool Equals(object? obj)
    {
        return obj is Aabb3D other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(XMin, XMax, YMin, YMax, ZMin, ZMax);
    }

    public static bool operator ==(Aabb3D left, Aabb3D right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Aabb3D left, Aabb3D right)
    {
        return !left.Equals(right);
    }
}