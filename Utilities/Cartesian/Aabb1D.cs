using System.Collections;

namespace Utilities.Cartesian;

/// <summary>
/// A readonly interval value type
/// </summary>
public readonly struct Aabb1D : IEnumerable<int>
{
    public Aabb1D(ICollection<int> extents, bool inclusive)
    {
        var delta = inclusive ? 0 : 1;
        Min = extents.Min() - delta;
        Max = extents.Max() + delta;
    }
    
    public Aabb1D(int min, int max)
    {
        Min = min;
        Max = max;
    }

    private int Min { get; }
    private int Max { get; }
    
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
}