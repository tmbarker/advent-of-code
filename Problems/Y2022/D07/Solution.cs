using Problems.Y2022.Common;

namespace Problems.Y2022.D07;

/// <summary>
/// No Space Left On Device: https://adventofcode.com/2022/day/7
/// </summary>
public class Solution : SolutionBase2022
{
    private const int DirectorySizeThreshold = 100000;
    private const int SystemVolume = 70000000;
    private const int RequiredSpace = 30000000;
    
    public override int Day => 7;
    
    public override string Run(int part)
    {
        var consoleOutput = GetInput();
        var directorySizeIndex = ConsoleParser.ConstructDirectorySizeIndex(consoleOutput);

        return part switch
        {
            0 => SumDirectoriesUnderSize(directorySizeIndex, DirectorySizeThreshold).ToString(),
            1 => FreeUpSpace(directorySizeIndex, SystemVolume, RequiredSpace).ToString(),
            _ => ProblemNotSolvedString,
        };
    }

    private static int SumDirectoriesUnderSize(Dictionary<string, int> directorySizeIndex, int thresholdSize)
    {
        return directorySizeIndex
            .Values
            .Where(v => v <= thresholdSize)
            .Sum();
    }

    private static int FreeUpSpace(Dictionary<string, int> directorySizeIndex, int totalSystemSpace, int requiredSpace)
    {
        var freeSpace = totalSystemSpace - directorySizeIndex[ConsoleParser.RootDirectoryPath];
        var needed = requiredSpace - freeSpace;

        return directorySizeIndex.Values
            .Where(v => v >= needed)
            .Min();
    }
}