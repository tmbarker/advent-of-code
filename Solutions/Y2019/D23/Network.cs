namespace Solutions.Y2019.D23;

public sealed class Network
{
    public const int NatId = 255;
    private const int NumNodes = 50;

    private readonly Nat _nat = new();
    private readonly Dictionary<int, Computer> _computers = new(capacity: NumNodes);
    
    public event EventHandler<PacketEventArgs>? PacketTransmitted;

    private IEnumerable<Computer> Nodes => _computers.Values;
    private bool AtIdle { get; set; }

    public Network(IList<long> firmware)
    {
        for (var i = 0; i < NumNodes; i++)
        {
            _computers.Add(i, new Computer(id: i, firmware));
            _computers[i].PacketEmitted += OnComputerPacketEmitted;
        }
    }

    public Task RunAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            Tick();
        }

        return token.IsCancellationRequested
            ? Task.FromCanceled(token)
            : Task.CompletedTask;
    }

    private void Tick()
    {
        AtIdle = true;
        TickNodes();
        TickNat();
    }

    private void TickNodes()
    {
        foreach (var node in Nodes)
        {
            node.Tick();
        }
    }

    private void TickNat()
    {
        if (AtIdle && _nat.BufferInitialized)
        {
            TransmitNatPacket();
        }
    }

    private void TransmitNatPacket()
    {
        var packet = _nat.ReadBuffer();
        var target = _computers[packet.RecipientId];

        target.EnqueuePacket(packet);
        target.Tick();
        
        RaisePacketTransmitted(NatId, packet);
    }
    
    private void OnComputerPacketEmitted(object? sender, PacketEventArgs e)
    {
        var senderId = e.SenderId;
        var payload = e.Payload;
        
        if (_computers.TryGetValue(payload.RecipientId, out var recipient))
        {
            recipient.EnqueuePacket(payload);
        }

        if (payload.RecipientId == NatId)
        {
            _nat.WriteBuffer(x: payload.X, y: payload.Y);
        }

        AtIdle = false;
        RaisePacketTransmitted(senderId, payload);
    }

    private void RaisePacketTransmitted(int senderId, Packet payload)
    {
        PacketTransmitted?.Invoke(this, new PacketEventArgs(senderId, payload));
    }
}