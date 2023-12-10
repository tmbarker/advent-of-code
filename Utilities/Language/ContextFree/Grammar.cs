namespace Utilities.Language.ContextFree;

/// <summary>
/// Represents a Context Free Grammar
/// </summary>
public sealed partial class Grammar
{
    public string Start { get; }
    public IReadOnlyList<Production> Productions { get; }
    public IReadOnlySet<string> NonTerminals { get; }
    public IReadOnlySet<string> Terminals { get; }

    public bool IsInCnf => Productions.All(p =>
        IsUnitTerminal(p, Terminals) || IsBinaryNonTerminal(p, NonTerminals));

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
    
    public void Print()
    {
        Console.WriteLine($"{nameof(Start)}: {Start}");
        Console.WriteLine($"{nameof(Productions)}");
        foreach (var production in Productions)
        {
            Console.WriteLine($"\t{production.NonTerminal} -> {string.Join(" ", production.Yields)}");
        }
    }
}