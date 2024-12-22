using System.Runtime.CompilerServices;

namespace Solutions.Y2024.D22;

[PuzzleInfo("Monkey Market", Topics.Math|Topics.BitwiseOperations, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var initial = ParseInputLines(parseFunc: long.Parse);
        var secrets = new long[initial.Length][];

        for (var i = 0; i < initial.Length; i++)
        {
            secrets[i] = new long[2000 + 1];
            secrets[i][0] = initial[i];

            for (var j = 1; j <= 2000; j++)
            {
                secrets[i][j] = Next(secrets[i][j - 1]);
            }
        }
        
        return part switch
        {
            1 => secrets.Sum(sequence => sequence[^1]),
            2 => MaximizePrice(secrets),
            _ => PuzzleNotSolvedString
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static long Next(long n)
    {
        n = MixPrune(n <<  6, n);
        n = MixPrune(n >>  5, n);
        n = MixPrune(n << 11, n);
        return n;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static long MixPrune(long a, long b)
    {
        return (a ^ b) & 0xFFFFFFL;
    }
    
    private static long MaximizePrice(long[][] secrets)
    {
        var prices = secrets.Select(sequence => sequence
                .Select(val => val % 10L)
                .ToArray())
            .ToArray();
        var changes = prices
            .Select(sequence => sequence
                .Skip(1)
                .Select((val, i) => val - sequence[i])
                .ToArray())
            .ToArray();
        
        //  Because all prices are in range [0, 9], all changes must be in range [-9,9]. This
        //  means each change is a tuple of four base-19 numbers. 19^4 = 130321.
        //
        var buyers = new long[130321];
        var totals = new long[130321];

        for (var b = 0; b < changes.Length; b++)
        {
            Array.Clear(buyers);
            for (var i = 0; i < changes[b].Length - 4; i++)
            {
                var del = changes[b];
                var key = Hash(a: del[i + 0], b: del[i + 1], c: del[i + 2], d: del[i + 3]);
            
                if (buyers[key] == 0L)
                {
                    totals[key] += prices[b][i + 4];
                    buyers[key] = 1L;
                }
            }
        }

        return totals.Max();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static long Hash(long a, long b, long c, long d)
    {
        return (((a + 9) * 19 + b + 9) * 19 + c + 9) * 19 + d + 9;
    }
}