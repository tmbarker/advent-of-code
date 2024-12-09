using Utilities.Extensions;
using Utilities.Numerics;

namespace Solutions.Y2024.D09;

public class Disk
{
    public readonly record struct File(int Min, int Length);
    
    public int?[] Blocks { get; }
    public List<File> Allocated { get; }
    public List<Range<int>> Free { get; }
    
    private Disk(int?[] blocks, List<File> allocated, List<Range<int>> free)
    {
        Blocks = blocks;
        Allocated = allocated;
        Free = free;
    }

    public long Checksum()
    {
        return Blocks
            .Select((file, i) => i * (file ?? 0L))
            .Sum();
    }
    
    public static Disk Parse(string map)
    {
        var volume = map.Sum(c => c.AsDigit());
        var blocks = new int?[volume];
        var allocated = new List<File>();
        var free = new List<Range<int>>();

        var file = -1;
        var head = 0;

        for (var i = 0; i < map.Length; i++)
        {
            var count = map[i].AsDigit();
            var empty = i % 2 == 1;
            
            if (!empty)
            {
                file++;
                allocated.Add(new File(Min: head, Length: count));
            }
            else if (count != 0)
            {
                free.Add(new Range<int>(min: head, max: head + count - 1));
            }
            
            for (var j = 0; j < count; j++)
            {
                blocks[head++] = empty
                    ? null
                    : file;
            }
        }

        return new Disk(blocks, allocated, free);
    }
}