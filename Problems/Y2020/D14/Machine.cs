using System.Text.RegularExpressions;

namespace Problems.Y2020.D14;

public static class Machine
{
    private static readonly Regex MskRegex = new(pattern: @"mask = (?<msk>[01X]+)");
    private static readonly Regex MemRegex = new(pattern: @"mem\[(?<adr>\d+)\] = (?<val>\d+)");
    
    public static ulong RunV1(IEnumerable<string> program)
    {
        var mem = new Dictionary<ulong, ulong>();
        var msk = new MaskSimple();

        foreach (var line in program)
        {
            var mskMatch = MskRegex.Match(line);
            if (mskMatch.Success)
            {
                msk = new MaskSimple(mskMatch.Groups["msk"].Value);
                continue;
            }

            var memMatch = MemRegex.Match(line);
            var adr = ulong.Parse(memMatch.Groups["adr"].Value);
            var val = ulong.Parse(memMatch.Groups["val"].Value);

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
            var mskMatch = MskRegex.Match(line);
            if (mskMatch.Success)
            {
                msk = new MaskFloating(mskMatch.Groups["msk"].Value);
                continue;
            }

            var memMatch = MemRegex.Match(line);
            var adr = ulong.Parse(memMatch.Groups["adr"].Value);
            var val = ulong.Parse(memMatch.Groups["val"].Value);

            foreach (var mod in msk.Apply(adr))
            {
                mem[mod] = val;
            }
        }

        return SumMemorySpace(mem);
    }

    private static ulong SumMemorySpace(Dictionary<ulong, ulong> mem)
    {
        return mem.Values.Aggregate(seed: 0UL, (sum, value) => sum + value);
    }
}