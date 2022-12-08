using Problems.Y2022.Common;

namespace Problems.Y2022.D07;

/// <summary>
/// No Space Left On Device: https://adventofcode.com/2022/day/7
/// </summary>
public class Solution : SolutionBase2022
{
    private const int Part1DirectorySizeThreshold = 100000;
    private const int Part2FileSystemSpace = 70000000;
    private const int Part2RequiredSpace = 30000000;
    
    public override int Day => 7;
    public override int Parts => 2;
    
    public override string Run(int part)
    {
        AssertInputExists();

        var consoleOutput = File.ReadAllLines(GetInputFilePath());
        var fileSystem = ElfFileSystem.Construct(consoleOutput);

        return part switch
        {
            0 => SumDirectoriesUnderSize(fileSystem, Part1DirectorySizeThreshold).ToString(),
            1 => FreeUpSpace(fileSystem, Part2FileSystemSpace, Part2RequiredSpace).ToString(),
            _ => ProblemNotSolvedString,
        };
    }

    private static int SumDirectoriesUnderSize(ElfFileSystem fileSystem, int thresholdSize)
    {
        return fileSystem.BuildDirectorySizeIndex()
            .Values
            .Where(v => v <= thresholdSize)
            .Sum();
    }

    private static int FreeUpSpace(ElfFileSystem fileSystem, int totalSystemSpace, int requiredSpace)
    {
        var sizeIndex = fileSystem.BuildDirectorySizeIndex();
        var freeSpace = totalSystemSpace - sizeIndex[fileSystem.RootDirectory.GetPath()];
        var additionalSpaceNeeded = requiredSpace - freeSpace;

        var freedSpace = sizeIndex.Values
            .Where(v => v >= additionalSpaceNeeded)
            .Min();

        return freedSpace;
    }
}