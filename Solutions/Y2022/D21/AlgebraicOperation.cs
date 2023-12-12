namespace Solutions.Y2022.D21;

public readonly struct AlgebraicOperation
{
    public Operator Operator { get; init; }
    public long KnownOperand { get; init; }
    public bool KnownOperandOnLhs { get; init; }
}