namespace Problems.Y2021.D24;

public static class Alu
{
    private const string Inp = "inp";
    private const string Add = "add";
    private const string Mul = "mul";
    private const string Div = "div";
    private const string Mod = "mod";
    private const string Eql = "eql";
    private const string ResultRegister = "z";
    
    private static readonly Dictionary<string, long> Registers = new()
    {
        {"w", 0L},
        {"x", 0L},
        {"y", 0L},
        {"z", 0L},
    };

    public static long Run(IEnumerable<string> instructions, Queue<int> inputBuffer)
    {
        Reset();
        
        foreach (var instr in instructions)
        {
            ParseInstruction(instr, out var cmd, out var lhs, out var rhs);
            ExecuteCommand(cmd, inputBuffer, lhs, rhs);
        }

        return Registers[ResultRegister];
    }

    private static void ExecuteCommand(string command, Queue<int> inputBuffer, string lhs, string rhs)
    {
        switch (command)
        {
            case Inp:
                Registers[lhs] = inputBuffer.Dequeue();
                break;
            case Add:
                Registers[lhs] += ResolveOperand(rhs);
                break;
            case Mul:
                Registers[lhs] *= ResolveOperand(rhs);
                break;
            case Div:
                Registers[lhs] /= ResolveOperand(rhs);
                break;
            case Mod:
                Registers[lhs] %= ResolveOperand(rhs);
                break;
            case Eql:
                Registers[lhs] = Registers[lhs] == ResolveOperand(rhs) ? 1 : 0;
                break;
        }
    }
    
    private static void Reset()
    {
        foreach (var register in Registers.Keys)
        {
            Registers[register] = 0L;
        }
    }

    private static long ResolveOperand(string strVal)
    {
        long value;
        if (string.IsNullOrWhiteSpace(strVal))
        {
            value = 0L;
        }
        else if (Registers.ContainsKey(strVal))
        {
            value = Registers[strVal];
        }
        else
        {
            value = long.Parse(strVal);
        }
        return value;
    }
    
    private static void ParseInstruction(string instruction, out string command, out string lhs, out string rhs)
    {
        var elements = instruction.Split(' ');
        command = elements[0];
        lhs = elements[1];
        rhs = elements.Length >= 3 ? elements[2] : string.Empty;
    }
}