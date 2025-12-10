using Utilities.Extensions;

namespace Solutions.Y2025.D10;

public sealed record Machine(string Lights, List<int[]> Buttons, int[] Joltage)
{
    public int LightMask { get; } = Lights
        .Select((c, i) => c == '#' ? 1 << i : 0)
        .Sum();
    
    public int[] ButtonMasks { get; } = Buttons
        .Select(button => button.Sum(i => 1 << i))
        .ToArray();

    public static Machine Parse(string line)
    {
        var parts = line.Split(' ');
        return new Machine(
            Lights:  parts[0][1..^1], 
            Buttons: parts[1..^1].Select(chunk => chunk.ParseInts()).ToList(), 
            Joltage: parts[^1].ParseInts());
    }

    public override string ToString()
    {
        return
            $"[{Lights}] " +
            $"({string.Join(") (", Buttons.Select(indices => string.Join(',', indices)))}) " +
            $"{{{string.Join(',', Joltage)}}}";
    }
}