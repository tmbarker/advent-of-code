using Problems.Y2019.IntCode;

namespace Problems.Y2019.D07;

public class AmplifierCircuit : IInputSource, IOutputSink
{
    private int _readCount;
    private Queue<int>? _phases;
    
    public int Signal { get; private set; }
    
    public void Reset(IEnumerable<int> phases)
    {
        _readCount = 0;
        _phases = new Queue<int>(phases);
        
        Signal = 0;
    }
    
    public int Read()
    {
        var val = _readCount++ % 2 == 0 ? _phases!.Dequeue() : Signal;
        return val;
    }

    public void Write(int value)
    {
        Signal = value;
    }
}