namespace Solutions.Y2019.D23;

public readonly record struct Packet(int RecipientId, long X, long Y);

public sealed class PacketEventArgs(int senderId, Packet payload) : EventArgs
{
    public int SenderId { get; } = senderId;
    public Packet Payload { get; } = payload;
}