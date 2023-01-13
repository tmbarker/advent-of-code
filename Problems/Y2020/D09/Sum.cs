namespace Problems.Y2020.D09;

public class Sum
{
    public long Value { get; private set; }
    private IList<long> Elements { get; } = new List<long>();

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