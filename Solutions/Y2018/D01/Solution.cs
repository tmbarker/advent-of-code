namespace Solutions.Y2018.D01;

[PuzzleInfo("Chronal Calibration", Topics.Math, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var numbers = ParseInputLines(int.Parse);
        return part switch
        {
            1 => numbers.Sum(),
            2 => GetFirstRepeatedFrequency(numbers),
            _ => PuzzleNotSolvedString
        };
    }

    private static int GetFirstRepeatedFrequency(int[] numbers)
    {
        var i = 0;
        var freq = 0;
        var seen = new HashSet<int>();

        while (seen.Add(freq))
        {
            freq += numbers[i];
            i = (i + 1) % numbers.Length;
        }

        return freq;
    }
}