namespace Solutions.Y2019.D20;

public readonly struct PortalKey : IEquatable<PortalKey>
{
    private readonly char _c1;
    private readonly char _c2;
    
    public PortalKey(char c1, char c2)
    {
        _c1 = c1;
        _c2 = c2;
    }

    public bool Equals(PortalKey other)
    {
        return _c1 == other._c1 && _c2 == other._c2;
    }

    public override bool Equals(object? obj)
    {
        return obj is PortalKey other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_c1, _c2);
    }

    public static bool operator ==(PortalKey left, PortalKey right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(PortalKey left, PortalKey right)
    {
        return !left.Equals(right);
    }
    
    public override string ToString()
    {
        return new string(new[] { _c1, _c2 });
    }
}