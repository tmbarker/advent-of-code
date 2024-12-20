namespace Solutions.Y2016.D16;

[PuzzleInfo("Dragon Checksum", Topics.BitwiseOperations, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputText();
        var data = input.Select(c => c == '1').ToList();
        
        return part switch
        {
            1 => Validate(data, length: 272),
            2 => Validate(data, length: 35651584),
            _ => PuzzleNotSolvedString
        };
    }

    private static string Validate(List<bool> data, int length)
    {
        while (data.Count < length)
        {
            data = Generate(data);
        }

        var trimmed = data.Take(length).ToList();
        var checksum = Checksum(trimmed);
        
        while (checksum.Count % 2 == 0)
        {
            checksum = Checksum(checksum);
        }

        return string.Concat(checksum.Select(b => b ? '1' : '0'));
    }
    
    private static List<bool> Generate(IList<bool> data)
    {
        var b = data
            .Reverse()
            .Select(bit => !bit);

        return data
            .Append(false)
            .Concat(b)
            .ToList();
    }

    private static List<bool> Checksum(List<bool> data)
    {
        var checksum = new List<bool>();
        for (var i = 0; i < data.Count; i += 2)
        {
            checksum.Add(!(data[i] ^ data[i + 1]));
        }

        return checksum;
    }
}