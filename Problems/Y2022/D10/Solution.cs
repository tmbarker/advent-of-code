using Problems.Attributes;
using Problems.Y2022.Common;

namespace Problems.Y2022.D10;

/// <summary>
/// Cathode-Ray Tube: https://adventofcode.com/2022/day/10
/// </summary>
[Favourite("Cathode-Ray Tube", Topics.Assembly, Difficulty.Medium)]
public class Solution : SolutionBase2022
{
    private const char InstructionDelimiter = ' ';
    private const char DarkPixel = '.';
    private const char LitPixel = '#';
    private const int CrtWidth = 40;

    private static readonly HashSet<int> SampleCycles = new() { 20, 60, 100, 140, 180, 220, };
    
    public override int Day => 10;
    
    public override object Run(int part)
    {
        var instructions = ParseInstructions(GetInputLines());
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
        var crtScreen = "\n";
        
        void OnCpuTick(Cpu.State state)
        {
            var pixelCol = (state.Cycle - 1) % CrtWidth;
            crtScreen += Math.Abs(state.X - pixelCol) <= 1 ? LitPixel : DarkPixel;

            if (pixelCol == CrtWidth - 1)
            {
                crtScreen += "\n";
            }
        }

        cpu.Ticked += OnCpuTick;
        cpu.Run(instructions);
        
        return crtScreen;
    }

    private static IEnumerable<(Cpu.Opcode Opcode, int Arg)> ParseInstructions(IEnumerable<string> lines)
    {
        return lines.Select(ParseLine);
    }

    private static (Cpu.Opcode Opcode, int Arg) ParseLine(string line)
    {
        var elements = line.Split(InstructionDelimiter);
        return elements.Length == 1 ? 
            (Cpu.Opcode.Noop, 0) : 
            (Cpu.Opcode.Addx, int.Parse(elements[1]));
    }
}