using Utilities.Extensions;

namespace Solutions.Y2025.D12;

[PuzzleInfo("Christmas Tree Farm", Topics.Vectors, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override int Parts => 1;

    public override object Run(int part)
    {
        var input = ChunkInputByNonEmpty();
        var shapes = input[..^1]
            .Select(chunk => (
                Id:   chunk[0].ParseInt(), 
                Size: chunk[1..].Sum(line => line.Count(c => c == '#'))))
            .ToDictionary(tuple => tuple.Id, tuple => tuple.Size);

        return input[^1]
            .Select(line =>
            {
                var chunks = line.Split(": ");
                var dims = chunks[0].ParseInts();
                return (Area: dims[0] * dims[1], Quantities: chunks[1].ParseInts());
            })
            .Count(region => region.Area >= region.Quantities.Select((quantity, id) => quantity * shapes[id]).Sum());
    }
}