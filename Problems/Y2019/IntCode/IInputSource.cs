namespace Problems.Y2019.IntCode;

public interface IInputSource
{
    int Read();
}

public class ConstantInputSource : IInputSource
{
    private readonly int _constant;
    
    public bool LogToConsole { get; init; }
    
    public ConstantInputSource(int constant)
    {
        _constant = constant;
    }

    public int Read()
    {
        if (LogToConsole)
        {
            Console.WriteLine($"[{nameof(IntCode)}.{nameof(Vm)}] IN  => {_constant}");
        }
        return _constant;
    }
}