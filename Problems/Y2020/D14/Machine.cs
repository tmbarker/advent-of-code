using System.Text.RegularExpressions;

namespace Problems.Y2020.D14;

public static class Machine
{
    private const string MskRegex = @"mask = ([X01]+)";
    private const string MemRegex = @"mem\[(\d+)\] = (\d+)";
    
    public static ulong RunV1(IEnumerable<string> program)
    {
        var mem = new Dictionary<ulong, ulong>();
        var msk = new MaskSimple();

        foreach (var line in program)
        {
            var maskMatch = Regex.Match(line, MskRegex);
            if (maskMatch.Success)
            {
                msk = new MaskSimple(maskMatch.Groups[1].Value);
                continue;
            }

            var memMatch = Regex.Match(line, MemRegex);
            var adr = ulong.Parse(memMatch.Groups[1].Value);
            var val = ulong.Parse(memMatch.Groups[2].Value);
            
            mem[adr] = msk.Apply(val);
        }

        return SumMemorySpace(mem);
    }
    
    public static ulong RunV2(IEnumerable<string> program)
    {
        var mem = new Dictionary<ulong, ulong>();
        var msk = new MaskFloating();

        foreach (var line in program)
        {
            var maskMatch = Regex.Match(line, MskRegex);
            if (maskMatch.Success)
            {
                msk = new MaskFloating(maskMatch.Groups[1].Value);
                continue;
            }

            var memMatch = Regex.Match(line, MemRegex);
            var adr = ulong.Parse(memMatch.Groups[1].Value);
            var val = ulong.Parse(memMatch.Groups[2].Value);

            foreach (var mod in msk.Apply(adr))
            {
                mem[mod] = val;
            }
        }

        return SumMemorySpace(mem);
    }

    private static ulong SumMemorySpace(Dictionary<ulong, ulong> mem)
    {
        return mem.Values.Aggregate(0UL, (sum, next) => sum + next);
    }
}