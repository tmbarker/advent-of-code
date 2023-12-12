using Utilities.Extensions;

namespace Solutions.Y2023.D05;

public sealed class Almanac
{
    public IReadOnlyList<long> Seeds { get; }
    public IReadOnlyList<MapTable> Maps { get; }

    private Almanac(IEnumerable<long> seeds, IEnumerable<IList<MapEntry>> mappings)
    {
        Seeds = seeds.ToList();
        Maps = mappings.Select(MapTable.Build).ToList();
    }
    
    public static Almanac Parse(string[] input)
    {
        var seeds = input[0].ParseLongs();
        var maps = input
            .Skip(2)
            .ChunkBy(l => !string.IsNullOrWhiteSpace(l) && !l.Contains(':'))
            .Select(c => c.Select(ParseMapEntry).ToList());

        return new Almanac(seeds, maps);
    }

    private static MapEntry ParseMapEntry(string line)
    {
        var longs = line.ParseLongs();
        return new MapEntry(
            destStart:   longs[0],
            sourceStart: longs[1],
            rangeLength: longs[2]);
    }
}