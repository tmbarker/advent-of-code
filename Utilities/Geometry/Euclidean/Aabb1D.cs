using System.Collections;

namespace Utilities.Geometry.Euclidean;

/// <summary>
/// A readonly interval value type
/// </summary>
public readonly struct Aabb1D : IEnumerable<int>, IEquatable<Aabb1D>
{
    public Aabb1D(int min, int max)
    {
        Min = min;
        Max = max;
    }

    public int Min { get; }
    public int Max { get; }
    public int Length => Max - Min + 1;
    
    public static bool Overlap(Aabb1D lhs, Aabb1D rhs, out  Aabb1D overlap)
    {
        var hasOverlap = lhs.Max >= rhs.Min && lhs.Min <= rhs.Max;
        if (!hasOverlap)
        {
            overlap = default;
            return false;
        }

        var limits = new[] { lhs.Min, lhs.Max, rhs.Min, rhs.Max }.Order().ToList();
        overlap = new Aabb1D(
            min: limits[1],
            max: limits[2]);
        return true;
    }
    
    public bool Contains(int value, bool inclusive)
    {
        return inclusive 
            ? ContainsInclusive(value) 
            : ContainsExclusive(value);
    }
    
    private bool ContainsInclusive(int value)
    {
        return value >= Min && value <= Max;
    }
    
    private bool ContainsExclusive(int value)
    {
        return value > Min && value < Max;
    }

    public override string ToString()
    {
        return $"[{Min}..{Max}]";
    }

    public IEnumerator<int> GetEnumerator()
    {
        for (var v = Min; v <= Max; v++)
        {
            yield return v;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public bool Equals(Aabb1D other)
    {
        return Min == other.Min && Max == other.Max;
    }

    public override bool Equals(object? obj)
    {
        return obj is Aabb1D other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Min, Max);
    }

    public static bool operator ==(Aabb1D left, Aabb1D right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Aabb1D left, Aabb1D right)
    {
        return !left.Equals(right);
    }
}