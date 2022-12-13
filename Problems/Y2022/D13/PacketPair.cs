namespace Problems.Y2022.D13;

public readonly struct PacketPair
{
    public PacketPair(int index, PacketElement first, PacketElement second)
    {
        Index = index;
        First = first;
        Second = second;
    }
    
    public int Index { get; }
    public PacketElement First { get; }
    public PacketElement Second { get; }
}