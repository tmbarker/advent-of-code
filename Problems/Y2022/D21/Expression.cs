namespace Problems.Y2022.D21;

public readonly struct Expression
{
    public string Id { get; init; }
    public Operator Operator { get; init; }
    public long Value { get; init; }
    public string Lhs { get; init; }
    public string Rhs { get; init; }
}