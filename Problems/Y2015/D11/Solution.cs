using System.Text.RegularExpressions;

namespace Problems.Y2015.D11;

[PuzzleInfo("Corporate Policy", Topics.StringParsing|Topics.RegularExpressions, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var password = GetInputText();
        return part switch
        {
            1 => FindNextValid(password),
            2 => FindNextValid(FindNextValid(password)),
            _ => ProblemNotSolvedString
        };
    }

    private static string FindNextValid(string password)
    {
        var valid = false;
        while (!valid)
        {
            password = Increment(password);
            valid = IsValid(password);
        }
        return password;
    }
    
    private static string Increment(string password)
    {
        var next = password.ToCharArray();
        for (var i = password.Length - 1; i >= 0; i--)
        {
            next[i] = (char)((password[i] - 'a' + 1) % 26 + 'a');
            if (next[i] != 'a')
            {
                break;
            }
        }
        return string.Concat(next);
    }

    private static bool IsValid(string password)
    {
        var hasRun = false;
        for (var i = 2; i < password.Length; i++)
        {
            if (password[i] == password[i - 1] + 1 && password[i - 1] == password[i - 2] + 1)
            {
                hasRun = true;
                break;
            }
        }

        return
            hasRun &&
            Regex.Matches(password, @"(.)\1").Count >= 2 &&
            Regex.IsMatch(password, @"(?!.*(i|o|l))");
    }
}