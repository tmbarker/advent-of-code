namespace Problems.Y2022.D16;

public struct StrategyState
{
    public StrategyState()
    {
        CurrentTime = 1;
    }

    public int CurrentTime { get; set; }
    public Stack<string> Path { get; } = new();
    public Dictionary<string, int> OpenedValves { get; } = new ();

    public string CurrentValve => Path.Peek();

    public bool IsValveOpened(string valve)
    {
        return OpenedValves.ContainsKey(valve);
    }
}