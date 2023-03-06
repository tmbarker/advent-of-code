namespace Problems.Y2018.D15;

public readonly struct CombatResult
{
    public char WinningTeam { get; }
    public int CompletedRounds { get; }
    public int HpSum { get; }
    public IReadOnlyDictionary<char, int> Casualties { get; }

    public int Score => CompletedRounds * HpSum;

    public static CombatResult FromState(GameState state)
    {
        return new CombatResult(
            winningTeam: state.Units.Values.First(unit => !unit.Dead).Team,
            completedRounds: state.Tick,
            hpSum: state.Units.Values.Sum(unit => unit.Hp),
            casualties: state.Casualties);
    }
    
    public void Print()
    {
        var casualtiesSummary = string.Join(' ', Casualties.Select(kvp => $"[{kvp.Key}={kvp.Value}]"));
        
        Console.WriteLine("COMBAT RESULT: ");
        Console.WriteLine($"Winning team: {WinningTeam}");
        Console.WriteLine($"Completed combat rounds: {CompletedRounds}");
        Console.WriteLine($"Remaining HP sum: {HpSum}");
        Console.WriteLine($"Casualties: {casualtiesSummary}");
    }
    
    private CombatResult(char winningTeam, int completedRounds, int hpSum, IReadOnlyDictionary<char, int> casualties)
    {
        WinningTeam = winningTeam;
        CompletedRounds = completedRounds;
        HpSum = hpSum;
        Casualties = casualties;
    }
}