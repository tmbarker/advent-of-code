namespace Problems.Y2019.D20;

public readonly struct Portal : IEquatable<Portal>
{
    private readonly char _p1;
    private readonly char _p2;
    
    public Portal(char p1, char p2)
    {
        _p1 = p1;
        _p2 = p2;
    }

    public bool Equals(Portal other)
    {
        return _p1 == other._p1 && _p2 == other._p2;
    }

    public override bool Equals(object? obj)
    {
        return obj is Portal other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_p1, _p2);
    }

    public static bool operator ==(Portal left, Portal right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Portal left, Portal right)
    {
        return !left.Equals(right);
    }
    
    public override string ToString()
    {
        return new string(new[] { _p1, _p2 });
    }
}