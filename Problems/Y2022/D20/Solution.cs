using Problems.Y2022.Common;
using Utilities.Extensions;

namespace Problems.Y2022.D20;

/// <summary>
/// Grove Positioning System: https://adventofcode.com/2022/day/20
/// </summary>
public class Solution : SolutionBase2022
{
    private const int Key1 = 1;
    private const int Key2 = 811589153;
    private const int Mixes1 = 1;
    private const int Mixes2 = 10;
    
    private static readonly HashSet<int> CoordinateOffsets = new()
    {
        1000,
        2000,
        3000,
    };

    public override int Day => 20;
    
    public override object Run(int part)
    {
        var encryptedNumbers = GetEncryptedNumbers();
        return part switch
        {
            1 => DecodeCoordinates(encryptedNumbers, Key1, Mixes1, CoordinateOffsets),
            2 => DecodeCoordinates(encryptedNumbers, Key2, Mixes2, CoordinateOffsets),
            _ => ProblemNotSolvedString
        };
    }

    private static long DecodeCoordinates(IEnumerable<int> encryptedNumbers, long key, int numMixes, IEnumerable<int> coordOffsets)
    {
        var numbers = encryptedNumbers.Select(n => n * key).ToList();
        var originalZeroIndex = numbers.IndexOf(0);
        var memoryList = new MemoryList<long>(numbers);

        for (var m = 0; m < numMixes; m++)
        {
            for (var i = 0; i < numbers.Count; i++)
            {
                var value = memoryList.GetValueOriginal(i);
                var currentIndex = memoryList.OriginalToCurrentIndex(i);
                var targetIndex = GetMixedIndex(currentIndex, value, memoryList.Count);
            
                memoryList.MoveOriginalElement(i, targetIndex);
            }   
        }

        var decodedCoords = new List<long>();
        var currentZeroIndex = memoryList.OriginalToCurrentIndex(originalZeroIndex);
        
        foreach (var offset in coordOffsets)
        {
            var index = (currentZeroIndex + offset) % memoryList.Count; 
            decodedCoords.Add(memoryList.GetValueCurrent(index));
        }

        return decodedCoords.Sum();
    }

    private static int GetMixedIndex(int currentIndex, long value, int count)
    {
        return (int)(currentIndex + value).Modulo(count - 1);
    }

    private IEnumerable<int> GetEncryptedNumbers()
    {
        return GetInputLines().Select(int.Parse);
    }
}