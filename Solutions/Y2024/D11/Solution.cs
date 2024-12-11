using Utilities.Extensions;

namespace Solutions.Y2024.D11;

[PuzzleInfo("Plutonian Pebbles", Topics.Recursion|Topics.Simulation, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => CountStones(blinks: 25),
            2 => CountStones(blinks: 75),
            _ => PuzzleNotSolvedString
        };
    }
    
    private long CountStones(int blinks)
    {
        var input = GetInputText();
        var memo= new Dictionary<(string, int), long>();

        return input
            .Split(' ')
            .Sum(stone => GetTotal(stone, blinks, memo));
    }
    
    private static long GetTotal(string stone, int blinks, Dictionary<(string, int), long> memo)
    {
        if (blinks == 0)
        {
            return 1;
        }
        
        var key = (stone, blinks);
        if (memo.TryGetValue(key, out var cached))
        {
            return cached;
        }

        if (stone.ParseLong() == 0L)
        {
            memo[key] = GetTotal(stone: "1", blinks - 1, memo);
        } 
        else if (stone.Length % 2 == 0)
        {
            var n = stone.Length / 2;
            var a = stone[..n];
            var b = stone[n..].TrimStart('0');
            if (string.IsNullOrEmpty(b))
            {
                b = "0";
            }
            
            memo[key] = GetTotal(stone: a, blinks - 1, memo) +
                        GetTotal(stone: b, blinks - 1, memo);
        }
        else
        {
            var next = 2024L * stone.ParseLong();
            memo[key] = GetTotal(stone: next.ToString(), blinks: blinks - 1, memo);    
        }
        
        return memo[key];
    }
}