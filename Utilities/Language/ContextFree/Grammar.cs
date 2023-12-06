namespace Utilities.Language.ContextFree;

/// <summary>
/// Represents a Context Free Grammar
/// </summary>
public sealed class Grammar
{
    public string Start { get; }
    public IReadOnlyList<Production> Productions { get; }
    public IReadOnlySet<string> NonTerminals { get; }
    public IReadOnlySet<string> Terminals { get; }
    
    public Grammar(string start, IEnumerable<Production> productions)
    {
        Start = start;
        Productions = new List<Production>(productions);

        NonTerminals = Productions
            .Select(production => production.NonTerminal)
            .ToHashSet();
        Terminals = Productions
            .SelectMany(production => production.Yields)
            .Where(@string => !NonTerminals.Contains(@string))
            .ToHashSet();

        if (!NonTerminals.Contains(Start))
        {
            throw new ArgumentException("Start symbol must be in the set of non-terminals");
        }

        if (Terminals.Intersect(NonTerminals).Any())
        {
            throw new ArgumentException("Terminals and non-terminals must be disjoint");
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