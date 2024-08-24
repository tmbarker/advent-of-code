namespace Solutions.Y2019.D23;

public sealed class NetworkAwaiter
{
    private readonly Network _network;
    private readonly CancellationTokenSource _cts = new();
    
    private int _targetRecipient = -1;
    private long _result = -1L;
    private long _prevNatMessage = -1L;

    public NetworkAwaiter(IList<long> firmware)
    {
        _network = new Network(firmware);
        _network.PacketTransmitted += OnNetworkPacketObserved;
    }

    public async Task<long> WaitForMessage(int targetRecipient)
    {
        _targetRecipient = targetRecipient;

        try
        {
            await _network.RunAsync(_cts.Token);
        }
        catch (OperationCanceledException)
        {
        }
        
        return _result;
    }
    
    public async Task<long> WaitForRepeatedNatMessage()
    {
        try
        {
            await _network.RunAsync(_cts.Token);
        }
        catch (OperationCanceledException)
        {
        }
        
        return _result;  
    }
    
    private void OnNetworkPacketObserved(object? sender, PacketEventArgs e)
    {
        if (e.Payload.RecipientId == _targetRecipient)
        {
            HaltWithResult(e.Payload.Y);
            return;
        }

        if (e.SenderId == Network.NatId)
        {
            OnNatPacketObserved(e.Payload);
        }
    }
    
    private void OnNatPacketObserved(Packet packet)
    {
        if (packet.Y == _prevNatMessage)
        {
            HaltWithResult(packet.Y);
        }

        _prevNatMessage = packet.Y;
    }

    private void HaltWithResult(long result)
    {
        _result = result;
        _cts.Cancel();
    }
}