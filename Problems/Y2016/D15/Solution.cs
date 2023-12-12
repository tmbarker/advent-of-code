using Utilities.Extensions;

namespace Problems.Y2016.D15;

[PuzzleInfo("Timing is Everything", Topics.Math, Difficulty.Easy, favourite: true)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => FindDelay(extraDisk: false),
            2 => FindDelay(extraDisk: true),
            _ => ProblemNotSolvedString
        };
    }

    private long FindDelay(bool extraDisk)
    {
        var input = GetInputLines();
        var disks = input.Select(ParseDisk).ToList();

        if (extraDisk)
        {
            var depth = disks.Max(disk => disk.Depth) + 1;
            var extra = new Disk(
                Depth: depth,
                Positions: 11,
                Initial: 0);
            
            disks.Add(extra);
        }
        
        var satisfied = false;
        var delay = 0;
        var step = disks.Min(disk => disk.Positions);

        while (!satisfied)
        {
            delay += step;
            satisfied = disks.All(disk => WillPassDisk(disk, delay));
        }

        return delay;
    }
    
    private static bool WillPassDisk(Disk disk, int delay)
    {
        var arriveAt = disk.Depth + delay;
        var position = (disk.Initial + arriveAt) % disk.Positions;

        return position == 0;
    }

    private static Disk ParseDisk(string line)
    {
        var numbers = line.ParseInts();
        return new Disk(
            Depth: numbers[0],
            Positions: numbers[1],
            Initial: numbers[3]);
    }
}