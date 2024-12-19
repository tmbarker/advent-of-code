namespace Solutions.Y2024.D19;

[PuzzleInfo("Linen Layout", Topics.Recursion, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var lines = GetInputLines();
        var patterns = lines[0].Split(", ");
        var designs = lines[2..];
        var memo = new Dictionary<string, long> { [string.Empty] = 1L };
        
        return part switch
        {
            1 => designs.Count(design => Permute(design, patterns, memo) > 0),
            2 => designs.Sum(design => Permute(design, patterns, memo)),
            _ => PuzzleNotSolvedString
        };
    }
    
    private static long Permute(string design, string[] patterns, Dictionary<string, long> memo)
    {
        if (memo.TryGetValue(design, out var value))
        {
            return value;
        }
        
        memo[design] = patterns
            .Where(design.StartsWith)
            .Sum(pattern => Permute(design[pattern.Length..], patterns, memo));
        return memo[design];
    }
}