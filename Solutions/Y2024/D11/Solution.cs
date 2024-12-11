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
        var memo= new Dictionary<(string, int), long>();
        return GetInputText()
            .Split(' ')
            .Sum(stone => GetTotal(s: stone, b: blinks, m: memo));
    }
    
    private static long GetTotal(string s, int b, Dictionary<(string, int), long> m)
    {
        if (b == 0)
        {
            return 1;
        }

        var k = (s, b--);
        var n = s.Length / 2;
        
        if (m.TryGetValue(k, out var cached))
        {
            return cached;
        }
        
        if (s is "" or "0")
        {
            m[k] = GetTotal(s: "1", b, m);
            return m[k];
        }

        if (s.Length % 2 == 0)
        {
            m[k] = GetTotal(s: s[..n],                b, m) + 
                   GetTotal(s: s[n..].TrimStart('0'), b, m);
            return m[k];
        }

        m[k] = GetTotal(s: $"{2024L * long.Parse(s)}", b, m);
        return m[k];
    }
}