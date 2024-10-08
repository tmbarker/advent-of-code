using Solutions.Y2017.Common;
using Utilities.Extensions;

namespace Solutions.Y2017.D10;

[PuzzleInfo("Knot Hash", Topics.Hashing, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputText();
        return part switch
        {
            1 => HashSimple(input),
            2 => HashFull(input),
            _ => PuzzleNotSolvedString
        };
    }

    private static int HashSimple(string input)
    {
        var lengths = GetByteArray(input.ParseInts());
        var ring = GetByteArray(Enumerable.Range(0, 256));
        var result = KnotHash.TieKnots(ring, lengths, rounds: 1);

        return result[0] * result[1];
    }

    private static string HashFull(string input)
    {
        return KnotHash.ComputeHash(input);
    }

    private static byte[] GetByteArray(IEnumerable<int> ints)
    {
        return ints.Select(n => (byte)n).ToArray();
    }
}