using System.Numerics;

namespace Utilities.Numerics;

public static class Numerics
{
    /// <summary>
    /// Compute the Greatest Common Divisor of <paramref name="a"/> and <paramref name="b"/>
    /// </summary>
    /// <returns>The GCD of <paramref name="a"/> and <paramref name="b"/></returns>
    public static T Gcd<T>(T a, T b) where T : INumber<T>
    {
        while (a != T.Zero && b != T.Zero)
        {
            if (a > b)
            {
                a %= b;
            }
            else
            {
                b %= a;
            }
        }

        return a + b;
    }

    /// <summary>
    /// Compute the Least Common Multiple of <paramref name="a"/> and <paramref name="b"/>
    /// </summary>
    /// <returns>The LCM of <paramref name="a"/> and <paramref name="b"/></returns>
    public static T Lcm<T>(T a, T b) where T : INumber<T>
    {
        return a * b / Gcd(a, b);
    }

    /// <summary>
    /// Compute the Least Common Multiple of the provided <paramref name="numbers"/>
    /// </summary>
    /// <returns>The LCM of <paramref name="numbers"/></returns>
    public static T Lcm<T>(ICollection<T> numbers) where T : INumber<T>
    {
        return numbers
            .Skip(1)
            .Aggregate(
                seed: numbers.First(),
                func: Lcm);
    }
}