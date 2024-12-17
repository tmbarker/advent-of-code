namespace Solutions.Y2015.D23;

public class Vm
{
    private readonly Dictionary<char, long> _registers = new();

    public long this[char reg]
    {
        get => _registers[reg];
        set => _registers[reg] = value;
    }

    public Vm()
    {
        Reset();
    }
    
    public void Run(IList<string> program)
    {
        var ip = 0;
        while (ip >= 0 && ip < program.Count)
        {
            var instr = program[ip];
            var tokens = instr.Split(' ');
            var op = tokens[0];
            
            switch (op)
            {
                case "hlf":
                    this[tokens[1][0]] /= 2L;
                    break;
                case "tpl":
                    this[tokens[1][0]] *= 3L;
                    break;
                case "inc":
                    this[tokens[1][0]]++;
                    break;
                case "jmp":
                    ip += int.Parse(tokens[1]);
                    continue;
                case "jie":
                    if (this[tokens[1][0]] % 2L == 0)
                    {
                        ip += int.Parse(tokens[2]);
                        continue;
                    }
                    break;
                case "jio":
                    if (this[tokens[1][0]] == 1)
                    {
                        ip += int.Parse(tokens[2]);
                        continue;
                    }
                    break;
            }
            ip++;
        }
    }
    
    private void Reset()
    {
        _registers.Clear();
        _registers['a'] = 0L;
        _registers['b'] = 0L;
    }
}