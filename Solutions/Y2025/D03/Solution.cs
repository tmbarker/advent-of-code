using Utilities.Extensions;

namespace Solutions.Y2025.D03;

[PuzzleInfo("Lobby", Topics.Math, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var take = part == 1 ? 2 : 12;
        return GetInputLines()
            .Select(line => line
                .Select(c => (long)c.AsDigit())
                .ToList())
            .Select(bank => Maximize(bank, power: 0L, remaining: take, startIndex: 0))
            .Sum();
    }

    private static long Maximize(List<long> bank, long power, int remaining, int startIndex)
    {
        while (true)
        {
            if (remaining <= 0 || startIndex >= bank.Count)
            {
                return power;
            }

            var maxVal = 0L;
            var maxIdx = -1;

            for (var i = startIndex; i <= bank.Count - remaining; i++)
            {
                if (bank[i] > maxVal)
                {
                    maxVal = bank[i];
                    maxIdx = i;
                }
            }

            power = 10L * power + maxVal;
            remaining -= 1;
            startIndex = maxIdx + 1;
        }
    }
}