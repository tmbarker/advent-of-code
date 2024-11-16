namespace Utilities.Language.ContextFree;

/// <summary>
///     A readonly value type representing a production rule in a Context Free Grammar (CFG)
/// </summary>
public readonly struct Production : IEquatable<Production>
{
    public string NonTerminal { get; }
    public IReadOnlyList<string> Yields { get; }

    public Production(string nonTerminal, IEnumerable<string> yields)
    {
        NonTerminal = nonTerminal;
        Yields = yields.ToArray();

        if (Yields.Count == 0)
        {
            throw new ArgumentException($"{nameof(Production)} must yield at least one terminal or non-terminal");
        }

        if (Yields.Any(string.IsNullOrWhiteSpace))
        {
            throw new ArgumentException($"{nameof(Production)}s cannot contain null or empty yields");
        }
    }

    public Production(string nonTerminal, string yields) : this(nonTerminal, yields: [yields])
    {
    }

    public bool Equals(Production other)
    {
        return NonTerminal == other.NonTerminal && Yields.SequenceEqual(other.Yields);
    }

    public override bool Equals(object? obj)
    {
        return obj is Production other && Equals(other);
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(NonTerminal);
        foreach (var yield in Yields)
        {
            hash.Add(yield);
        }
        return hash.ToHashCode();
    }

    public static bool operator ==(Production left, Production right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Production left, Production right)
    {
        return !left.Equals(right);
    }

    public override string ToString()
    {
        return $"{NonTerminal} -> {string.Join(" ", Yields)}";
    }
}