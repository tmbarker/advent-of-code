using Problems.Common;
using Utilities.Extensions;

namespace Problems.Y2018.D19;

/// <summary>
/// Go With The Flow: https://adventofcode.com/2018/day/19
/// </summary>
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();
        var ipAdr = input[0].ParseInt();
        var program = ParseInstructions(input[1..]);

        return part switch
        {
            1 => Execute(ipAdr, program, r0: 0, enableOptimizations: true),
            2 => Execute(ipAdr, program, r0: 1, enableOptimizations: true),
            _ => ProblemNotSolvedString
        };
    }

    private static int Execute(int ipAdr, IList<Cpu.Instruction> program, int r0, bool enableOptimizations)
    {
        var cpu = new Cpu(ipAdr) { [0] = r0 };
        var ec = cpu.Run(program, enableOptimizations);

        return ec;
    }

    private static IList<Cpu.Instruction> ParseInstructions(IEnumerable<string> lines)
    {
        return new List<Cpu.Instruction>(lines.Select(ParseInstruction));
    }

    private static Cpu.Instruction ParseInstruction(string line)
    {
        var elements = line.Split(' ');
        return new Cpu.Instruction(
            Opcode: elements[0],
            A: int.Parse(elements[1]),
            B: int.Parse(elements[2]),
            C: int.Parse(elements[3]));
    }
}