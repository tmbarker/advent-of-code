using System.Text;
using Problems.Common;
using Utilities.Hashing;

namespace Problems.Y2016.D05;

/// <summary>
/// How About a Nice Game of Chess?: https://adventofcode.com/2016/day/5
/// </summary>
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => FindOrderedPassword(),
            2 => FindPositionalPassword(),
            _ => ProblemNotSolvedString
        };
    }

    private string FindOrderedPassword()
    {
        var input = GetInputText();
        var pass = new StringBuilder();
        var hashProvider = new Md5Provider();

        for (var i = 0; pass.Length < 8; i++)
        {
            var hash = hashProvider.GetHashHex($"{input}{i}");
            var found = hash.StartsWith("00000");

            if (!found)
            {
                continue;
            }
            
            Log($"Found password char [i={i}]: {hash[5]}");
            pass.Append(hash[5]);
        }

        return pass.ToString();
    }
    
    private string FindPositionalPassword()
    {
        var input = GetInputText();
        var pass = new char[8];
        var unset = Enumerable.Range(start: 0, count: 8).ToHashSet();
        var hashProvider = new Md5Provider();

        for (var i = 0; unset.Count > 0; i++)
        {
            var hash = hashProvider.GetHashHex($"{input}{i}");
            var found = hash.StartsWith("00000");
            var pos = hash[5] - '0';

            if (!found || !unset.Contains(pos) || pos < 0 || pos >= 8)
            {
                continue;
            }

            Log($"Found password char [i={i}, p={pos}]: {hash[6]}");
            unset.Remove(pos);
            pass[pos] = hash[6];
        }

        return string.Concat(pass);
    }

    private void Log(string log)
    {
        if (LogsEnabled)
        {
            Console.WriteLine(log);
        }
    }
}