namespace Problems.Y2022.D16;

public readonly struct Strategy
{
    public Strategy(int flow, IEnumerable<string> opened)
    {
        Flow = flow;
        Opened = new HashSet<string>(opened);
    }
    
    public int Flow { get; }
    public IReadOnlySet<string> Opened { get; }
}