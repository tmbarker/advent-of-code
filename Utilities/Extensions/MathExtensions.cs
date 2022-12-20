namespace Utilities.Extensions;

public static class MathExtensions
{
    public static int Modulo(this int a, int modulus)
    {
        return (a % modulus + modulus) % modulus;
    }
    
    public static long Modulo(this long a, long modulus)
    {
        return (a % modulus + modulus) % modulus;
    }
}