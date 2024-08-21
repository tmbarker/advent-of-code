namespace Solutions.Y2015.D20;

[PuzzleInfo("Infinite Elves and Infinite Houses", Topics.Math, Difficulty.Medium, favourite: true)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputText();
        var threshold = int.Parse(input);
        
        return part switch
        {
            1 => FindFirstHouse(threshold, factor: 10, limit: int.MaxValue),
            2 => FindFirstHouse(threshold, factor: 11, limit: 50),
            _ => PuzzleNotSolvedString
        };
    }

    private static int FindFirstHouse(int threshold, int factor, int limit)
    {
        //  Compute an upperbound of houses to compute, then cache the number of presents for all
        //  houses from 1 to upperbound. Use sieve-like method to compute sum of divisors:
        //
        var upperBound = threshold / factor + 1;
        var presentCounts = new int[upperBound + 1];

        for (var i = 1; i <= upperBound; i++)
        {
            var delivered = 0;
            for (var j = i; j <= upperBound && delivered < limit; j += i) 
            {
                presentCounts[j] += factor * i;
                delivered++;
            }   
        }

        for (var i = 1; i < presentCounts.Length; i++)
        {
            if (presentCounts[i] >= threshold)
            {
                return i;
            }
        }

        throw new NoSolutionException();
    }
}