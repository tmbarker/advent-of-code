using Utilities.Extensions;

namespace Problems.Y2017.D15;

[PuzzleInfo("Dueling Generators", Topics.BitwiseOperations, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private const ulong FactorA = 16807UL;
    private const ulong FactorB = 48271UL;
    private const ulong Modulus = 2147483647UL;
    private const ulong Mask = 0xFFFFUL;
    
    private static readonly Predicate<ulong> True = _ => true;
    private static readonly Predicate<ulong> Div4 = n => n % 4UL == 0;
    private static readonly Predicate<ulong> Div8 = n => n % 8UL == 0;

    public override object Run(int part)
    {
        var input = GetInputLines();
        var seeds = ParseSeeds(input);

        return part switch
        {
            1 => Count(seeds.A, seeds.B, predA: True, predB: True, rounds: 40000000UL),
            2 => Count(seeds.A, seeds.B, predA: Div4, predB: Div8, rounds: 5000000UL),
            _ => ProblemNotSolvedString
        };
    }

    private static ulong Count(ulong seedA, ulong seedB, Predicate<ulong> predA, Predicate<ulong> predB, ulong rounds)
    {
        var count = 0UL;
        var a = seedA;
        var b = seedB;

        for (var i = 0UL; i < rounds; i++)
        {
            do
            {
                a = a * FactorA % Modulus;
            } while (!predA(a));

            do
            {
                b = b * FactorB % Modulus;
            } while (!predB(b));
            
            if ((a & Mask) == (b & Mask))
            {
                count++;
            }
        }

        return count;
    }

    private static (ulong A, ulong B) ParseSeeds(IList<string> input)
    {
        var seedA = (ulong)input[0].ParseInt();
        var seedB = (ulong)input[1].ParseInt();

        return (seedA, seedB);
    }
}