using Problems.Y2020.Common;

namespace Problems.Y2020.D25;

/// <summary>
/// Combo Breaker: https://adventofcode.com/2020/day/25
/// </summary>
public class Solution : SolutionBase2020
{
    private const long DeviceSubject = 7L;
    private const long Mod = 20201227L;
    
    public override int Day => 25;
    public override int Parts => 1;

    public override object Run(int part)
    {
        return part switch
        {
            0 => CrackEncryption(ParsePublicKeys(GetInputLines())),
            _ => ProblemNotSolvedString,
        };
    }

    private static long CrackEncryption((long Key1, long Key2) publicKeys)
    {
        return Transform(publicKeys.Key2, FindLoopSize(DeviceSubject, publicKeys.Key1));
    }
    
    private static int FindLoopSize(long subject, long pubKey)
    {
        var loops = 0;
        var value = 1L;

        while (value != pubKey)
        {
            value = Loop(subject, value);
            loops++;
        }

        return loops;
    }

    private static long Transform(long subject, int numLoops)
    {
        var value = 1L;
        for (var i = 0; i < numLoops; i++)
        {
            value = Loop(subject, value);
        }
        return value;
    }
    
    private static long Loop(long subject, long value)
    {
        return value * subject % Mod;
    }
    
    private static (long Key1, long Key2) ParsePublicKeys(IList<string> input)
    {
        return (long.Parse(input[0]), long.Parse(input[1]));
    }
}