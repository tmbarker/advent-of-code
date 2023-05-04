using System.Text.RegularExpressions;
using Utilities.Extensions;

namespace Problems.Y2022.D11;

public static class MonkeyData
{
    private const int DataLinesPerMonkey = 6;
    private const char Multiply = '*';
    private const string VariableSignifier = "old";

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
        var index   = chunk[0].ParseInt();
        var divisor = chunk[3].ParseInt();
        var success = chunk[4].ParseInt();
        var failure = chunk[5].ParseInt();
        var items = chunk[1].ParseInts();

        var variableCount = Regex.Matches(chunk[2], VariableSignifier).Count;
        var isMultiplicative = chunk[2].Contains(Multiply);
        var op = !isMultiplicative ? Operator.Add : variableCount > 1 ? Operator.Square : Operator.Multiply;
        var arg = op == Operator.Square ? 0 : chunk[2].ParseInt();

        divisorsSet.Add(divisor);
        return (index, new Monkey(items)
        {
            InspectionOperator = op,
            InspectionOperand = arg,
            TestDivisor = divisor,
            ThrowToOnSuccess = success,
            ThrowToOnFailure = failure,
            ApplyBoredDivisor = applyBoredDivisor
        });
    } 
}