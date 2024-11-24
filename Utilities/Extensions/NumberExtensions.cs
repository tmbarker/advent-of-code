using System.Numerics;

namespace Utilities.Extensions;

public static class NumberExtensions
{
    /// <summary>
    ///     Returns the signed remainder of the division:
    ///     <para>
    ///         <b><paramref name="a" />/<paramref name="modulus" /></b>
    ///     </para>
    /// </summary>
    /// <param name="a">The dividend</param>
    /// <param name="modulus">The divisor</param>
    /// <typeparam name="T">The type associated with <paramref name="a" /> and the <paramref name="modulus" /></typeparam>
    /// <returns>The signed remainder of the division</returns>
    /// <exception cref="DivideByZeroException">The provided <paramref name="modulus" /> is zero</exception>
    public static T Modulo<T>(this T a, T modulus) where T : INumber<T>
    {
        if (T.IsZero(modulus))
        {
            throw new DivideByZeroException();
        }
        
        return (a % modulus + modulus) % modulus;
    }
}