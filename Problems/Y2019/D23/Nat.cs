using Problems.Common;

namespace Problems.Y2019.D23;

public class Nat
{
    private Packet Packet { get; set; }
    public bool BufferInitialized { get; private set; }
    
    public void WriteBuffer(long x, long y)
    {
        Packet = new Packet(RecipientId: 0, x, y);
        BufferInitialized = true;
    }

    public Packet ReadBuffer()
    {
        return BufferInitialized ? Packet : throw new NoSolutionException();
    }
}