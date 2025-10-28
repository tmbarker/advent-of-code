using System.Numerics;
using Utilities.Extensions;

namespace Utilities.Numerics;

public static class Numerics
{
    /// <summary>
    ///   Compute the Greatest Common Divisor of a and b.
    /// </summary>
    /// <returns>The GCD of a and b</returns>
    public static T Gcd<T>(T a, T b) where T : INumber<T>
    {
        a = T.Abs(a);
        b = T.Abs(b);
        
        if (a == T.Zero) return b;
        if (b == T.Zero) return a;
        
        while (b != T.Zero)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }

    /// <summary>
    ///   Extended Euclidean Algorithm to compute GCD and Bézout coefficients.
    ///   Finds x, y such that ax + by = gcd(a, b).
    /// </summary>
    /// <returns>The GCD of a and b</returns>
    public static (T Gcd, T X, T Y) ExtendedGcd<T>(T a, T b) where T : INumber<T>
    {
        var origA = a;
        var origB = b;
        
        a = T.Abs(a);
        b = T.Abs(b);
        
        if (a == T.Zero) return (Gcd: b, X: T.Zero, Y: T.One);
        if (b == T.Zero) return (Gcd: a, X: T.One,  Y: T.Zero);

        var (gcd, x1, y1) = ExtendedGcd(b % a, a);
        var x = y1 - b / a * x1;
        var y = x1;
        
        if (origA < T.Zero) x = -x;
        if (origB < T.Zero) y = -y;
        return (gcd, x, y);
    }
    
    /// <summary>
    ///   Compute the Least Common Multiple of <paramref name="a" /> and <paramref name="b" />.
    /// </summary>
    /// <returns>The LCM of <paramref name="a" /> and <paramref name="b" /></returns>
    public static T Lcm<T>(T a, T b) where T : INumber<T>
    {
        if (a == T.Zero || b == T.Zero)
        {
            return T.Zero;
        }
        
        var gcd = Gcd(a, b);
        
        //  NOTE: Use the identity: LCM(a,b) = |a*b| / GCD(a,b)
        //  Rewrite as LCM(a,b) = |a| / GCD(a,b) * |b| to avoid overflow
        //
        a = T.Abs(a);
        b = T.Abs(b);
        
        return a / gcd * b;
    }

    /// <summary>
    ///   Compute the Least Common Multiple of the provided <paramref name="numbers" />.
    /// </summary>
    /// <returns>The LCM of <paramref name="numbers" /></returns>
    public static T Lcm<T>(ICollection<T> numbers) where T : INumber<T>
    {
        if (numbers.Count == 0)
        {
            throw new ArgumentException("Cannot compute LCM of empty collection", nameof(numbers));
        }
            
        return numbers
            .Skip(1)
            .Aggregate(
                seed: numbers.First(),
                func: Lcm);
    }

    /// <summary>
    ///   Compute (a + b) mod m.
    /// </summary>
    /// <returns>The result of (a + b) mod m</returns>
    public static T ModAdd<T>(T a, T b, T m) where T : INumber<T>
    {
        return (a.Modulo(m) + b.Modulo(m)).Modulo(m);
    }
    
    /// <summary>
    ///   Compute (a * b) mod m using the Russian peasant method to avoid overflow.
    /// </summary>
    /// <returns>The result of (a * b) mod m</returns>
    public static T ModMultiply<T>(T a, T b, T m) where T : IBinaryNumber<T>, IShiftOperators<T, int, T>
    {
        a = a.Modulo(m);
        b = b.Modulo(m);
        var result = T.Zero;
     
        //  NOTE: Use the Russian peasant method for safe multiplication
        //
        while (b > T.Zero)
        {
            if ((b & T.One) == T.One)
            {
                result = ModAdd(result, a, m);
            }
            a = ModAdd(a, a, m);
            b >>= 1;
        }
        
        return result;
    }
    
    /// <summary>
    ///   Compute the modular inverse of a modulo m.
    /// </summary>
    /// <returns>The modular inverse of a modulo m</returns>
    public static T ModInverse<T>(T a, T m) where T : INumber<T>
    {
        var (gcd, x, _) = ExtendedGcd(a, m);
        if (gcd != T.One)
        {
            throw new InvalidOperationException($"Modular inverse of {a} modulo {m} does not exist (gcd = {gcd})");
        }
        
        return x.Modulo(m);
    }
}