using Problems.Y2019.IntCode;

namespace Problems.Y2019.D23;

public class Computer
{
    private readonly int _id;
    private readonly IntCodeVm _vm;
    private readonly Queue<Packet> _inputBuffer = new();

    public event EventHandler<PacketEventArgs>? PacketEmitted;

    public Computer(int id, IList<long> firmware)
    {
        _id = id;
        _vm = IntCodeVm.Create(firmware, id);
        _vm.AwaitingInput += OnAwaitingInput;
        _vm.OutputEmitted += OnOutputEmitted;
    }

    public void Tick()
    {
        _vm.Run();
    }

    public void EnqueuePacket(Packet packet)
    {
        _inputBuffer.Enqueue(packet);
    }

    private void OnAwaitingInput(object? sender, EventArgs e)
    {
        if (_inputBuffer.Count == 0)
        {
            _vm.InputBuffer.Enqueue(item: -1L);
            return;
        }

        var packet = _inputBuffer.Dequeue();
        _vm.InputBuffer.Enqueue(packet.X);
        _vm.InputBuffer.Enqueue(packet.Y);
    }
    
    private void OnOutputEmitted(object? sender, EventArgs e)
    {
        if (_vm.OutputBuffer.Count < 3)
        {
            return;
        }

        RaisePacketEmitted(new Packet(
            RecipientId: (int)_vm.OutputBuffer.Dequeue(),
            X: _vm.OutputBuffer.Dequeue(),
            Y: _vm.OutputBuffer.Dequeue()));
    }

    private void RaisePacketEmitted(Packet payload)
    {
        PacketEmitted?.Invoke(this, new PacketEventArgs(_id, payload));
    }
}