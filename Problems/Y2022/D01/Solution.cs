using Utilities.Extensions;

namespace Problems.Y2022.D01;

[PuzzleInfo("Calorie Counting", Topics.None, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => GetMaxCalories(num: 1),
            2 => GetMaxCalories(num: 3),
            _ => ProblemNotSolvedString
        };
    }

    private int GetMaxCalories(int num)
    {
        return GetInputLines()
            .ChunkBy(line => !string.IsNullOrWhiteSpace(line))
            .Select(chunk => chunk.Sum(int.Parse))
            .OrderDescending()
            .Take(num)
            .Sum();
    }
}