namespace Problems.Y2019.D23;

public class NetworkAwaiter
{
    private readonly Network _network;
    private readonly CancellationTokenSource _cts = new();
    private readonly TaskCompletionSource<long> _tcs = new();
    
    private int _targetRecipient = -1;
    private long _prevNatMessage = -1L;

    public NetworkAwaiter(IList<long> firmware)
    {
        _network = new Network(firmware);
        _network.PacketTransmitted += OnNetworkPacketObserved;
    }

    public Task<long> WaitForMessage(int targetRecipient)
    {
        _targetRecipient = targetRecipient;
        _network.RunAsync(_cts.Token);
        return _tcs.Task;
    }
    
    public Task<long> WaitForRepeatedNatMessage()
    {
        _network.RunAsync(_cts.Token);
        return _tcs.Task;
    }
    
    private void OnNetworkPacketObserved(object? sender, PacketEventArgs e)
    {
        if (e.Payload.RecipientId == _targetRecipient)
        {
            SetResult(e.Payload.Y);
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
            SetResult(packet.Y);
        }

        _prevNatMessage = packet.Y;
    }

    private void SetResult(long result)
    {
        _cts.Cancel();
        _tcs.SetResult(result);
    }
}