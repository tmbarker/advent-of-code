namespace Problems.Y2022.D05;

public readonly struct CraneInstruction
{
    public int NumMoves { get; init; }
    public int SourceStack { get; init; }
    public int DestinationStack { get; init; }
}