namespace Problems.Y2019.Common;

public static class Vm
{
    private const int Add  =  1;
    private const int Mul  =  2;
    private const int Halt = 99;
    
    private delegate int Operation(IList<int> memory, int pc);
    private static readonly Dictionary<int, Operation> OpExecutors = new()
    {
        { Add, ExecuteAdd },
        { Mul, ExecuteMul },
    };

    public static IList<int> Run(IList<int> memory)
    {
        var ip = 0;
        while (memory[ip] != Halt)
        {
            ip += OpExecutors[memory[ip]].Invoke(memory, ip);
        }
        return memory;
    }

    private static int ExecuteAdd(IList<int> memory, int ip)
    {
        memory[memory[ip + 3]] = memory[memory[ip + 1]] + memory[memory[ip + 2]];
        return 4;
    }
    
    private static int ExecuteMul(IList<int> memory, int ip)
    {
        memory[memory[ip + 3]] = memory[memory[ip + 1]] * memory[memory[ip + 2]];
        return 4;
    }
}