using Problems.Common;

namespace Problems.Y2017.D08;

/// <summary>
/// I Heard You Like Registers: https://adventofcode.com/2017/day/8
/// </summary>
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();
        var instructions = new List<Instruction>(input.Select(ParseInstruction));
        
        return part switch
        {
            1 => GetMaxRegister(instructions, Scope.Halted),
            2 => GetMaxRegister(instructions, Scope.Lifetime),
            _ => ProblemNotSolvedString
        };
    }

    private static int GetMaxRegister(IList<Instruction> instructions, Scope scope)
    {
        var max = 0;
        var registers = instructions
            .SelectMany(instr => new[] { instr.DestReg, instr.SrcReg })
            .Distinct()
            .ToDictionary(r => r, _ => 0);

        foreach (var instr in instructions)
        {
            var condition = instr.CheckOp switch
            {
                ">"  => registers[instr.SrcReg]  > instr.CheckArg,
                "<"  => registers[instr.SrcReg]  < instr.CheckArg,
                ">=" => registers[instr.SrcReg] >= instr.CheckArg,
                "<=" => registers[instr.SrcReg] <= instr.CheckArg,
                "==" => registers[instr.SrcReg] == instr.CheckArg,
                "!=" => registers[instr.SrcReg] != instr.CheckArg,
                _ => throw new NoSolutionException()
            };

            if (!condition)
            {
                continue;
            }

            switch (instr.DestOp)
            {
                case "inc":
                    registers[instr.DestReg] += instr.DestArg;
                    break;
                case "dec":
                    registers[instr.DestReg] -= instr.DestArg;
                    break;
            }

            if (scope == Scope.Lifetime)
            {
                max = Math.Max(max, registers.Values.Max());
            }
        }

        return Math.Max(max, registers.Values.Max());
    }
    
    private static Instruction ParseInstruction(string line)
    {
        var elements = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return new Instruction(
            DestReg: elements[0],
            DestOp: elements[1],
            DestArg: int.Parse(elements[2]),
            SrcReg: elements[4],
            CheckOp: elements[5],
            CheckArg: int.Parse(elements[6]));
    }
}