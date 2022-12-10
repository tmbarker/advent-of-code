using Problems.Y2022.Common;

namespace Problems.Y2022.D10;

/// <summary>
/// Cathode-Ray Tube: https://adventofcode.com/2022/day/10
/// </summary>
public class Solution : SolutionBase2022
{
    private const char InstructionDelimiter = ' ';
    private const char DarkPixel = '.';
    private const char LitPixel = '#';
    private const int CrtWidth = 40;

    private static readonly HashSet<int> SampleCycles = new() { 20, 60, 100, 140, 180, 220, };
    
    public override int Day => 10;
    
    public override string Run(int part)
    {
        var instructions = ParseInstructions(GetInput());

        return part switch
        {
            0 => CalculateSignalStrength(instructions).ToString(),
            1 => RenderCrt(instructions),
            _ => ProblemNotSolvedString,
        };
    }

    private static int CalculateSignalStrength(IEnumerable<(Opcode Opcode, int Arg)> instructions)
    {
        var cpu = new Cpu();
        var signalStrength = 0;

        void OnCpuTick(State state)
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

    private static string RenderCrt(IEnumerable<(Opcode Opcode, int Arg)> instructions)
    {
        var cpu = new Cpu();
        var image = "\n";
        
        void OnCpuTick(State state)
        {
            var pixelCol = (state.Cycle - 1) % CrtWidth;
            image += Math.Abs(state.X - pixelCol) <= 1 ? LitPixel : DarkPixel;

            if (pixelCol == CrtWidth - 1)
            {
                image += "\n";
            }
        }

        cpu.Ticked += OnCpuTick;
        cpu.Run(instructions);
        
        return image;
    }

    private static IEnumerable<(Opcode Opcode, int Arg)> ParseInstructions(IEnumerable<string> lines)
    {
        return lines.Select(ParseLine);
    }

    private static (Opcode Opcode, int Arg) ParseLine(string line)
    {
        var elements = line.Split(InstructionDelimiter);
        return elements.Length == 1 ? 
            (Opcode.Noop, 0) : 
            (Opcode.Addx, int.Parse(elements[1]));

    }
}