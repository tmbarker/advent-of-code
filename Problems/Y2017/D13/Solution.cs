using Problems.Y2017.Common;
using Utilities.Extensions;

namespace Problems.Y2017.D13;

/// <summary>
/// Packet Scanners: https://adventofcode.com/2017/day/13
/// </summary>
public class Solution : SolutionBase2017
{
    public override int Day => 13;
    
    public override object Run(int part)
    {
        var input = GetInputLines();
        var scanners = new List<Scanner>(input.Select(ParseScanner));

        return part switch
        {
            1 => GetSeverity(scanners, delay: 0),
            2 => FindDelay(scanners),
            _ => ProblemNotSolvedString
        };
    }

    private static int FindDelay(IList<Scanner> scanners)
    {
        var delay = 0;
        while (scanners.Any(s => IsCaught(scanner: s, time: s.Depth + delay)))
        {
            delay++;
        }
        return delay;
    }
    
    private static int GetSeverity(IEnumerable<Scanner> scanners, int delay)
    {
        return scanners
            .Where(s => IsCaught(scanner: s, time: s.Depth + delay))
            .Sum(s => s.Severtiy);
    }

    private static bool IsCaught(Scanner scanner, int time)
    {
        return time % (scanner.Range - 1) == 0 && time / (scanner.Range - 1) % 2 == 0;
    }

    private static Scanner ParseScanner(string line)
    {
        var numbers = line.ParseInts();
        return new Scanner(
            depth: numbers[0],
            range: numbers[1]);
    }
}