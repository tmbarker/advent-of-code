namespace Problems.Y2021.D14;

public readonly struct Rule
{
    public char Lhs { get; init; }
    public char Rhs { get; init; }
    public char Insert { get; init; }

    public (char, char) MatchKey => (Lhs, Rhs);
}