using System.Text.RegularExpressions;
using Problems.Attributes;
using Problems.Y2020.Common;

namespace Problems.Y2020.D04;

/// <summary>
/// Passport Processing: https://adventofcode.com/2020/day/4
/// </summary>
[Favourite("Passport Processing", Topics.RegularExpressions, Difficulty.Medium)]
public class Solution : SolutionBase2020
{
    private const RegexOptions Options = RegexOptions.Multiline;
    private static readonly IReadOnlyDictionary<string, string> FieldValidators = new Dictionary<string, string>
    {
        {"byr", @"^(?=.*byr:((19[2-9][0-9])|(200[0-2]))\b).*"},
        {"iyr", @"^(?=.*iyr:20((1[0-9])|20)\b).*"},
        {"eyr", @"^(?=.*eyr:20((2[0-9])|30)\b).*"},
        {"hgt", @"^(?=.*hgt:(1([5-8][0-9]|9[0-3])cm|(59|6[0-9]|7[0-6])in)\b).*"},
        {"hcl", @"^(?=.*hcl:#[0-9a-f]{6}\b).*"},
        {"ecl", @"^(?=.*ecl:(amb|blu|brn|gry|grn|hzl|oth)\b).*"},
        {"pid", @"^(?=.*pid:\d{9}\b).*"},
    };

    public override int Day => 4;
    
    public override object Run(int part)
    {
        var passports = ParsePassportData(GetInputText());
        return part switch
        {
            1 =>  passports.Count(ValidatePassportFieldPresence),
            2 =>  passports.Count(ValidatePassportFieldData),
            _ => ProblemNotSolvedString
        };
    }

    private static bool ValidatePassportFieldPresence(string passport)
    {
        return FieldValidators.Keys.All(passport.Contains);
    }
    
    private static bool ValidatePassportFieldData(string passport)
    {
        return FieldValidators.Values.All(regex => Regex.IsMatch(passport, regex, Options));
    }
    
    private static IEnumerable<string> ParsePassportData(string input)
    {
        return input.Split(new[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);
    }
}