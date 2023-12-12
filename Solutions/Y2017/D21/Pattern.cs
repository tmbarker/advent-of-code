namespace Solutions.Y2017.D21;

public readonly struct Pattern
{
    private readonly char[] _buffer;

    public int Size { get; }
    public string Key => string.Concat(_buffer);
    public int On => _buffer.Count(c => c == '#');

    public char this[(int X, int Y) pos]
    {
        get => _buffer[pos.Y * (Size + 1) + pos.X];
        set => _buffer[pos.Y * (Size + 1) + pos.X] = value;
    }
    
    public Pattern(int size, string buffer)
    {
        _buffer = buffer.ToCharArray();
        Size = size;
    }

    public Pattern Flip()
    {
        var flipped = Empty(size: Size);
        
        for (var x = 0; x < Size; x++)
        for (var y = 0; y < Size; y++)
        {
            flipped[(x, y)] = this[(Size - x - 1, y)];
        }

        return flipped;
    }
    
    public Pattern Rotate()
    {
        var rotated = Empty(size: Size);
        
        for (var x = 0; x < Size; x++)
        for (var y = 0; y < Size; y++)
        {
            rotated[(x, y)] = this[(Size - 1 - y, x)];
        }

        return rotated;
    }

    public Pattern SubRegion((int X, int Y) offset, int size)
    {
        var subregion = Empty(size);
        for (var y = 0; y < size; y++)
        for (var x = 0; x < size; x++)
        {
            subregion[(x, y)] = this[(offset.X + x, offset.Y + y)];
        }

        return subregion;
    }

    public static Pattern Empty(int size)
    {
        var buffer = new string(c: '/', count: size * (size + 1) - 1);
        return new Pattern(size, buffer);
    }
}