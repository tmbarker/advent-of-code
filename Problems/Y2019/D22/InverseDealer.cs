using Problems.Common;
using Utilities.Extensions;

namespace Problems.Y2019.D22;

public class InverseDealer
{
    private readonly long _deckSize;
    
    public InverseDealer(long deckSize)
    {
        _deckSize = deckSize;
    }
    
    public long UndoStack(long result)
    {
        return _deckSize - 1 - result;
    }
    
    public long UndoCut(long result, long amount)
    {
        return (result + amount).Modulo(_deckSize);
    }
    
    public long UndoIncrement(long result, long amount)
    {
        return (result * ModularInverse(amount, _deckSize)).Modulo(_deckSize);
    }

    /// <summary>
    /// Execute the Extended Euclidean Algorithm (EEA) to obtain the modular multiplicative inverse of <paramref name="a"/>
    /// mod <paramref name="n"/>, such that <paramref name="a"/>*t is congruent to 1 mod <paramref name="n"/>
    /// </summary>
    /// <param name="a">The value to invert</param>
    /// <param name="n">The modulus</param>
    /// <returns>The modular multiplicative inverse of <paramref name="a"/> mod <paramref name="n"/></returns>
    /// <exception cref="NoSolutionException"><paramref name="a"/> is not invertible</exception>
    private static long ModularInverse(long a, long n)
    {
        long t = 0L, newt = 1L;
        long r = n, newr = a;

        while (newr != 0)
        {
            var quotient = r / newr;
            (t, newt) = (newt, t - quotient * newt);
            (r, newr) = (newr, r - quotient * newr);
        }

        if (r > 1)
        {
            throw new NoSolutionException();
        }

        if (t < 0)
        {
            t += n;
        }

        return t;
    }
}