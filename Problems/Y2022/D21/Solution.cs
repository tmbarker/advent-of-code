using Problems.Y2022.Common;

namespace Problems.Y2022.D21;

/// <summary>
/// Monkey Math: https://adventofcode.com/2022/day/21
/// </summary>
public class Solution : SolutionBase2022
{
    private const string Root = "root";
    private const string Human = "humn";
    
    private static readonly Dictionary<char, Operator> OperatorMap = new()
    {
        { '+', Operator.Add },
        { '-', Operator.Subtract },
        { '*', Operator.Multiply },
        { '/', Operator.Divide },
    };
    private static readonly Dictionary<Operator, char> SymbolMap = new()
    {
        { Operator.Add, '+' },
        { Operator.Subtract, '-' },
        { Operator.Multiply, '*' },
        { Operator.Divide, '/' },
        { Operator.Equality, '=' },
    };
    private static readonly Dictionary<Operator, Operator> InversesMap = new()
    {
        { Operator.Add, Operator.Subtract },
        { Operator.Subtract, Operator.Add },
        { Operator.Multiply, Operator.Divide },
        { Operator.Divide, Operator.Multiply },
    };

    public override int Day => 21;
    
    public override object Run(int part)
    {
        var jobs = ParseJobs(GetInput());
        return part switch
        {
            0 => ComputeJobResult(Root, jobs),
            1 => BuildEquationString(Root, Human, jobs),
            _ => ProblemNotSolvedString,
        };
    }

    private static long ComputeJobResult(string assignee, IList<Job> jobs)
    {
        var jobMap = jobs.ToDictionary(j => j.Assignee);
        var resultMap = jobs
            .Where(j => j.Operator == Operator.Identity)
            .ToDictionary(identity => identity.Assignee, identity => identity.Value);

        return ExecuteJob(assignee, jobMap, resultMap);
    }
    
    // TODO: Solve part 2 algebraically instead of printing the equation to plug into an online solver
    private static string BuildEquationString(string assignee, string constraint, IList<Job> jobs)
    {
        var jobMap = jobs.ToDictionary(j => j.Assignee);
        var resultMap = jobs
            .Where(j => j.Operator == Operator.Identity && j.Assignee != constraint)
            .ToDictionary(identity => identity.Assignee, identity => identity.Value);

        var lhs = jobMap[assignee].LhsOperand;
        var rhs = jobMap[assignee].RhsOperand;

        var lhsExpression = FormJobExpressionString(lhs, constraint, jobMap, resultMap);
        var rhsExpression = FormJobExpressionString(rhs, constraint, jobMap, resultMap);
        
        return $"{lhsExpression} {SymbolMap[Operator.Equality]} {rhsExpression}";
    }


    private static string FormJobExpressionString(string assignee, string constraint, Dictionary<string, Job> jobs,
        Dictionary<string, long> results)
    {
        if (assignee == constraint)
        {
            return constraint;
        }
        
        if (results.ContainsKey(assignee))
        {
            return results[assignee].ToString();
        }
        
        if (TryExecuteJob(assignee, constraint, jobs, results))
        {
            return results[assignee].ToString();
        }
        
        var job = jobs[assignee];
        var lhs = FormJobExpressionString(job.LhsOperand, constraint, jobs, results);
        var rhs = FormJobExpressionString(job.RhsOperand, constraint, jobs, results);

        return $"({lhs} {SymbolMap[job.Operator]} {rhs})";
    }

    private static bool TryExecuteJob(string assignee, string constraint, IDictionary<string, Job> jobs, IDictionary<string, long> results)
    {
        if (assignee == constraint)
        {
            return false;
        }
        
        if (results.ContainsKey(assignee))
        {
            return true;
        }

        var job = jobs[assignee];
        var lhs = job.LhsOperand;
        var rhs = job.RhsOperand;

        if (!results.ContainsKey(lhs))
        {
            TryExecuteJob(lhs, constraint, jobs, results);
        }
        
        if (!results.ContainsKey(rhs))
        {
            TryExecuteJob(rhs, constraint, jobs, results);
        }

        if (results.ContainsKey(lhs) && results.ContainsKey(rhs))
        {
            results[assignee] = ExecuteOperation(job.Operator, results[lhs], results[rhs]);
            return true;
        }

        return false;
    }
    
    private static long ExecuteJob(string assignee, IDictionary<string, Job> jobs, IDictionary<string, long> results)
    {
        if (results.ContainsKey(assignee))
        {
            return results[assignee];
        }

        var job = jobs[assignee];
        var lhs = job.LhsOperand;
        var rhs = job.RhsOperand;

        if (!results.ContainsKey(lhs))
        {
            results[lhs] = ExecuteJob(lhs, jobs, results);
        }
        
        if (!results.ContainsKey(rhs))
        {
            results[rhs] = ExecuteJob(rhs, jobs, results);
        }
        
        results[assignee] = ExecuteOperation(job.Operator, results[lhs], results[rhs]);
        return results[assignee];
    }

    private static long ExecuteOperation(Operator op, long lhs, long rhs)
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
            case Operator.Equality:
            default:
                throw new ArgumentOutOfRangeException(nameof(op), op, null);
        }
    }
    
    private static IList<Job> ParseJobs(IEnumerable<string> lines)
    {
        return lines.Select(ParseJob).ToList();
    }

    private static Job ParseJob(string line)
    {
        var elements = line.Split(':', StringSplitOptions.TrimEntries);
        var assignee = elements[0];

        var arguments = elements[1].Split(' ');
        if (arguments.Length == 1)
        {
            return new Job
            {
                Assignee = assignee,
                Operator = Operator.Identity,
                Value = long.Parse(elements[1]),
            };
        }

        return new Job
        {
            Assignee = assignee,
            Operator = OperatorMap[arguments[1][0]],
            LhsOperand = arguments[0],
            RhsOperand = arguments[2],
        };
    }
}