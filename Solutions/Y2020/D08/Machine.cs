namespace Solutions.Y2020.D08;

public sealed class Machine
{
    public const string Nop = "nop";
    public const string Jmp = "jmp";
    private const string Acc = "acc";

    public readonly record struct Result(int Acc, bool Looped);
    
    public static Result Run(List<(string Op, int Arg)> instructions)
    {
        var looped = false;
        var acc = 0;
        var pc = 0;
        var pcValueSet = new HashSet<int>([0]);

        while (pc < instructions.Count)
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
                looped = true;
                break;
            }
        }

        return new Result(acc, looped);
    }
}