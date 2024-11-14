using Utilities.Collections;

namespace Solutions.Y2021.D08;

[PuzzleInfo("Seven Segment Search", Topics.None, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private const char PartsDelimiter = '|';
    private const char ElementsDelimiter = ' ';
    
    // NOTE: This is the reference seven segment display used in the hardcoded map below
    //       aaaa
    //      b    c
    //      b    c
    //       dddd
    //      e    f
    //      e    f
    //       gggg
    //
    private static readonly Dictionary<int, HashSet<char>> RequiredSegmentsMap = new()
    {
        {0, ['a', 'b', 'c', 'e', 'f', 'g'] },
        {1, ['c', 'f'] },
        {2, ['a', 'c', 'd', 'e', 'g'] },
        {3, ['a', 'c', 'd', 'f', 'g'] },
        {4, ['b', 'c', 'd', 'f'] },
        {5, ['a', 'b', 'd', 'f', 'g'] },
        {6, ['a', 'b', 'd', 'e', 'f', 'g'] },
        {7, ['a', 'c', 'f'] },
        {8, ['a', 'b', 'c', 'd', 'e', 'f', 'g'] },
        {9, ['a', 'b', 'c', 'd', 'f', 'g'] }
    };

    public override object Run(int part)
    {
        var notes = ParseSignalPatternNotes(GetInputLines()).ToList();

        return part switch
        {
            1 => CountUniqueSegmentDigitOccurrences(notes),
            2 => SumDecodedOutputs(notes),
            _ => PuzzleNotSolvedString
        };
    }

    private static int CountUniqueSegmentDigitOccurrences(IEnumerable<DisplayObservation> observations)
    {
        var occurrences = 0;
        var uniqueSegmentCountsSet = GetDigitsWithUniqueSegmentCounts()
            .Select(d => RequiredSegmentsMap[d].Count);
        
        foreach (var observation in observations)
        {
            var outputDigitLengths = observation.OutputDigitSegments.Select(d => d.Length);
            occurrences += outputDigitLengths.Count(digitLength => uniqueSegmentCountsSet.Contains(digitLength));
        }

        return occurrences;
    }

    private static int SumDecodedOutputs(IEnumerable<DisplayObservation> observations)
    {
        var sum = 0;
        foreach (var observation in observations)
        {
            var segmentMap = DecodeSegmentPatterns(observation.UniqueSegmentPatterns);
            var decodedOutput = DecodeOutputDigits(observation.OutputDigitSegments, segmentMap);

            sum += decodedOutput;
        }
        return sum;
    }

    private static Dictionary<char, char> DecodeSegmentPatterns(IList<string> uniqueSegmentPatterns)
    {
        var decodedSegmentsMap = new Dictionary<char, char>();
        
        //  We can decode the first 3 signals/segments by noticing that in any given set of 10 distinct digits, there
        //  are 3 segments with a distinct frequency count. These 3 mappings can be found by comparing the expected
        //  segment frequencies with the observed segment frequencies
        //
        var expectedSegmentFrequencies = FormExpectedSegmentFrequencyMap();
        var distinctSegmentFrequencies = FilterByDistinctValues(expectedSegmentFrequencies);
        var observedSegmentFrequencies = FormObservedSegmentFrequencyMap(uniqueSegmentPatterns);
        
        foreach (var (expectedSegment, expectedFrequency) in distinctSegmentFrequencies)
        foreach (var (observedSegment, observedFrequency) in observedSegmentFrequencies)
        {
            if (observedFrequency == expectedFrequency)
            {
                decodedSegmentsMap.Add(observedSegment, expectedSegment);
                break;
            }
        }

        //  We now have 3 of 7 signals/segments decoded, to decode the remaining 4 we can use the digits which
        //  require a distinct number of segments to display, starting with the digit requiring the fewest segments
        //
        var orderedDigitsWithUniqueSegmentCountSet = GetDigitsWithUniqueSegmentCounts()
            .OrderBy(d => RequiredSegmentsMap[d].Count);

        foreach (var digit in orderedDigitsWithUniqueSegmentCountSet)
        {
            var expectedSegments = RequiredSegmentsMap[digit];
            var numSegments = RequiredSegmentsMap[digit].Count;
            
            foreach (var signalPattern in uniqueSegmentPatterns)
            {
                if (signalPattern.Length != numSegments)
                {
                    continue;
                }
                
                //  Check that we only have 1 missing/yet to be decoded segment in the digit
                //
                if (signalPattern.Count(s => !decodedSegmentsMap.ContainsKey(s)) != 1)
                {
                    continue;
                }
                
                var signal = signalPattern.Single(s => !decodedSegmentsMap.ContainsKey(s));
                var segment = expectedSegments.Single(s => !decodedSegmentsMap.ContainsValue(s));
                decodedSegmentsMap.Add(signal, segment);
            }
        }

        return decodedSegmentsMap;
    }

    private static int DecodeOutputDigits(IList<string> outputDigitSegments, IReadOnlyDictionary<char, char> map)
    {
        const int decimalBase = 10;
        var output = 0;
        
        for (var i = 0; i < outputDigitSegments.Count; i++)
        {
            var outputSegments = outputDigitSegments[outputDigitSegments.Count - 1 - i];
            var decodedSegments = outputSegments.Select(c => map[c]).ToList();

            foreach (var (digit, segments) in RequiredSegmentsMap)
            {
                if (!decodedSegments.All(c => segments.Contains(c)) || decodedSegments.Count != segments.Count)
                {
                    continue;
                }

                output += (int)(Math.Pow(decimalBase, i) * digit);
                break;
            }
        }

        return output;
    }

    private static IEnumerable<int> GetDigitsWithUniqueSegmentCounts()
    {
        var map = RequiredSegmentsMap.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Count);
        var distinct = FilterByDistinctValues(map);

        return distinct.Keys;
    }

    private static IDictionary<char, int> FormExpectedSegmentFrequencyMap()
    {
        var frequencyMap = RequiredSegmentsMap[8].ToDictionary(c => c, _ => 0);
        
        foreach (var (_, segments) in RequiredSegmentsMap)
        foreach (var segment in segments)
        {
            frequencyMap[segment]++;
        }

        return frequencyMap;
    }

    private static IDictionary<char, int> FormObservedSegmentFrequencyMap(IEnumerable<string> observations)
    {
        var frequencyMap = new DefaultDict<char, int>(defaultValue: 0);
        
        foreach (var observation in observations)
        foreach (var segment in observation)
        {
            frequencyMap[segment]++;
        }
        
        return frequencyMap;
    }

    private static IEnumerable<DisplayObservation> ParseSignalPatternNotes(IEnumerable<string> input)
    {
        const StringSplitOptions options = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;
        foreach (var line in input)
        {
            var parts = line.Split(separator: PartsDelimiter, options);
            var uniquePatterns = parts[0].Split(separator: ElementsDelimiter, options);
            var outputDigits = parts[1].Split(separator: ElementsDelimiter, options);

            yield return new DisplayObservation(uniquePatterns, outputDigits);
        }
    }

    /// <summary>
    ///     Filter the dictionary entries so only Key-Value pairs with a distinct Value are returned
    /// </summary>
    private static Dictionary<TKey, TValue> FilterByDistinctValues<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
        where TKey : notnull
    {
        return dictionary
            .GroupBy(kvp => kvp.Value)
            .Where(g => g.Count() == 1)
            .Select(g => g.First())
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }
}