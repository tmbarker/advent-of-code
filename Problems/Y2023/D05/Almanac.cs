using Utilities.Extensions;

namespace Problems.Y2023.D05;

public sealed class Almanac
{
    private const string MapToken = "map";
    private const int NumMaps = 7;
    
    public IReadOnlyList<long> Seeds { get; }
    public IReadOnlyList<MapTable> Maps { get; }

    private Almanac(IEnumerable<long> seeds, IEnumerable<List<MapEntry>> mappings)
    {
        Seeds = seeds.ToList();
        Maps = mappings.Select(MapTable.Build).ToList();
    }
    
    public static Almanac Parse(string[] input)
    {
        var seeds = input[0].ParseLongs();
        var maps = new List<List<MapEntry>>(capacity: NumMaps);

        for (var i = 2; i < input.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(input[i]))
            {
                continue;
            }
                
            if (input[i].Contains(MapToken))
            {
                maps.Add(item: new List<MapEntry>());
                continue;
            }
            
            var longs = input[i].ParseLongs();
            var mapping = new MapEntry(
                destStart:   longs[0],
                sourceStart: longs[1],
                rangeLength: longs[2]);
                
            maps[^1].Add(mapping);
        }

        return new Almanac(seeds, maps);
    }
}