namespace Solutions.Y2020.D09;

public sealed class Sum
{
    public long Value { get; private set; }
    private List<long> Elements { get; } = [];

    public Sum(long initial)
    {
        Value = initial;
        Elements.Add(initial);
    }

    public long Add(long number)
    {
        Value += number;
        Elements.Add(number);
        return Value;
    }
    
    public long GetExtremaSum()
    {
        return Elements.Min() + Elements.Max();
    }
}