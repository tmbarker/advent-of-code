using System.Diagnostics.CodeAnalysis;
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

    private const int CrtHeight = 6;
    private const int CrtWidth = 40;

    private const string Part2Result = "Read the characters from the console!";

    private static readonly HashSet<int> SampleCycles = new()
    {
        20,
        60,
        100,
        140,
        180,
        220,
    };
    private static readonly Dictionary<Instruction, int> CycleMap = new()
    {
        { Instruction.addx, 2 },
        { Instruction.noop, 1 },
    };

    public override int Day => 10;
    
    public override string Run(int part)
    {
        AssertInputExists();

        return part switch
        {
            0 => CalculateSignalStrength(ParseInstructions(GetInput())).ToString(),
            1 => Part2Result,
            _ => ProblemNotSolvedString,
        };
    }

    private static int CalculateSignalStrength(IEnumerable<(Instruction instruction, int arg)> instructions)
    {
        var register = 1;
        var cycle = 0;
        var signalStrength = 0;

        foreach (var instruction in instructions)
        {
            var numCycles = CycleMap[instruction.instruction];
            for (var i = 0; i < numCycles; i++)
            {
                cycle++;
                if (SampleCycles.Contains(cycle))
                {
                    signalStrength += (cycle * register);
                }
                
                var pixelX = cycle % CrtWidth;
                var spriteVisible = (pixelX == register || pixelX == (register + 1) || pixelX == (register - 1));

                Console.Write(spriteVisible ? LitPixel : DarkPixel);
                if (cycle % CrtWidth == 0)
                {
                    Console.WriteLine();
                }
            }

            if (instruction.instruction == Instruction.addx)
            {
                register += instruction.arg;
            }
        }

        return signalStrength;
    }

    private IEnumerable<string> GetInput()
    {
        return File.ReadAllLines(GetInputFilePath());
    }

    private static IEnumerable<(Instruction instruction, int arg)> ParseInstructions(IEnumerable<string> lines)
    {
        return lines.Select(ParseLine);
    }

    private static (Instruction Instruction, int Arg) ParseLine(string line)
    {
        var elements = line.Split(InstructionDelimiter);
        return elements.Length == 1 ? 
            (Instruction.noop, 0) : 
            (Instruction.addx, int.Parse(elements[1]));

    }
}

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum Instruction
{
    // ReSharper disable once IdentifierTypo
    addx = 0,
    noop = 1,
}