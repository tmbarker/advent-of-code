using System.Text;
using System.Text.RegularExpressions;
using Problems.Attributes;
using Problems.Common;

namespace Problems.Y2020.D19;

using Messages = IEnumerable<string>;
using Rules = IDictionary<int, string>;
using Memo = IDictionary<int, string>;

/// <summary>
/// Monster Messages: https://adventofcode.com/2020/day/19
/// </summary>
[Favourite("Monster Messages", Topics.RegularExpressions, Difficulty.Hard)]
public class Solution : SolutionBase
{
    private const int Rule = 0;

    /*
     For part 2 we are given two rules, 8 and 11, which are recursive. We can handle both using custom Regexes as follows:
     
     Rule 8)  "42 | 42 8" Means that we want to have one or more instances of pattern 42, which is the
              definition of the + token. Therefore, we can implement 8') using:
              "42 +"
     
     Rule 11) "42 31 | 42 11 31" Means that we want to have an equal number of 42s and 31s (and at least one of each).
              .NET Regex gives us Balancing Groups, which allows us to 'pop' the values captured to a named capture
              group. The pattern will fail if we try to pop an empty group. Therefore we can create a named capture 
              group which is pushed to as we match 42s, and popped as we match 31s. 
              
              The last thing we need to do is validate that our "stack" is empty after the final 31. To do so we
              can use Conditional Patterns, (?(CONDITION)TRUE|FALSE). We are allowed to use capture group names for the
              condition, which will fail if the group is non-empty. When only one condition is specified, that is the
              fail pattern, so we can simply specify an empty negative lookahead (?!) to ensure that our match fails.
              
              Therefore, we can implement 11') using:
              "(?<Stack> 42 )+(?<-Stack> 31 )+(?(Stack)(?!))"
                    ^               ^              ^
                   Push            Pop        Assert Empty
    */ 
    private static readonly Rules RuleOverrides = new Dictionary<int, string>
    {
        { 8,  "42 +" },
        { 11, "(?<Stack> 42 )+(?<-Stack> 31 )+(?(Stack)(?!))" }
    };

    public override object Run(int part)
    {
        ParseInput(GetInputLines(), out var rules, out var messages);
        return part switch
        {
            1 => CountMatches(Rule, messages, rules, useRuleOverrides: false),
            2 => CountMatches(Rule, messages, rules, useRuleOverrides: true),
            _ => ProblemNotSolvedString
        };
    }

    private static int CountMatches(int ruleId, Messages messages, Rules rules, bool useRuleOverrides)
    {
        if (useRuleOverrides)
        {
            foreach (var (id, rule) in RuleOverrides)
            {
                rules[id] = rule;
            }
        }
        
        var memo = new Dictionary<int, string>();
        var regex = new Regex($"^{BuildRegex(ruleId, rules, memo)}$");
        
        return messages.Count(m => regex.Match(m).Success);
    }
    
    private static string BuildRegex(int ruleId, Rules rules, Memo memo)
    {
        if (memo.TryGetValue(ruleId, out var builtRegex))
        {
            return builtRegex;
        }
        
        var rule = rules[ruleId];
        if (rule.Contains('"'))
        {
            memo[ruleId] = rule[rule.IndexOf('"') + 1].ToString();
            return memo[ruleId];
        }
        
        var hasAlternations = rule.Contains('|');
        var tokens = rule.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var regex = new StringBuilder();

        if (hasAlternations)
        {
            regex.Append("(?:");
        }
        
        foreach (var token in tokens)
        {
            regex.Append(int.TryParse(token, out var nestedRuleId)
                ? BuildRegex(nestedRuleId, rules, memo)
                : token);
        }

        if (hasAlternations)
        {
            regex.Append(')');
        }

        memo[ruleId] = regex.ToString();
        return memo[ruleId];
    }
    
    private static void ParseInput(IList<string> input, out Rules rules, out Messages messages)
    {
        rules = new Dictionary<int, string>();
        for (var i = 0; !string.IsNullOrWhiteSpace(input[i]); i++)
        {
            var elements = input[i].Split(separator: ':');
            var ruleId = int.Parse(elements[0]);
            
            rules.Add(ruleId, elements[1]);
        }

        messages = new List<string>(input.Skip(rules.Count + 1));
    }
}