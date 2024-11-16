using System.Text;

namespace Utilities.Language.ContextFree;

/// <summary>
///     Represents a Context Free Grammar.
/// </summary>
public sealed partial class Grammar
{
    public string Start { get; }
    public string? Epsilon { get; }
    public IReadOnlyList<Production> Productions { get; }
    public IReadOnlySet<string> NonTerminals { get; }
    public IReadOnlySet<string> Terminals { get; }

    public bool IsInCnf => Productions.All(p =>
        IsUnitTerminal(p) || IsBinaryNonTerminal(p) || (IsEpsilon(p) && p.NonTerminal == Start));

    /// <summary>
    ///     Create a Context Free Grammar.
    /// </summary>
    /// <param name="start">The starting non-terminal</param>
    /// <param name="productions">The production rules of the grammar</param>
    /// <exception cref="ArgumentException">
    ///     The start symbol does not appear as the non-terminal in any of the
    ///     production rules
    /// </exception>
    /// <exception cref="ArgumentException">The terminals and non-terminals are <b>not</b> disjoint</exception>
    public Grammar(string start, IEnumerable<Production> productions)
    {
        Start = start;
        Productions = new List<Production>(productions);
        NonTerminals = Productions
            .Select(p => p.NonTerminal)
            .ToHashSet();
        Terminals = Productions
            .SelectMany(production => production.Yields)
            .Where(s => !NonTerminals.Contains(s))
            .ToHashSet();
        
        if (!NonTerminals.Contains(Start))
        {
            throw new ArgumentException(message: "Start symbol must be in the set of non-terminals");
        }

        if (Terminals.Intersect(NonTerminals).Any())
        {
            throw new ArgumentException(message: "Terminals and non-terminals must be disjoint");
        }
    }

    /// <summary>
    ///     Create a Context Free Grammar.
    /// </summary>
    /// <param name="start">The starting non-terminal</param>
    /// <param name="epsilon">The epsilon symbol, denoting the empty string</param>
    /// <param name="productions">The production rules of the grammar</param>
    /// <exception cref="ArgumentException">
    ///     The start symbol does not appear as the non-terminal in any of the
    ///     production rules
    /// </exception>
    /// <exception cref="ArgumentException">The terminals and non-terminals are <b>not</b> disjoint</exception>
    /// <exception cref="ArgumentException">Epsilon appears as the non-terminal in a production rule</exception>
    /// <exception cref="ArgumentException">
    ///     A production rule yields both a terminals/non-terminals <b>and</b>
    ///     epsilon
    /// </exception>
    public Grammar(string start, string? epsilon, IEnumerable<Production> productions) : this(start, productions)
    {
        Epsilon = epsilon;
        if (epsilon == null)
        {
            return;
        }
        
        if (string.IsNullOrWhiteSpace(epsilon))
        {
            throw new ArgumentException(message: "Epsilon cannot be null or empty");
        }

        if (NonTerminals.Contains(epsilon))
        {
            throw new ArgumentException(message: "Epsilon cannot appear as the non-terminal in a production rule");
        }
        
        if (Productions.Any(p => p.Yields.Count > 1 && p.Yields.Contains(Epsilon)))
        {
            throw new ArgumentException(message: "Productions cannot yield both terminals/non-terminals and epsilon");
        }
    }

    /// <summary>
    ///     Print a summarization of the grammar to the console.
    /// </summary>
    public void Print()
    {
        Console.Write(this);
    }
    
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{nameof(Start)}: {Start}");

        if (Epsilon != null)
        {
            sb.AppendLine($"{nameof(Epsilon)}: {Epsilon}");
        }
        
        sb.AppendLine($"{nameof(Productions)}:");
        foreach (var group in Productions.GroupBy(p => p.NonTerminal))
        {
            var yields = group.Select(p => string.Join(" ", p.Yields));
            var alternations = string.Join(" | ", yields);
            sb.AppendLine($"\t{group.Key} -> {alternations}");
        }
        
        return sb.ToString();
    }
}