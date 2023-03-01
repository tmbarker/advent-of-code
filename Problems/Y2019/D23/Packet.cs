namespace Problems.Y2019.D23;

public readonly record struct Packet(int RecipientId, long X, long Y);

public class PacketEventArgs : EventArgs
{
    public PacketEventArgs(int senderId, Packet payload)
    {
        SenderId = senderId;
        Payload = payload;
    }
    
    public int SenderId { get; }
    public Packet Payload { get; }
}