namespace Problems.Y2020.D18;

public static class Operators
{
    public const char Add = '+';
    public const char Mul = '*';
    public const char Open = '(';
    public const char Close = ')';

    public delegate long Operator(long lhs, long rhs); 
    
    public static readonly Dictionary<char, Operator> Delegates = new()
    {
        { Add, (a, b) => a + b },
        { Mul, (a, b) => a * b },
    };
    public static readonly Dictionary<char, int> EqualPrecedence = new()
    {
        { Add, 0 },
        { Mul, 0 },
    };
    public static readonly Dictionary<char, int> AddPrecedence = new()
    {
        { Add, 0 },
        { Mul, 1 },
    };
}