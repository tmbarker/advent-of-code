using System.Text.RegularExpressions;
using Problems.Attributes;
using Problems.Common;
using Utilities.Extensions;

namespace Problems.Y2017.D21;

using Transform = Func<Pattern, Pattern>;
using Rules = IReadOnlyDictionary<string, string>;

/// <summary>
/// Fractal Art: https://adventofcode.com/2017/day/21
/// </summary>
[Favourite("Fractal Art", Topics.Vectors, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private static readonly Pattern Initial = new(size: 3, buffer: @".#./..#/###");
    private static readonly IReadOnlyList<Transform> Transforms = new List<Transform>
    {
        p => p.Rotate(),
        p => p.Rotate(),
        p => p.Rotate(),
        p => p.Flip(),
        p => p.Rotate(),
        p => p.Rotate(),
        p => p.Rotate()
    };

    public override object Run(int part)
    {
        var input = GetInputLines();
        var rules = ParseEnhancementRules(input);
        
        return part switch
        {
            1 => Enhance(pattern: Initial, rules: rules, iterations: 5).On,
            2 => Enhance(pattern: Initial, rules: rules, iterations: 18).On,
            _ => ProblemNotSolvedString
        };
    }

    private static Pattern Enhance(Pattern pattern, Rules rules, int iterations)
    {
        for (var i = 0; i < iterations; i++)
        {
            pattern = Enhance(pattern, rules);
        }
        return pattern;
    }

    private static Pattern Enhance(Pattern pattern, Rules rules)
    {
        var subregionSize = pattern.Size % 2 == 0 ? 2 : 3;
        var subregionsPerSide = pattern.Size / subregionSize;
        var enhancedRegionSize = subregionSize + 1;
        var enhancedPattern = Pattern.Empty(size: enhancedRegionSize * subregionsPerSide);
        
        for (var x = 0; x < subregionsPerSide; x++)
        for (var y = 0; y < subregionsPerSide; y++)
        {
            var subregionOffset = (x * subregionSize, y * subregionSize);
            var subregion = pattern.SubRegion(subregionOffset, subregionSize);
            var enhanced = new Pattern(size: subregion.Size + 1, buffer: rules[subregion.Key]);
            var enhancedOffset = (enhancedRegionSize * x, enhancedRegionSize * y);

            EmbedRegion(enhancedPattern, enhanced, enhancedOffset);
        }
        
        return enhancedPattern;
    }

    private static void EmbedRegion(Pattern pattern, Pattern region, (int X, int Y) offset)
    {
        for (var x = 0; x < region.Size; x++)
        for (var y = 0; y < region.Size; y++)
        {
            pattern[(offset.X + x, offset.Y + y)] = region[(x, y)];
        }
    }

    private static Rules ParseEnhancementRules(IEnumerable<string> lines)
    {
        var regex = new Regex(@"(?<Input>[.#/]+) => (?<Output>[.#/]+)");
        var map = new Dictionary<string, string>();
        
        foreach (var line in lines)
        {
            var match = regex.Match(line);
            var input = match.Groups["Input"].Value;
            var output = match.Groups["Output"].Value;

            map[input] = output;
        }
        
        //  Precompute the transformations of each enhancement key
        //
        foreach (var nominalKey in map.Keys.Freeze())
        foreach (var transformedKey in GetTransformedKeys(nominalKey))
        {
            map[transformedKey] = map[nominalKey];
        }

        return map;
    }
    
    private static IEnumerable<string> GetTransformedKeys(string key)
    {
        var size = key.IndexOf('/');
        var pattern = new Pattern(size: size, buffer: key);

        foreach (var transform in Transforms)
        {
            pattern = transform(pattern);
            yield return pattern.Key;
        }
    }
}