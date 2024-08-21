namespace Solutions.Y2018.D02;

[PuzzleInfo("Inventory Management System", Topics.StringParsing, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => GetChecksum(ids: GetInputLines(), c1: 2, c2: 3),
            2 => GetCommonCorrectLetters(ids: GetInputLines()),
            _ => PuzzleNotSolvedString
        };
    }

    private static int GetChecksum(IList<string> ids, int c1, int c2)
    {
        return
            ids.Count(id => HasExactCountOfAnyLetter(id, c1)) *
            ids.Count(id => HasExactCountOfAnyLetter(id, c2));
    }

    private static string GetCommonCorrectLetters(IList<string> ids)
    {
        var numIds = ids.Count;
        var idLength = ids[0].Length;
        
        for (var i = 0; i < numIds; i++)
        for (var j = 0; j < numIds; j++)
        {
            var z = ZipCommon(ids[i], ids[j]);
            if (z.Length == idLength - 1)
            {
                return z;
            }
        }

        throw new NoSolutionException();
    }

    private static string ZipCommon(string a, string b)
    {
        return string.Join(string.Empty, a.Zip(b, (c1, c2) => c1 == c2 ? c1.ToString() : string.Empty));
    }

    private static bool HasExactCountOfAnyLetter(string id, int count)
    {
        return id.Any(c => id.Count(e => e == c) == count);
    }
}