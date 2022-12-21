namespace Problems.Y2022.D21;

public static class ExpressionFactory
{
    private const char IdDelimiter = ':';
    private const char ElementDelimiter = ' ';
    private static readonly Dictionary<char, Operator> OperatorMap = new()
    {
        { '+', Operator.Add },
        { '-', Operator.Subtract },
        { '*', Operator.Multiply },
        { '/', Operator.Divide },
    };
    
    public static Expression Parse(string expressionStr)
    {
        var elements = expressionStr.Split(IdDelimiter, StringSplitOptions.TrimEntries);
        var arguments = elements[1].Split(ElementDelimiter);
        
        if (arguments.Length == 1)
        {
            return new Expression
            {
                Id = elements[0],
                Operator = Operator.Identity,
                Value = long.Parse(elements[1]),
            };
        }

        return new Expression
        {
            Id = elements[0],
            Operator = OperatorMap[arguments[1][0]],
            Lhs = arguments[0],
            Rhs = arguments[2],
        };
    }
}