namespace Problems.Y2022.D16;

public readonly struct Strategy
{
    public Strategy(IEnumerable<KeyValuePair<string, int>> valveOpeningTimings)
    {
        foreach (var kvp in valveOpeningTimings)
        {
            ValveOpeningTimings.Add(kvp.Key, kvp.Value);
        }
    }

    public Dictionary<string, int> ValveOpeningTimings { get; } = new();
}