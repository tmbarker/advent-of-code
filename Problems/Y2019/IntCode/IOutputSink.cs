namespace Problems.Y2019.IntCode;

public interface IOutputSink
{
    void Write(int value);
}

public class OutputBuffer : IOutputSink
{
    public bool LogToConsole { get; init; }
    public IList<int> Values { get; } = new List<int>();

    public void Write(int value)
    {
        Values.Add(value);
        if (LogToConsole)
        {
            Console.WriteLine($"[{nameof(IntCode)}.{nameof(Vm)}] OUT => {value}");   
        }
    }

    public void Clear()
    {
        Values.Clear();
    }
}