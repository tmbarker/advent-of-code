using Problems.Y2020.Common;
using System.Text.RegularExpressions;

namespace Problems.Y2020.D04;

/// <summary>
/// Passport Processing: https://adventofcode.com/2020/day/4
/// </summary>
public class Solution : SolutionBase2020
{
    private const RegexOptions Options = RegexOptions.Multiline;
    private static readonly IReadOnlyDictionary<string, Predicate<string>> FieldValidators =
        new Dictionary<string, Predicate<string>>
    {
        {"byr", p => Regex.IsMatch(p,@"^(?=.*byr:((19[2-9][0-9])|(200[0-2]))\b).*", Options)},
        {"iyr", p => Regex.IsMatch(p,@"^(?=.*iyr:20((1[0-9])|20)\b).*", Options)},
        {"eyr", p => Regex.IsMatch(p,@"^(?=.*eyr:20((2[0-9])|30)\b).*", Options)},
        {"hgt", p => Regex.IsMatch(p,@"^(?=.*hgt:(1([5-8][0-9]|9[0-3])cm|(59|6[0-9]|7[0-6])in)\b).*", Options)},
        {"hcl", p => Regex.IsMatch(p,@"^(?=.*hcl:#[0-9a-f]{6}\b).*", Options)},
        {"ecl", p => Regex.IsMatch(p,@"^(?=.*ecl:(amb|blu|brn|gry|grn|hzl|oth)\b).*", Options)},
        {"pid", p => Regex.IsMatch(p,@"^(?=.*pid:\d{9}\b).*", Options)},
    };

    public override int Day => 4;
    
    public override object Run(int part)
    {
        var passports = ParsePassportData(GetInputText());
        return part switch
        {
            0 => passports.Count(ValidatePassportFieldPresence),
            1 => passports.Count(ValidatePassportFieldData),
            _ => ProblemNotSolvedString,
        };
    }

    private static bool ValidatePassportFieldPresence(string passport)
    {
        return FieldValidators.Keys.All(passport.Contains);
    }
    
    private static bool ValidatePassportFieldData(string passport)
    {
        return FieldValidators.Values.All(isValidPredicate => isValidPredicate.Invoke(passport));
    }
    
    private static IEnumerable<string> ParsePassportData(string input)
    {
        return input.Split(new[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);
    }
}