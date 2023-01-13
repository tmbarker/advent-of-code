namespace Problems.Y2020.D07;

public readonly struct BagContent
{
    public BagContent(int count, string colour)
    {
        Count = count;
        Colour = colour;
    }
    
    public int Count { get; }
    public string Colour { get; }

    public void Deconstruct(out int count, out string colour)
    {
        count = Count;
        colour = Colour;
    }
}