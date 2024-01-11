namespace Solutions.Y2022.D16;

public readonly struct Strategy(int flow, IEnumerable<string> opened)
{
    public int Flow { get; } = flow;
    public IReadOnlySet<string> Opened { get; } = new HashSet<string>(opened);
}