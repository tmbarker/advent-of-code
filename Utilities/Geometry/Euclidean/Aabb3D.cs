using System.Collections;

namespace Utilities.Geometry.Euclidean;

/// <summary>
/// A readonly 3D AABB value type
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
        Min = new Vector3D(
            x: extents.Min(p => p.X) - delta,
            y: extents.Min(p => p.Y) - delta,
            z: extents.Min(p => p.Z) - delta);
        Max = new Vector3D(
            x: extents.Max(p => p.X) + delta,
            y: extents.Max(p => p.Y) + delta,
            z: extents.Max(p => p.Z) + delta);
    }

    public Aabb3D(int xMin, int xMax, int yMin, int yMax, int zMin, int zMax)
    {
        Min = new Vector3D(xMin, yMin, zMin);
        Max = new Vector3D(xMax, yMax, zMax);
    }

    public Aabb3D(Vector3D min, Vector3D max)
    {
        Min = min;
        Max = max;
    }
    
    public Vector3D Min { get; }
    public Vector3D Max { get; }

    public int XMin => Min.X;
    public int XMax => Max.X;
    public int YMin => Min.Y;
    public int YMax => Max.Y;
    public int ZMin => Min.Z;
    public int ZMax => Max.Z;

    public int XLength => XMax - XMin + 1;
    public int YLength => YMax - YMin + 1;
    public int ZLength => ZMax - ZMin + 1;
    public long Volume => (long)XLength * YLength * ZLength;
    public Vector3D Center => new(x: (XMin + XMax) / 2, y: (YMin + YMax) / 2, z: (ZMin + ZMax) / 2);
    
    public static bool Overlap(Aabb3D a, Aabb3D b, out  Aabb3D overlap)
    {
        var hasOverlap =
            a.XMax >= b.XMin && a.XMin <= b.XMax &&
            a.YMax >= b.YMin && a.YMin <= b.YMax &&
            a.ZMax >= b.ZMin && a.ZMin <= b.ZMax;

        if (!hasOverlap)
        {
            overlap = default;
            return false;
        }

        var xLimits = new[] { a.XMin, a.XMax, b.XMin, b.XMax }.Order().ToList();
        var yLimits = new[] { a.YMin, a.YMax, b.YMin, b.YMax }.Order().ToList();
        var zLimits = new[] { a.ZMin, a.ZMax, b.ZMin, b.ZMax }.Order().ToList();
        
        overlap = new Aabb3D(
            xMin: xLimits[1],
            xMax: xLimits[2],
            yMin: yLimits[1],
            yMax: yLimits[2],
            zMin: zLimits[1],
            zMax: zLimits[2]);
        return true;
    }

    public Aabb3D Shift(Vector3D amount)
    {
        return new Aabb3D(min: Min + amount, max: Max + amount);
    }
    
    public long GetSurfaceArea()
    {
        return 2L * (ZLength * XLength + YLength * XLength + YLength * ZLength);
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