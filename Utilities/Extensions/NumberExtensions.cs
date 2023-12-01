using System.Numerics;

namespace Utilities.Extensions;

public static class NumberExtensions
{
    public static int Modulo(this int a, int modulus)
    {
        return (a % modulus + modulus) % modulus;
    }
    
    public static long Modulo(this long a, long modulus)
    {
        return (a % modulus + modulus) % modulus;
    }

    public static double Modulo(this double a, double modulus)
    {
        return (a % modulus + modulus) % modulus;
    }
    
    public static BigInteger Modulo(this BigInteger a, BigInteger modulus)
    {
        return (a % modulus + modulus) % modulus;
    }
}