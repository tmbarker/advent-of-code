using System.Collections;
using System.Globalization;
using Problems.Common;
using Problems.Y2017.Common;
using Utilities.Collections;

namespace Problems.Y2017.D14;

/// <summary>
/// Disk Defragmentation: https://adventofcode.com/2017/day/14
/// </summary>
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var hashPrefix = GetInputText();
        var memory = BuildMemoryMap(hashPrefix);
        
        return part switch
        {
            1 => CountSquares(memory),
            2 => CountRegions(memory),
            _ => ProblemNotSolvedString
        };
    }

    private static int CountSquares(bool[,] memory)
    {
        var count = 0;
        
        for (var row = 0; row < 128; row++)
        for (var col = 0; col < 128; col++)
        {
            count += memory[row, col] ? 1 : 0;
        }
        
        return count;
    }

    private static int CountRegions(bool[,] memory)
    {
        var disjointSet = new DisjointSet<(int row, int col)>();
        
        for (var row = 0; row < 128; row++)
        for (var col = 0; col < 128; col++)
        {
            if (!memory[row, col])
            {
                continue;
            }

            disjointSet.MakeSet((row, col));
            foreach (var adjacent in GetAdjacentSet(memory, row, col))
            {
                disjointSet.MakeSet(adjacent);
                disjointSet.Union((row, col), adjacent);
            }
        }

        return disjointSet.PartitionsCount;
    }
    
    private static bool[,] BuildMemoryMap(string prefix)
    {
        var map = new bool[128, 128];
        
        for (var row = 0; row < 128; row++)
        {
            var hash = KnotHash.ComputeHash(input: $"{prefix}-{row}");
            var bits = HexToBitArray(hash);

            for (var col = 0; col < 128; col++)
            {
                map[row, col] = bits[col];
            }
        }
        
        return map;
    }

    private static IEnumerable<(int Row, int Col)> GetAdjacentSet(bool[,] memory, int row, int col)
    {
        if (ValidAddress(row - 1, col) && memory[row - 1, col]) yield return (row - 1, col);
        if (ValidAddress(row + 1, col) && memory[row + 1, col]) yield return (row + 1, col);
        if (ValidAddress(row, col - 1) && memory[row, col - 1]) yield return (row, col - 1);
        if (ValidAddress(row, col + 1) && memory[row, col + 1]) yield return (row, col + 1);
    }

    private static bool ValidAddress(int row, int col)
    {
        return row is >= 0 and < 128 && col is >= 0 and < 128;
    }

    private static BitArray HexToBitArray(string hex)
    {
        var bits = new BitArray(length: 4 * hex.Length);
        
        for (var i = 0; i < hex.Length; i++)
        {
            var b = byte.Parse(hex[i].ToString(), NumberStyles.HexNumber);
            for (var j = 0; j < 4; j++)
            {
                bits.Set(i * 4 + j, (b & (1 << (3 - j))) != 0);
            }
        }
        
        return bits;
    }
}