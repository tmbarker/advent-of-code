using System.Text;

namespace Problems.Y2022.D10;

[PuzzleInfo("Cathode-Ray Tube", Topics.Assembly, Difficulty.Medium, favourite: true)]
public sealed class Solution : SolutionBase
{
    private const int CrtWidth = 40;
    private static readonly HashSet<int> SampleCycles = [20, 60, 100, 140, 180, 220];

    public override object Run(int part)
    {
        var instructions = ParseInputLines(parseFunc: ParseInstruction);
        return part switch
        {
            1 => CalculateSignalStrength(instructions),
            2 => RenderCrt(instructions),
            _ => ProblemNotSolvedString
        };
    }

    private static int CalculateSignalStrength(IEnumerable<(Cpu.Opcode Opcode, int Arg)> instructions)
    {
        var cpu = new Cpu();
        var signalStrength = 0;

        void OnCpuTick(Cpu.State state)
        {
            if (SampleCycles.Contains(state.Cycle))
            {
                signalStrength += state.Cycle * state.X;
            }
        }

        cpu.Ticked += OnCpuTick;
        cpu.Run(instructions);

        return signalStrength;
    }

    private static string RenderCrt(IEnumerable<(Cpu.Opcode Opcode, int Arg)> instructions)
    {
        var cpu = new Cpu();
        var crtScreen = new StringBuilder("\n");
        
        void OnCpuTick(Cpu.State state)
        {
            var pixelCol = (state.Cycle - 1) % CrtWidth;
            var on = Math.Abs(state.X - pixelCol) <= 1;
            
            crtScreen.Append(on ? '#' : '.');
            
            if (pixelCol == CrtWidth - 1)
            {
                crtScreen.Append('\n');
            }
        }

        cpu.Ticked += OnCpuTick;
        cpu.Run(instructions);
        
        return crtScreen.ToString();
    }

    private static (Cpu.Opcode Opcode, int Arg) ParseInstruction(string line)
    {
        var elements = line.Split(' ');
        return elements.Length == 1 ? 
            (Cpu.Opcode.Noop, 0) : 
            (Cpu.Opcode.Addx, int.Parse(elements[1]));
    }
}