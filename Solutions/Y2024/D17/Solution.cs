using System.Numerics;
using Utilities.Extensions;

namespace Solutions.Y2024.D17;

[InputSpecificSolution]
[PuzzleInfo("Chronospatial Computer", Topics.Assembly, Difficulty.Hard, favourite: true)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();
        var program = input[^1].ParseInts();
        
        return part switch
        {
            1 => Execute(program, a: input[0].ParseNumber<BigInteger>()),
            2 => Reverse(program),
            _ => PuzzleNotSolvedString
        };
    }

    private static string Execute(int[] program, BigInteger a)
    {
        return string.Join(',', Vm.Run(program, a));
    }

    private static BigInteger Reverse(int[] program)
    {
        var queue = new Queue<(BigInteger val, int idx)>([(val: BigInteger.Zero, idx: 1)]);
        while (queue.Count != 0)
        {
            var (value, idx) = queue.Dequeue();
            for (var candidate = value; candidate <= value + 0b111; candidate++)
            {
                var need = program[^idx..];
                var have = Vm.Simulate(candidate);

                if (!need.SequenceEqual(have)) continue;
                if (idx == program.Length) return candidate;

                queue.Enqueue((candidate << 3, idx + 1));
            }
        }

        throw new NoSolutionException("Could not construct a solution");
    }
}