using Problems.Common;
using Utilities.Extensions;

namespace Problems.Y2021.D08;

/// <summary>
/// Seven Segment Search: https://adventofcode.com/2021/day/8
/// </summary>
public class Solution : SolutionBase
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
        {0, new HashSet<char>{'a', 'b', 'c', 'e', 'f', 'g'}},
        {1, new HashSet<char>{'c', 'f'}},
        {2, new HashSet<char>{'a', 'c', 'd', 'e', 'g'}},
        {3, new HashSet<char>{'a', 'c', 'd', 'f', 'g'}},
        {4, new HashSet<char>{'b', 'c', 'd', 'f'}},
        {5, new HashSet<char>{'a', 'b', 'd', 'f', 'g'}},
        {6, new HashSet<char>{'a', 'b', 'd', 'e', 'f', 'g' }},
        {7, new HashSet<char>{'a', 'c', 'f'}},
        {8, new HashSet<char>{'a', 'b', 'c', 'd', 'e', 'f', 'g'}},
        {9, new HashSet<char>{'a', 'b', 'c', 'd', 'f', 'g'}}
    };

    public override object Run(int part)
    {
        var notes = ParseSignalPatternNotes(GetInputLines()).ToList();

        return part switch
        {
            1 => CountUniqueSegmentDigitOccurrences(notes),
            2 => SumDecodedOutputs(notes),
            _ => ProblemNotSolvedString
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
        var distinctSegmentFrequencies = expectedSegmentFrequencies.FilterByDistinctValues();
        var observedSegmentFrequencies = FormObservedSegmentFrequencyMap(uniqueSegmentPatterns);
        
        foreach (var (segment, expectedFrequency) in distinctSegmentFrequencies)
        {
            foreach (var (observedSegment, observedFrequency) in observedSegmentFrequencies)
            {
                if (observedFrequency != expectedFrequency)
                {
                    continue;
                }
                
                decodedSegmentsMap.Add(observedSegment, segment);
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
        return RequiredSegmentsMap
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Count)
            .FilterByDistinctValues()
            .Keys.ToHashSet();
    }

    private static Dictionary<char, int> FormExpectedSegmentFrequencyMap()
    {
        var frequencyMap = RequiredSegmentsMap[8].ToDictionary(c => c, _ => 0);
        foreach (var (_, segments) in RequiredSegmentsMap)
        {
            foreach (var segment in segments)
            {
                frequencyMap[segment]++;
            }
        }

        return frequencyMap;
    }

    private static Dictionary<char, int> FormObservedSegmentFrequencyMap(IEnumerable<string> observations)
    {
        var frequencyMap = new Dictionary<char, int>();
        foreach (var observation in observations)
        {
            foreach (var segment in observation)
            {
                frequencyMap.EnsureContainsKey(segment);
                frequencyMap[segment]++;
            }
        }
        
        return frequencyMap;
    }

    private static IEnumerable<DisplayObservation> ParseSignalPatternNotes(IEnumerable<string> input)
    {
        const StringSplitOptions splitOptions = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;
        foreach (var line in input)
        {
            var parts = line.Split(PartsDelimiter, splitOptions);
            var uniquePatterns = parts[0].Split(ElementsDelimiter, splitOptions);
            var outputDigits = parts[1].Split(ElementsDelimiter, splitOptions);

            yield return new DisplayObservation(uniquePatterns, outputDigits);
        }
    }
}