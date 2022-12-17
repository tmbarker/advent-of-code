namespace Problems.Y2022.D16;

public readonly struct StrategyState
{
    public StrategyState(string currentValve, int timeRemaining, int pressureRelieved, IEnumerable<string> openedValves)
    {
        CurrentValve = currentValve;
        TimeRemaining = timeRemaining;
        PressureRelieved = pressureRelieved;
        OpenedValves = new HashSet<string>(openedValves);
    }
    
    public string CurrentValve { get; }
    public int TimeRemaining { get; }
    public int PressureRelieved { get; }
    public HashSet<string> OpenedValves { get; }

    public override string ToString()
    {
        var openedString = OpenedValves.Count == 0 ? "/" : string.Join(',', OpenedValves);
        return $"[V={CurrentValve}  T={TimeRemaining}  P={PressureRelieved}  O={openedString}]";
    }
}