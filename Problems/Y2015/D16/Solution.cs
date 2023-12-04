using System.Text.RegularExpressions;
using Problems.Common;
using Utilities.Extensions;

namespace Problems.Y2015.D16;

/// <summary>
/// Aunt Sue: https://adventofcode.com/2015/day/16
/// </summary>
public class Solution : SolutionBase
{
    private delegate bool ValidPredicate(IEnumerable<(string, int)> things);
    
    public override object Run(int part)
    {
        return part switch
        {
            1 => Find(predicate: IsValidExact),
            2 => Find(predicate: IsValidRange),
            _ => ProblemNotSolvedString
        };
    }

    private int Find(ValidPredicate predicate)
    {
        var input = GetInputLines();
        var regex = new Regex(@"Sue (?<Id>\d+): (?:(?<Keys>[a-z]+): (?<Values>\d+)(?:, )?)+");
        
        foreach (var line in input)
        {
            var match = regex.Match(line);
            var keys = match.Groups["Keys"].Captures.Select(c => c.Value);
            var values = match.Groups["Values"].ParseInts();
            
            if (predicate(keys.Zip(values)))
            {
                return match.Groups["Id"].ParseInt();
            }
        }

        throw new NoSolutionException();
    }

    private static bool IsValidExact(IEnumerable<(string, int)> things)
    {
        foreach (var (thing, amount) in things)
        {
            switch (thing)
            {
                case @"children"    when amount != 3:
                case @"cats"        when amount != 7:
                case @"samoyeds"    when amount != 2:
                case @"pomeranians" when amount != 3:
                case @"akitas"      when amount != 0:
                case @"vizslas"     when amount != 0:
                case @"goldfish"    when amount != 5:
                case @"trees"       when amount != 3:
                case @"cars"        when amount != 2:
                case @"perfumes"    when amount != 1:
                    return false;
            }
        }
        return true;
    }
    
    private static bool IsValidRange(IEnumerable<(string, int)> things)
    {
        foreach (var (thing, amount) in things)
        {
            switch (thing)
            {
                case @"children"    when amount != 3:
                case @"cats"        when amount <= 7:
                case @"samoyeds"    when amount != 2:
                case @"pomeranians" when amount >= 3:
                case @"akitas"      when amount != 0:
                case @"vizslas"     when amount != 0:
                case @"goldfish"    when amount >= 5:
                case @"trees"       when amount <= 3:
                case @"cars"        when amount != 2:
                case @"perfumes"    when amount != 1:
                    return false;
            }
        }
        return true;
    }
}