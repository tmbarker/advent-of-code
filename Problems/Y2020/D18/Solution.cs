using Problems.Attributes;
using Problems.Common;
using Utilities.Extensions;

namespace Problems.Y2020.D18;

using Tokens = Queue<char>;
using Precedences = IReadOnlyDictionary<char, int>;

/// <summary>
/// Operation Order: https://adventofcode.com/2020/day/18
/// </summary>
[Favourite("Operation Order", Topics.StringParsing|Topics.Math, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var expressions = ParseExpressions(GetInputLines());
        return part switch
        {
            1 => expressions.Sum(tokens => Evaluate(tokens, Operators.EqualPrecedence)),
            2 => expressions.Sum(tokens => Evaluate(tokens, Operators.AddPrecedence)),
            _ => ProblemNotSolvedString
        };
    }

    private static long Evaluate(Tokens tokens, Precedences precedences)
    {
        var operators = new Stack<char>();
        var literals = new Stack<long>();

        while (tokens.Any())
        {
            var token = tokens.Dequeue();
            switch (token)
            {
                case Operators.Add:
                case Operators.Mul:
                    HandleOperatorToken(token, operators, literals, precedences);
                    break;
                case Operators.Open:
                    literals.Push(Evaluate(tokens, precedences));
                    break;
                case Operators.Close:
                    return EvaluateTokenStacks(operators, literals);
                default:
                    literals.Push(token.AsDigit());
                    break;
            }
        }

        return EvaluateTokenStacks(operators, literals);
    }

    private static long EvaluateTokenStacks(Stack<char> operators, Stack<long> literals)
    {
        while (operators.Any())
        {
            literals.Push(Operators.Delegates[operators.Pop()](
                lhs: literals.Pop(),
                rhs: literals.Pop()));
        }
        return literals.Single();
    }

    private static void HandleOperatorToken(char token, Stack<char> operators, Stack<long> literals, Precedences precedences)
    {
        while (operators.Any() && precedences[operators.Peek()] <= precedences[token])
        {
            literals.Push(Operators.Delegates[operators.Pop()](
                lhs: literals.Pop(),
                rhs: literals.Pop())); 
        }
        operators.Push(token);
    }

    private static IEnumerable<Queue<char>> ParseExpressions(IEnumerable<string> input)
    {
        return new List<Queue<char>>(input.Select(ParseTokens));
    }
    
    private static Queue<char> ParseTokens(string expression)
    {
        return new Queue<char>(expression.RemoveWhitespace());
    }
}