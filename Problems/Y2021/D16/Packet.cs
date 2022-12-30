namespace Problems.Y2021.D16;

public abstract class Packet
{
    protected Packet(int version, IEnumerable<Packet> subPackets)
    {
        Version = version;
        SubPackets = new List<Packet>(subPackets);
    }
    
    public int Version { get; }
    public IReadOnlyList<Packet> SubPackets { get; }

    public abstract long Evaluate();
}

public class LiteralPacket : Packet
{
    private readonly long _value;
    
    public LiteralPacket(int version, long value) : base(version, Enumerable.Empty<Packet>())
    {
        _value = value;
    }

    public override long Evaluate()
    {
        return _value;
    }
}

public class OperatorPacket : Packet
{
    private readonly Operator _operator;
    
    public OperatorPacket(int version, Operator @operator, IEnumerable<Packet> subPackets) : base(version, subPackets)
    {
        _operator = @operator;
    }

    public override long Evaluate()
    {
        switch (_operator)
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