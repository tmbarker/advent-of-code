using System.Collections;

namespace Utilities.Geometry.Euclidean;

/// <summary>
/// A readonly 3D AABB value type
/// </summary>
public readonly record struct Aabb3D : IEnumerable<Vector3D>
{
    public Aabb3D(Vector3D min, Vector3D max)
    {
        Min = min;
        Max = max;
    }
    
    public Aabb3D(int xMin, int xMax, int yMin, int yMax, int zMin, int zMax)
    {
        Min = new Vector3D(xMin, yMin, zMin);
        Max = new Vector3D(xMax, yMax, zMax);
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
    
    public Vector3D Min { get; }
    public Vector3D Max { get; }

    public int XLength => Max.X - Min.X + 1;
    public int YLength => Max.Y - Min.Y + 1;
    public int ZLength => Max.Z - Min.Z + 1;
    public long Volume => (long)XLength * YLength * ZLength;
    public Vector3D Center => new(x: (Min.X + Max.X) / 2, y: (Min.Y + Max.Y) / 2, z: (Min.Z + Max.Z) / 2);
    
    public static bool Overlap(Aabb3D a, Aabb3D b, out  Aabb3D overlap)
    {
        var hasOverlap =
            a.Max.X >= b.Min.X && a.Min.X <= b.Max.X &&
            a.Max.Y >= b.Min.Y && a.Min.Y <= b.Max.Y &&
            a.Max.Z >= b.Min.Z && a.Min.Z <= b.Max.Z;

        if (!hasOverlap)
        {
            overlap = default;
            return false;
        }
        
        overlap = new Aabb3D(
            xMin: int.Max(a.Min.X, b.Min.X),
            xMax: int.Min(a.Max.X, b.Max.X),
            yMin: int.Max(a.Min.Y, b.Min.Y),
            yMax: int.Min(a.Max.Y, b.Max.Y),
            zMin: int.Max(a.Min.Z, b.Min.Z),
            zMax: int.Min(a.Max.Z, b.Max.Z));
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
            pos.X >= Min.X && pos.X <= Max.X && 
            pos.Y >= Min.Y && pos.Y <= Max.Y && 
            pos.Z >= Min.Z && pos.Z <= Max.Z;
    }
    
    private bool ContainsExclusive(Vector3D pos)
    {
        return
            pos.X > Min.X && pos.X < Max.X &&
            pos.Y > Min.Y && pos.Y < Max.Y &&
            pos.Z > Min.Z && pos.Z < Max.Z;
    }
    
    public override string ToString()
    {
        return $"[X={Min.X}..{Max.X}, Y={Min.Y}..{Max.Y}, Z={Min.Z}..{Max.Z}]";
    }

    public IEnumerator<Vector3D> GetEnumerator()
    {
        for (var x = Min.X; x <= Max.X; x++)
        for (var y = Min.Y; y <= Max.Y; y++)
        for (var z = Min.Z; z <= Max.Z; z++)
        {
            yield return new Vector3D(x, y, z);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}