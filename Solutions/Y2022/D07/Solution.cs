namespace Solutions.Y2022.D07;

[PuzzleInfo("No Space Left On Device", Topics.StringParsing, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var consoleOutput = GetInputLines();
        var consoleParser = new ConsoleParser();
        var sizeIndex = consoleParser.BuildDirectoryMap(consoleOutput);

        return part switch
        {
            1 => SumDirectoriesUnderSize(sizeIndex, thresholdSize: 100000),
            2 => FreeUpSpace(sizeIndex, systemVolume: 70000000, requiredSpace: 30000000),
            _ => PuzzleNotSolvedString
        };
    }

    private static int SumDirectoriesUnderSize(IDictionary<string, int> sizeIndex, int thresholdSize)
    {
        return sizeIndex.Values
            .Where(v => v <= thresholdSize)
            .Sum();
    }

    private static int FreeUpSpace(IDictionary<string, int> directorySizeIndex, int systemVolume, int requiredSpace)
    {
        var freeSpace = systemVolume - directorySizeIndex[ConsoleParser.RootDirectoryPath];
        var needed = requiredSpace - freeSpace;

        return directorySizeIndex.Values
            .Where(v => v >= needed)
            .Min();
    }
}