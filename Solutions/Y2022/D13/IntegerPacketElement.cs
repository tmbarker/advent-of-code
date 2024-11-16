namespace Solutions.Y2022.D13;

public sealed class IntegerPacketElement(int value) : PacketElement
{
    public int Value { get; } = value;

    public ListPacketElement AsList()
    {
        return new ListPacketElement(elements: [this]);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}