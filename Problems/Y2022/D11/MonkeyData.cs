using System.Text.RegularExpressions;
using Utilities.Extensions;

namespace Problems.Y2022.D11;

public static class MonkeyData
{
    private const int DataLinesPerMonkey = 6;
    private const char Multiply = '*';
    private const string VariableSignifier = "old";
    private const string NumberRegex = @"(\d+)";
    
    public static Dictionary<int, Monkey> Parse(IEnumerable<string> input, bool applyBoredDivisor)
    {
        var monkeys = new Dictionary<int, Monkey>();
        var chunks = input.Chunk(DataLinesPerMonkey + 1);
        var divisorsSet = new HashSet<int>();

        foreach (var chunk in chunks)
        {
            var (index, monkey) = ParseMonkey(chunk, applyBoredDivisor, divisorsSet);
            monkeys.Add(index, monkey);
        }

        Monkey.TestDivisorProduct = divisorsSet.Aggregate((i, j) => i * j);
        return monkeys;
    }

    private static (int, Monkey) ParseMonkey(IList<string> chunk, bool applyBoredDivisor, ISet<int> divisorsSet)
    {
        var index = int.Parse(Regex.Match(chunk[0], NumberRegex).Value);
        var items = Regex.Matches(chunk[1], NumberRegex).Select(m => int.Parse(m.Value));
        var divisor = int.Parse(Regex.Match(chunk[3], NumberRegex).Value);
        var success = int.Parse(Regex.Match(chunk[4], NumberRegex).Value);
        var failure = int.Parse(Regex.Match(chunk[5], NumberRegex).Value);

        var variableCount = Regex.Matches(chunk[2], VariableSignifier).Count;
        var isMultiplicative = chunk[2].Contains(Multiply);
        var op = !isMultiplicative ? Operator.Add : variableCount > 1 ? Operator.Square : Operator.Multiply;
        var arg = op == Operator.Square ? 0 : int.Parse(Regex.Match(chunk[2], NumberRegex).Value);

        divisorsSet.EnsureContains(divisor);
        return (index, new Monkey(items)
        {
            InspectionOperator = op,
            InspectionOperand = arg,
            TestDivisor = divisor,
            ThrowToOnSuccess = success,
            ThrowToOnFailure = failure,
            ApplyBoredDivisor = applyBoredDivisor,
        });
    } 
}