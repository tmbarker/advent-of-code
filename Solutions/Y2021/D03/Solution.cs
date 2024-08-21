using Utilities.Extensions;

namespace Solutions.Y2021.D03;

[PuzzleInfo("Binary Diagnostic", Topics.BitwiseOperations, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var numberStrings = GetInputLines();
        
        return part switch
        {
            1 => ComputeConsumptionRate(numberStrings),
            2 => ComputeLifeSupportRating(numberStrings),
            _ => PuzzleNotSolvedString
        };
    }

    private static long ComputeConsumptionRate(IList<string> numbers)
    {
        var numBits = numbers[0].Length;
        var setBitsCountMap = new Dictionary<int, int>(numBits);

        for (var bit = 0; bit < numBits; bit++)
        {
            setBitsCountMap.Add(bit, ComputeBitFrequency(numbers, bit));
        }

        var gamma = 0L;
        var epsilon = 0L;

        for (var i = 0; i < numBits; i++)
        {
            if (setBitsCountMap[i] > numbers.Count / 2)
            {
                gamma += SetBitToDecimal(i);
            }
            else
            {
                epsilon += SetBitToDecimal(i);
            }
        }

        return gamma * epsilon;
    }

    private static long ComputeLifeSupportRating(IList<string> numbers)
    {
        return
            ComputeDecimalValue(FindNumber(numbers, BitCriteria.MostCommon)) *
            ComputeDecimalValue(FindNumber(numbers, BitCriteria.LeastCommon));
    }
    
    private static string FindNumber(IEnumerable<string> numbers, BitCriteria criteria)
    {
        var remaining = new List<string>(numbers);
    
        var numBits = remaining[0].Length;   
        var bit = numBits - 1;
        
        for (; bit >= 0; bit--)
        {
            var numRemaining = remaining.Count;
            if (numRemaining <= 1)
            {
                return remaining.Single();
            }
            
            var bitFrequency = ComputeBitFrequency(remaining, bit);
            var setOnMajority = 2 * bitFrequency >= numRemaining;
    
            remaining = remaining
                .Where(n => EvaluateBitCriteria(n, bit, setOnMajority, criteria))
                .ToList();
        }

        return remaining.Single();
    }
    
    private static bool EvaluateBitCriteria(string number, int currentBit, bool majoritySet, BitCriteria criteria)
    {
        var set = IsBitSet(number, currentBit);
        return criteria == BitCriteria.MostCommon ? 
            set == majoritySet : 
            set != majoritySet;
    }

    private static long ComputeDecimalValue(string number)
    {
        var value = 0L;
        
        for (var bit = 0; bit < number.Length; bit++)
        {
            if (IsBitSet(number, bit))
            {
                value += SetBitToDecimal(bit);
            }
        }

        return value;
    }

    private static long SetBitToDecimal(int bit)
    {
        return (long)Math.Pow(2, bit);
    }
    
    private static int ComputeBitFrequency(IEnumerable<string> numbers, int bit)
    {
        return numbers.Count(n => IsBitSet(n, bit));
    }

    private static bool IsBitSet(string number, int bit)
    {
        return number[number.Length - 1 - bit].AsDigit() > 0;
    }
}