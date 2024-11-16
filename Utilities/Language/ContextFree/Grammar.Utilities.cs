namespace Utilities.Language.ContextFree;

public sealed partial class Grammar
{
    /// <summary>
    ///     Does this production yield the empty string?
    /// </summary>
    public bool IsEpsilon(Production p)
    {
        return Epsilon != null && p.Yields.Count == 1 && p.Yields[0] == Epsilon;
    }
    
    /// <summary>
    ///     Does this production yield exactly one terminal?
    /// </summary>
    public bool IsUnitTerminal(Production p)
    {
        return p.Yields.Count == 1 && Terminals.Contains(p.Yields[0]);
    }

    /// <summary>
    ///     Does this production yield exactly two non-terminals?
    /// </summary>
    public bool IsBinaryNonTerminal(Production p)
    {
        return p.Yields.Count == 2 && p.Yields.All(NonTerminals.Contains);
    }

    /// <summary>
    ///     Does this production yield both terminals and non-terminals?
    /// </summary>
    public bool IsNonSolitary(Production p)
    {
        return p.Yields.Any(NonTerminals.Contains) && p.Yields.Any(Terminals.Contains);
    }

    /// <summary>
    ///     Does this production yield more than two non-terminals?
    /// </summary>
    public bool IsSupraBinaryNonTerminal(Production p)
    {
        return p.Yields.Count > 2 && p.Yields.All(NonTerminals.Contains);
    }

    /// <summary>
    ///     Does this production yield exactly one non-terminals?
    /// </summary>
    public bool IsUnitNonTerminal(Production p)
    {
        return p.Yields.Count == 1 && NonTerminals.Contains(p.Yields[0]);
    }
}