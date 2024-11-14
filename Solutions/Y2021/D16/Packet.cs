namespace Solutions.Y2021.D16;

public abstract class Packet(int version, IEnumerable<Packet> subPackets)
{
    public int Version { get; } = version;
    public IReadOnlyList<Packet> SubPackets { get; } = new List<Packet>(subPackets);

    public abstract long Evaluate();
}

public sealed class LiteralPacket(int version, long value) : Packet(version, subPackets: [])
{
    public override long Evaluate()
    {
        return value;
    }
}

public sealed class OperatorPacket(int version, Operator @operator, IEnumerable<Packet> subPackets)
    : Packet(version, subPackets)
{
    public override long Evaluate()
    {
        switch (@operator)
        {
            case Operator.Sum:
                return SubPackets.Sum(p => p.Evaluate());
            case Operator.Product:
                return SubPackets.Select(p => p.Evaluate()).Aggregate((a, b) => a * b);
            case Operator.Minimum:
                return SubPackets.Min(p => p.Evaluate());
            case Operator.Maximum:
                return SubPackets.Max(p => p.Evaluate());
            case Operator.GreaterThan:
                return SubPackets[0].Evaluate() > SubPackets[1].Evaluate() ? 1L : 0L;
            case Operator.LessThan:
                return SubPackets[0].Evaluate() < SubPackets[1].Evaluate() ? 1L : 0L;
            case Operator.EqualTo:
                return SubPackets[0].Evaluate() == SubPackets[1].Evaluate() ? 1L : 0L;
            case Operator.Identity:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}