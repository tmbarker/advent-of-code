using Utilities.Numerics;

namespace Solutions.Y2024.D09;

[PuzzleInfo("Disk Fragmenter", Topics.Simulation, Difficulty.Medium, favourite: true)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var map = GetInputText();
        var disk = Disk.Parse(map);
        
        return part switch
        {
            1 => DefragmentBlocks(disk),
            2 => DefragmentFiles(disk),
            _ => PuzzleNotSolvedString
        };
    }

    private static long DefragmentBlocks(Disk disk)
    {
        var head = 0;
        var tail = disk.Blocks.Length - 1;

        while (head < tail)
        {
            if (disk.Blocks[head] != null)
            {
                head++;
                continue;
            }

            while (disk.Blocks[tail] == null) tail--;
            disk.Blocks[head++] = disk.Blocks[tail];
            disk.Blocks[tail--] = null;
        }

        return disk.Checksum();
    }
    
    private static long DefragmentFiles(Disk disk)
    {
        for (var fileId = disk.Allocated.Count - 1; fileId >= 0; fileId--)
        {
            var file = disk.Allocated[fileId];
            for (var j = 0; j < disk.Free.Count; j++)
            {
                var free = disk.Free[j];
                if (free.Min >= file.Min) break;
                if (free.Length < file.Length) continue;

                for (var k = 0; k < file.Length; k++)
                {
                    disk.Blocks[free.Min + k] = fileId;
                    disk.Blocks[file.Min + k] = null;
                }
                
                disk.Free.RemoveAt(index: j);
                if (free.Length > file.Length)
                {
                    disk.Free.Insert(index: j, new Range<int>(min: free.Min + file.Length, max: free.Max));
                }
                break;
            }
        }
        
        return disk.Checksum();
    }
}