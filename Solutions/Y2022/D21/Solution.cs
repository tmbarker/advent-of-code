namespace Solutions.Y2022.D21;

[PuzzleInfo("Monkey Math", Topics.StringParsing|Topics.Math, Difficulty.Hard)]
public sealed class Solution : SolutionBase
{
    private const string Root = "root";
    private const string Unknown = "humn";

    private static readonly HashSet<Operator> CommutativeOperators = [Operator.Add, Operator.Multiply];
    private static readonly Dictionary<Operator, Operator> InverseOperators = new()
    {
        { Operator.Add, Operator.Subtract },
        { Operator.Subtract, Operator.Add },
        { Operator.Multiply, Operator.Divide },
        { Operator.Divide, Operator.Multiply }
    };

    public override object Run(int part)
    {
        var expressions = ParseExpressions(GetInputLines());
        return part switch
        {
            1 => EvaluateExpression(id: Root, expressions),
            2 => SolveForUnknown(equationId: Root, Unknown, expressions),
            _ => ProblemNotSolvedString
        };
    }

    private static long EvaluateExpression(string id, IList<Expression> flatExpressions)
    {
        var expressions = flatExpressions.ToDictionary(e => e.Id);
        var results = EvaluateConstantExpressions(flatExpressions, string.Empty);

        SimplifyExpressionTerms(id, string.Empty, expressions, results);

        return results[id];
    }

    private static long SolveForUnknown(string equationId, string unknown, IList<Expression> flatExpressions)
    {
        var expressions = flatExpressions.ToDictionary(e => e.Id);
        var results = EvaluateConstantExpressions(flatExpressions, unknown);

        var equation = expressions[equationId];
        var lhs = equation.Lhs;
        var rhs = equation.Rhs;
        
        SimplifyExpressionTerms(lhs, unknown, expressions, results);
        SimplifyExpressionTerms(rhs, unknown, expressions, results);
        
        var unsolved = !results.ContainsKey(lhs) ? lhs : rhs;
        var solved = results.ContainsKey(lhs) ? lhs : rhs;
        
        var algebra = GetOperations(unsolved, unknown, expressions, results);
        var result = ReverseOperations(algebra, results[solved]);

        return result;
    }

    private static long ReverseOperations(Queue<AlgebraicOperation> operations, long equals)
    {
        while (operations.Count > 0)
        {
            var op = operations.Dequeue();
            equals = SolveAlgebraicOperation(op, equals);
        }

        return equals;
    }
    
    private static long SolveAlgebraicOperation(AlgebraicOperation operation, long equals)
    {
        var op = operation.Operator;
        var known = operation.KnownOperand;
        var knownOnLhs = operation.KnownOperandOnLhs;
        
        if (CommutativeOperators.Contains(operation.Operator))
        {
            return EvaluateArithmeticOperation(InverseOperators[op], lhs: equals, rhs: known);
        }
        
        //  NOTE: The order of operands in non-commutative operators (e.g. division, subtraction) must be respected
        //
        return knownOnLhs
            ? EvaluateArithmeticOperation(op, lhs: known, rhs: equals)
            : EvaluateArithmeticOperation(InverseOperators[op], lhs: equals, rhs: known);
    }
    
    private static Queue<AlgebraicOperation> GetOperations(string expressionId, string unknown, IDictionary<string, Expression> expressions, IDictionary<string, long> results)
    {
        var operations = new Queue<AlgebraicOperation>();
        var currentExpId = expressionId;
        
        while (true)
        {
            var exp = expressions[currentExpId];
            var lhs = exp.Lhs;
            var rhs = exp.Rhs;
            var lhsSolved = results.ContainsKey(lhs);

            operations.Enqueue(new AlgebraicOperation
            {
                Operator = exp.Operator,
                KnownOperand = results[lhsSolved ? lhs : rhs],
                KnownOperandOnLhs = lhsSolved
            });

            var unsolvedExpr = !results.ContainsKey(lhs) ? lhs : rhs;
            if (unsolvedExpr == unknown)
            {
                break;
            }

            currentExpId = unsolvedExpr;
        }

        return operations;
    }

    private static void SimplifyExpressionTerms(string id, string unknown, IDictionary<string, Expression> expressions, IDictionary<string, long> results)
    {
        if (id == unknown || results.ContainsKey(id))
        {
            return;
        }

        var exp = expressions[id];
        var lhs = exp.Lhs;
        var rhs = exp.Rhs;
        
        SimplifyExpressionTerms(lhs, unknown, expressions, results);
        SimplifyExpressionTerms(rhs, unknown, expressions, results);

        if (results.ContainsKey(lhs) && results.ContainsKey(rhs))
        {
            results[id] = EvaluateArithmeticOperation(exp.Operator, results[lhs], results[rhs]);
        }
    }

    private static long EvaluateArithmeticOperation(Operator op, long lhs, long rhs)
    {
        switch (op)
        {
            case Operator.Add:
                return lhs + rhs;
            case Operator.Subtract:
                return lhs - rhs;
            case Operator.Multiply:
                return lhs * rhs;
            case Operator.Divide:
                return lhs / rhs;
            case Operator.Identity:
            default:
                throw new ArgumentOutOfRangeException(nameof(op), op, null);
        }
    }
    
    private static Dictionary<string, long> EvaluateConstantExpressions(IEnumerable<Expression> expressions, string unknown)
    {
        return expressions
            .Where(e => e.Operator == Operator.Identity && e.Id != unknown)
            .ToDictionary(identity => identity.Id, identity => identity.Value);
    }
    
    private static IList<Expression> ParseExpressions(IEnumerable<string> lines)
    {
        return lines.Select(ExpressionFactory.Parse).ToList();
    }
}