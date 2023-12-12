namespace Solutions.Y2020.D08;

public sealed class Machine
{
    public const string Nop = "nop";
    public const string Jmp = "jmp";
    private const string Acc = "acc";

    public event Action<int>? LoopDetected;

    public Task<int> Run(IList<(string Op, int Arg)> instructions, CancellationToken token)
    {
        var acc = 0;
        var pc = 0;
        var pcValueSet = new HashSet<int>([0]);

        while (pc < instructions.Count && !token.IsCancellationRequested)
        {
            var (op, arg) = instructions[pc];
            switch (op)
            {
                case Nop:
                    pc++;
                    break;
                case Jmp:
                    pc += arg;
                    break;
                case Acc:
                    acc += arg;
                    pc++;
                    break;
            }
            
            if (!pcValueSet.Add(pc))
            {
                RaiseLoopDetected(acc);
            }
        }

        return Task.FromResult(acc);
    }
    
    private void RaiseLoopDetected(int acc)
    {
        LoopDetected?.Invoke(acc);
    }
}