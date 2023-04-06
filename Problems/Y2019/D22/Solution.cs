using System.Numerics;
using System.Text.RegularExpressions;
using Problems.Attributes;
using Problems.Common;
using Problems.Y2019.Common;
using Utilities.Extensions;

namespace Problems.Y2019.D22;

/// <summary>
/// Slam Shuffle: https://adventofcode.com/2019/day/22
/// </summary>
[Favourite("Slam Shuffle", Topics.Math, Difficulty.Hard)]
public class Solution : SolutionBase2019
{
    private const string Stack = "stack";
    private const string Cut = "cut";
    private const string Increment = "increment";

    private static readonly Regex NumberRegex = new(@"(-?\d+)");
    
    public override int Day => 22;
    
    public override object Run(int part)
    {
        return part switch
        {
            1 => FollowCard(card: 2019L, deckSize: 10007L),
            2 => TraceCard(index: 2020L, deckSize: 119315717514047L, numShuffles: 101741582076661L),
            _ => ProblemNotSolvedString
        };
    }

    private long FollowCard(long card, long deckSize)
    {
        var steps = GetInputLines();
        var index = card;

        foreach (var step in steps)
        {
            var amount = ParseArg(step);
            index = step switch
            {
                not null when step.Contains(Stack) => deckSize - index - 1,
                not null when step.Contains(Cut) => index - amount,
                not null when step.Contains(Increment) => index * amount,
                _ => throw new NoSolutionException()
            };

            index = index.Modulo(deckSize);
        }
        
        return index;
    }

    private long TraceCard(long index, long deckSize, long numShuffles)
    {
        var steps = GetInputLines();
        var reversedSteps = new List<string>(steps.Reverse());

        var m = new BigInteger(1);
        var b = new BigInteger(0);
        var d = new BigInteger(deckSize);

        foreach (var step in reversedSteps)
        {
            var arg = ParseArg(step);
            var amount = new BigInteger(arg);
            
            switch (step)
            {
                case not null when step.Contains(Stack):
                    m = -m;
                    b = -b - 1;
                    break;
                case not null when step.Contains(Cut):
                    b += amount;
                    break;
                case not null when step.Contains(Increment):
                    var inv = ModInverse(a: amount, modulus: d);
                    m *= inv;
                    b *= inv;
                    break;
            }

            b = b.Modulo(d);
            m = m.Modulo(d);
        }
        
        // exponentiation for m, geometric series partial sum for b
        //
        b = ModGeometricSum(a: b, r: m, exponent: numShuffles, modulus: d);
        m = BigInteger.ModPow(value: m, exponent: numShuffles, modulus: d);

        var card = (m * index + b).Modulo(d);
        var asLong = (long)card;

        return asLong;
    }
    
    /// <summary>
    /// Execute the Extended Euclidean Algorithm (EEA) to obtain the modular multiplicative inverse of a mod m
    /// </summary>
    private static BigInteger ModInverse(BigInteger a, BigInteger modulus)
    {
        BigInteger t = new (0), newt = new (1);
        BigInteger r = modulus, newr = a;

        while (newr != 0)
        {
            var quotient = r / newr;
            (t, newt) = (newt, t - quotient * newt);
            (r, newr) = (newr, r - quotient * newr);
        }

        if (r > 1)
        {
            throw new NoSolutionException(message: $"{a} is not invertible mod {modulus}");
        }

        if (t < 0)
        {
            t += modulus;
        }

        return t;
    }

    private static BigInteger ModGeometricSum(BigInteger a, BigInteger r, BigInteger exponent, BigInteger modulus)
    {
        // The partial sum of a geometric series is given by: s = a * (1 - r^n) / (1 - r)
        // Under modulo m, this becomes: s = a * (1 - r^n) * ModInv(1 - r) mod m
        //
        var numerator = a * (1 - BigInteger.ModPow(value: r, exponent: exponent, modulus: modulus));
        var invDenominator = ModInverse(a: 1 - r, modulus: modulus);
        
        // Note, using the modular multiplicative inverse, we now multiply the numerator and the
        // inverted denominator
        //
        var partialSum = numerator * invDenominator;
        var inRange = partialSum.Modulo(modulus);

        return inRange;
    }
    
    private static long ParseArg(string line)
    {
        var match = NumberRegex.Match(line);
        return match.Success
            ? long.Parse(match.Groups[0].Value)
            : 0L;
    }
}