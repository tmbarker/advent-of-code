namespace Solutions.Y2024.D25;

[PuzzleInfo("Code Chronicle", Topics.StringParsing, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override int Parts => 1;

    public override object Run(int part)
    {
        var locks = new List<List<int>>();
        var keys  = new List<List<int>>();
        var congruent = new HashSet<(int Lock, int Key)>();

        foreach (var schematic in ChunkInputByNonEmpty())
        {
            var isLock = schematic[0].All(c => c == '#');
            var heights = Enumerable.Range(0, schematic[0].Length)
                .Select(x => schematic.Count(line => line[x] == '#') - 1)
                .ToList();

            if (isLock)
                locks.Add(heights);
            else
                keys.Add(heights);
        }
        
        for (var l = 0; l < locks.Count; l++)
        for (var k = 0; k < keys.Count;  k++)
        {
            if (locks[l].Zip(keys[k]).All(tuple => tuple.First + tuple.Second <= 5))
            {
                congruent.Add((l, k));
            }
        }

        return congruent.Count;
    }
}