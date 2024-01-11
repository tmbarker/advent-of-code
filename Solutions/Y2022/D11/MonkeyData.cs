using System.Text.RegularExpressions;
using Utilities.Extensions;

namespace Solutions.Y2022.D11;

public static class MonkeyData
{
    public static Dictionary<int, Monkey> Parse(IEnumerable<string> input, bool applyBoredDivisor)
    {
        var monkeys = new Dictionary<int, Monkey>();
        var chunks = input.ChunkByNonEmpty();
        var divisorsSet = new HashSet<int>();

        foreach (var chunk in chunks)
        {
            var (index, monkey) = ParseMonkey(chunk, applyBoredDivisor, divisorsSet);
            monkeys.Add(index, monkey);
        }

        Monkey.TestDivisorProduct = divisorsSet.Aggregate((i, j) => i * j);
        return monkeys;
    }

    private static (int, Monkey) ParseMonkey(IList<string> chunk, bool applyBoredDivisor, ISet<int> divisors)
    {
        var index   = chunk[0].ParseInt();
        var divisor = chunk[3].ParseInt();
        var success = chunk[4].ParseInt();
        var failure = chunk[5].ParseInt();
        var items = chunk[1].ParseInts();

        var variableCount = Regex.Matches(input: chunk[2], pattern: "old").Count;
        var isMultiplicative = chunk[2].Contains('*');
        var op = !isMultiplicative 
            ? Operator.Add 
            : variableCount > 1 ? Operator.Square : Operator.Multiply;
        var arg = op == Operator.Square 
            ? 0 
            : chunk[2].ParseInt();

        divisors.Add(divisor);
        
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