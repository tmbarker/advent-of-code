using Problems.Common;

namespace Problems.Y2022.D07;

/// <summary>
/// No Space Left On Device: https://adventofcode.com/2022/day/7
/// </summary>
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var consoleOutput = GetInputLines();
        var sizeIndex = ConsoleParser.ConstructDirectorySizeIndex(consoleOutput);

        return part switch
        {
            1 => SumDirectoriesUnderSize(sizeIndex, thresholdSize: 100000),
            2 => FreeUpSpace(sizeIndex, systemVolume: 70000000, requiredSpace: 30000000),
            _ => ProblemNotSolvedString
        };
    }

    private static int SumDirectoriesUnderSize(Dictionary<string, int> sizeIndex, int thresholdSize)
    {
        return sizeIndex.Values
            .Where(v => v <= thresholdSize)
            .Sum();
    }

    private static int FreeUpSpace(Dictionary<string, int> directorySizeIndex, int systemVolume, int requiredSpace)
    {
        var freeSpace = systemVolume - directorySizeIndex[ConsoleParser.RootDirectoryPath];
        var needed = requiredSpace - freeSpace;

        return directorySizeIndex.Values
            .Where(v => v >= needed)
            .Min();
    }
}