using Utilities.Extensions;

namespace Solutions.Y2018.D21;

[PuzzleInfo("Chronal Conversion", Topics.Assembly|Topics.Simulation, Difficulty.Hard)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();
        var ipAdr = input[0].ParseInt();
        var program = ParseInstructions(input[1..]);
        
        return part switch
        {
            1 => GetEarliestHaltValue(ipAdr, program),
            2 => GetLatestHaltValue(ipAdr, program),
            _ => PuzzleNotSolvedString
        };
    }

    private static long GetEarliestHaltValue(int ipAdr, IList<Cpu.Instruction> program)
    {
        //  This logic comes from analyzing the assembly. Reference the adjacent asm.txt file to see the
        //  annotated assembly. For my input, with my IP bound to r2, the program could only halt when
        //  r4 == r0 is checked at IP = 28L. Therefore, the value of r4 the first time this condition is checked
        //  is my answer
        //
        var value = 0L;
        var cpu = new Cpu(ipAdr);
        var cts = new CancellationTokenSource();

        void OnHaltCheck()
        {
            value = cpu[4L];
            cts.Cancel();
        }

        cpu.RegisterIpListener(ipValue: 28L, OnHaltCheck);
        cpu.Run(program, cts.Token);
        
        return value;
    }

    private long GetLatestHaltValue(int ipAdr, IList<Cpu.Instruction> program)
    {
        //  This logic comes from analyzing the assembly. Reference the adjacent asm.txt file to see the
        //  annotated assembly. For my input, with my IP bound to r2, the program could only halt when
        //  r4 == r0 is checked at IP = 28L. Therefore, my answer is the last unique value in r4 when the 
        //  IP hits 28L, before r4 starts cycling
        //
        var lastValue = 0L;
        var prevValues = new HashSet<long>();
        var cpu = new Cpu(ipAdr);
        var cts = new CancellationTokenSource();

        void OnHaltCheck()
        {
            var checkRegVal = cpu[4L];
            if (prevValues.Add(checkRegVal))
            {
                lastValue = checkRegVal;
            }
            else
            {
                cts.Cancel();
            }
        }

        Log("Warning, part 2 of this solution will take some time to run...");
        
        cpu.RegisterIpListener(ipValue: 28L, OnHaltCheck);
        cpu.Run(program, cts.Token);
        
        return lastValue;
    }

    private static List<Cpu.Instruction> ParseInstructions(IEnumerable<string> lines)
    {
        return [..lines.Select(ParseInstruction)];
    }

    private static Cpu.Instruction ParseInstruction(string line)
    {
        var elements = line.Split(' ');
        return new Cpu.Instruction(
            Opcode: elements[0],
            A: long.Parse(elements[1]),
            B: long.Parse(elements[2]),
            C: long.Parse(elements[3]));
    }
}