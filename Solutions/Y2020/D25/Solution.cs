namespace Solutions.Y2020.D25;

[PuzzleInfo("Combo Breaker", Topics.Math, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private const long DeviceSubject = 7L;
    private const long Mod = 20201227L;
    
    public override int Parts => 1;

    public override object Run(int part)
    {
        return part switch
        {
            1 => CrackEncryption(ParsePublicKeys(GetInputLines())),
            _ => PuzzleNotSolvedString
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
    
    private static (long Key1, long Key2) ParsePublicKeys(string[] input)
    {
        return (long.Parse(input[0]), long.Parse(input[1]));
    }
}