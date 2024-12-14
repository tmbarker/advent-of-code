using Utilities.Extensions;

namespace Solutions.Y2018.D19;

[PuzzleInfo("Go With The Flow", Topics.Assembly|Topics.Simulation, Difficulty.Hard)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();
        var ipAdr = input[0].ParseInt();
        var program = input[1..].Select(ParseInstruction).ToArray();

        return part switch
        {
            1 => Execute(ipAdr, program, r0: 0, enableOptimizations: true),
            2 => Execute(ipAdr, program, r0: 1, enableOptimizations: true),
            _ => PuzzleNotSolvedString
        };
    }

    private static int Execute(int ipAdr, IList<Cpu.Instruction> program, int r0, bool enableOptimizations)
    {
        var cpu = new Cpu(ipAdr) { [0] = r0 };
        var ec = cpu.Run(program, enableOptimizations);

        return ec;
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