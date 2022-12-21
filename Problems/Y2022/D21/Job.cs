namespace Problems.Y2022.D21;

public readonly struct Job
{
    public string Assignee { get; init; }
    public Operator Operator { get; init; }
    public long Value { get; init; }
    public string LhsOperand { get; init; }
    public string RhsOperand { get; init; }
}