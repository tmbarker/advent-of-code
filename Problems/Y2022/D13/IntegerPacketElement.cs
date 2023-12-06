namespace Problems.Y2022.D13;

public sealed class IntegerPacketElement : PacketElement
{
    public IntegerPacketElement(int value)
    {
        Value = value;
    }
    
    public int Value { get; }

    public ListPacketElement AsList()
    {
        return new ListPacketElement(Enumerable.Repeat(this, 1));
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}