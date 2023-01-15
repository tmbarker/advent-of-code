namespace Utilities.Cartesian;

/// <summary>
/// A readonly interval value type
/// </summary>
public readonly struct Aabb1D
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
    
    public int Min { get; }
    public int Max { get; }
    
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
}