namespace Solutions.Y2020.D09;

[PuzzleInfo("Encoding Error", Topics.Math, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private const int PreambleLength = 25;
    
    public override object Run(int part)
    {
        var numbers = ParseInputLines(parseFunc: long.Parse);
        return part switch
        {
            1 => FindWeakness(numbers),
            2 => ExploitWeakness(numbers),
            _ => PuzzleNotSolvedString
        };
    }

    private static long FindWeakness(IList<long> numbers)
    {
        var window = new Queue<long>(numbers.Take(PreambleLength));
        for (var i = PreambleLength; i < numbers.Count; i++)
        {
            if (!TwoSumExists(numbers[i], new HashSet<long>(window)))
            {
                return numbers[i];
            }

            window.Dequeue();
            window.Enqueue(numbers[i]);
        }

        throw new NoSolutionException();
    }

    private static long ExploitWeakness(IList<long> numbers)
    {
        var weakness = FindWeakness(numbers);
        var trailingSums = new List<Sum>();

        foreach (var number in numbers)
        {
            for (var j = trailingSums.Count - 1; j >= 0; j--)
            {
                if (trailingSums[j].Add(number) == weakness)
                {
                    return trailingSums[j].GetExtremaSum();
                }
                
                if (trailingSums[j].Value > weakness)
                {
                    trailingSums.RemoveAt(j);
                }
            }

            trailingSums.Add(new Sum(number));
        }

        throw new NoSolutionException();
    }
    
    private static bool TwoSumExists(long number, IReadOnlySet<long> window)
    {
        return window.Select(n1 => number - n1).Any(window.Contains);
    }
}