using Utilities.Collections;

namespace Problems.Y2016.D12;

public class Vm
{
    private readonly DefaultDictionary<string, long> _registers;
    private long _ip;

    public long this[string reg]
    {
        get => _registers[reg];
        set => _registers[reg] = value;
    }

    public Vm()
    {
        _registers = new DefaultDictionary<string, long>(defaultValue: 0L);
    }

    public void Run(IList<string[]> program)
    {
        while (_ip >= 0 && _ip < program.Count)
        {
            var tokens = program[(int)_ip];
            var args = tokens[1..];
            var op = tokens[0];

            switch (op)
            {
                case "cpy":
                    this[args[1]] = GetValue(args[0]);
                    break;
                case "inc":
                    this[args[0]]++;
                    break;
                case "dec":
                    this[args[0]]--;
                    break;
                case "jnz":
                    if (GetValue(args[0]) != 0)
                    {
                        _ip += GetValue(args[1]);
                        continue;
                    }
                    break;
            }
            
            _ip++;
        }
    }

    private long GetValue(string s)
    {
        return long.TryParse(s, out var value)
            ? value
            : this[s];
    }
}