namespace Problems.Y2022.D05;

public class StacksState
{
    public StacksState(Dictionary<int, Stack<char>> stackMap)
    {
        StackMap = stackMap;
    }
    
    public Dictionary<int, Stack<char>> StackMap { get; }
}