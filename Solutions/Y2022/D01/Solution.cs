namespace Solutions.Y2022.D01;

[PuzzleInfo("Calorie Counting", Topics.None, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => GetMaxCalories(num: 1),
            2 => GetMaxCalories(num: 3),
            _ => PuzzleNotSolvedString
        };
    }

    private int GetMaxCalories(int num)
    {
        return ChunkInputByNonEmpty()
            .Select(chunk => chunk.Sum(int.Parse))
            .OrderDescending()
            .Take(num)
            .Sum();
    }
}