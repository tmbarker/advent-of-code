using Problems.Common;
using Utilities.Geometry.Euclidean;

namespace Problems.Y2021.D02;

/// <summary>
/// Dive: https://adventofcode.com/2021/day/2
/// </summary>
public sealed class Solution : SolutionBase
{
    private const string Forward = "forward";
    private const string Down = "down";
    private const string Up = "up";

    public override object Run(int part)
    {
        var instructions = ParseInstructions(GetInputLines());
        
        return part switch
        {
            1 => ComputeSimplePositionProduct(instructions),
            2 => ComputeAimedPositionProduct(instructions),
            _ => ProblemNotSolvedString
        };
    }

    private static int ComputeSimplePositionProduct(IEnumerable<(string Command, int Amount)> instructions)
    {
        var position = Vector2D.Zero;
        foreach (var instruction in instructions)
        {
            switch (instruction.Command)
            {
                case Forward:
                    position += instruction.Amount * Vector2D.Right;
                    break;
                case Down:
                    position += instruction.Amount * Vector2D.Down;
                    break;
                case Up:
                    position += instruction.Amount * Vector2D.Up;
                    break;
            }
        }

        return Math.Abs(position.X * position.Y);
    }
    
    private static int ComputeAimedPositionProduct(IEnumerable<(string Command, int Amount)> instructions)
    {
        var aim = 0;
        var position = Vector2D.Zero;
        
        foreach (var instruction in instructions)
        {
            switch (instruction.Command)
            {
                case Down:
                    aim += instruction.Amount;
                    break;
                case Up:
                    aim -= instruction.Amount;
                    break;
                case Forward:
                    position += instruction.Amount * Vector2D.Right;
                    position += instruction.Amount * aim * Vector2D.Down;
                    break;
            }
        }

        return Math.Abs(position.X * position.Y);
    }

    private static IEnumerable<(string Command, int Amount)> ParseInstructions(IEnumerable<string> input)
    {
        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (var line in input)
        {
            var elements = line.Split(' ');
            var command = elements[0];
            var value = int.Parse(elements[1]);

            yield return (command, value);
        }
    }
}