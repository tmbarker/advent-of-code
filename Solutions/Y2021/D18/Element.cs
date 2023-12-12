namespace Solutions.Y2021.D18;

public readonly struct Element
{
    public static readonly Element Open = new (Type.Open);
    public static readonly Element Close = new (Type.Close);
    public static readonly Element Delim = new (Type.Delim);
    
    public Type ElementType { get; }
    public int Value { get; }

    private Element(Type elementType)
    {
        ElementType = elementType;
        Value = 0;
    }

    public Element(int value) : this(Type.Value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return ElementType switch
        {
            Type.Open => "[",
            Type.Close => "]",
            Type.Delim => ",",
            Type.Value => Value.ToString(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public enum Type
    {
        Open,
        Close,
        Delim,
        Value
    }
}

