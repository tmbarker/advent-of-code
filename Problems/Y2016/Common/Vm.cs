namespace Problems.Y2016.Common;

public sealed class Vm
{
    private readonly Dictionary<string, long> _registers = new();
    private long _ip;

    public long this[string reg]
    {
        get => _registers[reg];
        set => _registers[reg] = value;
    }

    public Vm()
    {
        _ip = 0;
        _registers["a"] = 0L;
        _registers["b"] = 0L;
        _registers["c"] = 0L;
        _registers["d"] = 0L;
    }
    
    public void Run(IList<string[]> program)
    {
        while (_ip < program.Count)
        {
            var tokens = program[(int)_ip];
            var args = tokens[1..];
            var op = tokens[0];

            switch (op)
            {
                case "cpy" when RegExists(args[1]):
                    this[args[1]] = GetValue(args[0]);
                    break;
                case "inc" when RegExists(args[0]):
                    this[args[0]]++;
                    break;
                case "dec" when RegExists(args[0]):
                    this[args[0]]--;
                    break;
                case "jnz":
                    if (GetValue(args[0]) != 0)
                    {
                        _ip += GetValue(args[1]);
                        continue;
                    }
                    break;
                case "tgl":
                    var targetAdr = _ip + GetValue(args[0]);
                    if (targetAdr >= 0 & targetAdr < program.Count)
                    {
                        var targetOp = program[(int)targetAdr][0];
                        program[(int)targetAdr][0] = program[(int)targetAdr].Length switch
                        {
                            2 when targetOp == "inc" => "dec",
                            2 => "inc",
                            3 when targetOp == "jnz" => "cpy",
                            3 => "jnz",
                            _ => throw new NoSolutionException()
                        };
                    }
                    break;
                case "out":
                    Console.WriteLine($"out => {GetValue(args[0])}");
                    break;
            }
            
            _ip++;
        }
    }

    private static bool RegExists(string arg)
    {
        return arg.Length == 1 && char.IsLetter(arg.Single());
    }
    
    private long GetValue(string s)
    {
        return long.TryParse(s, out var value)
            ? value
            : this[s];
    }
}