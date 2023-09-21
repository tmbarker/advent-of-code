namespace Utilities.Language.ContextFree;

/// <summary>
/// A readonly value type representing a production rule in a Context Free Grammar (CFG)
/// </summary>
public readonly struct Production : IEquatable<Production>
{
    private const char YieldDelimiter = ',';
    private readonly string _yieldHash;

    public string NonTerminal { get; }
    public IReadOnlyList<string> Yields { get; }
    
    public Production(string nonTerminal, IEnumerable<string> yields)
    {
        NonTerminal = nonTerminal;
        Yields = new List<string>(collection: yields);
        
        if (Yields.Count == 0)
        {
            throw new ArgumentException($"{nameof(Production)} must yield at least one terminal or non-terminal");
        }
        
        if (Yields.Any(v => v.Contains(YieldDelimiter)))
        {
            throw new ArgumentException("Yield delimiter cannot appear in the production outputs");
        }
        
        _yieldHash = string.Join(YieldDelimiter, Yields);
    }

    public Production(string nonTerminal, string yields) : this(nonTerminal, yields: new[] { yields })
    {
    }
    
    public bool Equals(Production other)
    {
        return NonTerminal == other.NonTerminal && _yieldHash == other._yieldHash;
    }

    public override bool Equals(object? obj)
    {
        return obj is Production other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(NonTerminal, _yieldHash);
    }

    public static bool operator ==(Production left, Production right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Production left, Production right)
    {
        return !left.Equals(right);
    }
}