using Problems.Attributes;
using Problems.Common;
using Utilities.Collections;
using Utilities.Geometry.Euclidean;

namespace Problems.Y2023.D03;

/// <summary>
/// Gear Ratios: https://adventofcode.com/2023/day/3
/// </summary>
[Favourite("Gear Ratios", Topics.Vectors, Difficulty.Easy)]
public class Solution : SolutionBase
{
    private readonly record struct Number(int Value, HashSet<Vector2D> Positions);
    private readonly record struct Schematic(List<Number> Numbers, DefaultDictionary<char, HashSet<Vector2D>> Symbols);
    
    private const char Void = '.';
    private const char Gear = '*';
    
    public override object Run(int part)
    {
        var lines = GetInputLines();
        var schematic = BuildSchematic(lines);
        
        return part switch
        {
            1 => SumPartNumbers(schematic),
            2 => SumGearRatios(schematic),
            _ => ProblemNotSolvedString
        };
    }

    private static int SumPartNumbers(Schematic schematic)
    {
        var sum = 0;
        var symbolPositions = schematic.Symbols.Values
            .SelectMany(set => set)
            .ToHashSet();
        
        foreach (var number in schematic.Numbers)
        {
            var adj = number.Positions
                .SelectMany(pos => pos.GetAdjacentSet(Metric.Chebyshev))
                .ToHashSet();

            if (symbolPositions.Any(adj.Contains))
            {
                sum += number.Value;
            }
        }

        return sum;
    }

    private static int SumGearRatios(Schematic schematic)
    {
        var gearPositions = schematic.Symbols[Gear];
        var sum = 0;

        foreach (var pos in gearPositions)
        {
            var adjPos = pos.GetAdjacentSet(Metric.Chebyshev);
            var adjNum = schematic.Numbers
                .Where(num => num.Positions.Any(adjPos.Contains))
                .ToList();

            if (adjNum.Count == 2)
            {
                sum += adjNum[0].Value * adjNum[1].Value;
            }
        }

        return sum;
    }
    
    private static Schematic BuildSchematic(IReadOnlyList<string> lines)
    {
        var symbols = new DefaultDictionary<char, HashSet<Vector2D>>(defaultValue: new HashSet<Vector2D>());
        var numbers = new List<Number>();

        for (var y = 0; y < lines.Count; y++)
        for (var x = 0; x < lines[0].Length; x++)
        {
            if (lines[y][x] == Void)
            {
                continue;
            }
            
            if (!char.IsDigit(lines[y][x]))
            {
                symbols[lines[y][x]].Add(item: new Vector2D(x, y));
                continue;
            }
            
            var positions = new HashSet<Vector2D> { new(x, y) };
            var span = 1;

            while (x + span < lines[0].Length && char.IsDigit(lines[y][x + span]))
            {
                positions.Add(item: new Vector2D(x: x + span, y));
                span++;
            }

            var value = int.Parse(lines[y][x..(x + span)]);
            var number = new Number(value, positions);

            numbers.Add(number);
            x += span - 1;
        }

        return new Schematic(numbers, symbols);
    }
}