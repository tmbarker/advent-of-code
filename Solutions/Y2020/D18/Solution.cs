using Utilities.Extensions;

namespace Solutions.Y2020.D18;

using Tokens = Queue<char>;
using Precedences = IReadOnlyDictionary<char, int>;

[PuzzleInfo("Operation Order", Topics.StringParsing|Topics.Math, Difficulty.Medium, favourite: true)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var expressions = ParseExpressions(GetInputLines());
        return part switch
        {
            1 => expressions.Sum(tokens => Evaluate(tokens, Operators.EqualPrecedence)),
            2 => expressions.Sum(tokens => Evaluate(tokens, Operators.AddPrecedence)),
            _ => PuzzleNotSolvedString
        };
    }

    private static long Evaluate(Tokens tokens, Precedences precedences)
    {
        var operators = new Stack<char>();
        var literals = new Stack<long>();

        while (tokens.Count > 0)
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
        while (operators.Count != 0)
        {
            literals.Push(Operators.Delegates[operators.Pop()](
                lhs: literals.Pop(),
                rhs: literals.Pop()));
        }
        return literals.Single();
    }

    private static void HandleOperatorToken(char token, Stack<char> operators, Stack<long> literals, Precedences precedences)
    {
        while (operators.Count != 0 && precedences[operators.Peek()] <= precedences[token])
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