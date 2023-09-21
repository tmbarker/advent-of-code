using Utilities.Graph;

namespace Utilities.Language.ContextFree;

/// <summary>
/// A CYK Parser which exposes <see cref="Recognize"/>
/// </summary>
public static class CykParser
{
    /// <summary>
    /// Execute the Cocke-Younger-Kasami (CYK) algorithm to attempt to parse the input using the provided grammar
    /// </summary>
    /// <param name="grammar">The grammar definition</param>
    /// <param name="sentence">The input to attempt to parse</param>
    /// <param name="parseTreeRoot">The root node of a valid parse tree if the input is recognized</param>
    /// <returns>A boolean representing if the input is recognized as part of the grammar</returns>
    public static bool Recognize(Grammar grammar, List<string> sentence, out GenericTreeNode<string>? parseTreeRoot)
    {
        var n = sentence.Count;
        var table = new CykTable();

        for (var s = 0; s < n; s++)
        {
            foreach (var production in grammar.Productions)
            {
                if (IsUnitTerminal(production, grammar.Terminals, out var terminal) && terminal == sentence[s])
                {
                    table.P[(1, s, production.NonTerminal)] = true;
                }
            }   
        }

        for (var l = 2; l <= n; l++)        // Length of span
        for (var s = 0; s <= n - l; s++)    // Start of span
        for (var p = 1; p < l; p++)         // Partition of span
        {
            foreach (var production in grammar.Productions)
            {
                if (!IsBinaryNonTerminal(production, grammar.NonTerminals, out var lhs, out var rhs))
                {
                    continue;
                }

                if (!table.P[(p, s, lhs)] || !table.P[(l - p, s + p, rhs)])
                {
                    continue;
                }

                table.P[(l, s, production.NonTerminal)] = true;
                table.B[(l, s, production.NonTerminal)].Add((p, lhs, rhs));
            }
        }

        var recognize = table.P[(n, 0, grammar.Start)];
        parseTreeRoot = recognize
            ? BuildParseTree(nt: grammar.Start, s: 0, l: n, t: table, i: sentence)
            : null;

        return recognize;
    }
    
    /// <summary>
    /// Does this production produce exactly one terminal?
    /// </summary>
    /// <param name="p">The production</param>
    /// <param name="t">The set of terminals</param>
    /// <param name="ut">The yielded terminal</param>
    /// <returns>A boolean representing if <paramref name="p"/> produces exactly one terminal</returns>
    private static bool IsUnitTerminal(Production p, IReadOnlySet<string> t, out string ut)
    {
        ut = p.Yields[0];
        return p.Yields.Count == 1 && t.Contains(ut);
    }

    /// <summary>
    /// Does this production produce exactly two non terminals? 
    /// </summary>
    /// <param name="p">The production</param>
    /// <param name="nt">The set of non-terminals</param>
    /// <param name="lhs">the yielded left non-terminal</param>
    /// <param name="rhs">The yielded right non-terminal</param>
    /// <returns>A boolean representing if <paramref name="p"/> produces exactly two non-terminals</returns>
    private static bool IsBinaryNonTerminal(Production p, IReadOnlySet<string> nt, out string lhs, out string rhs)
    {
        if (p.Yields.Count == 2 && p.Yields.All(nt.Contains))
        {
            lhs = p.Yields[0];
            rhs = p.Yields[1];
            return true;
        }
        
        lhs = string.Empty;
        rhs = string.Empty;
        return false;
    }
    
    /// <summary>
    /// Build a single valid parse tree
    /// </summary>
    /// <param name="nt">The non-terminal to parse</param>
    /// <param name="s">The substring start position</param>
    /// <param name="l">The substring length</param>
    /// <param name="t">The populated <see cref="CykTable"/></param>
    /// <param name="i">The input sentence</param>
    /// <returns>A valid parse tree</returns>
    private static GenericTreeNode<string> BuildParseTree(string nt, int s, int l, CykTable t, IReadOnlyList<string> i) 
    {
        var node = new GenericTreeNode<string>(value: nt);
        if (l == 1)
        {
            node.Children.Add(item: new GenericTreeNode<string>(value: i[s]));
            return node;
        }

        foreach (var (p, lhs, rhs) in t.B[(l, s, nt)])
        {
            node.Children.Add(item: BuildParseTree(nt: lhs, s: s, l: p, t: t, i: i));
            node.Children.Add(item: BuildParseTree(nt: rhs, s: s + p, l: l - p, t: t, i: i));

            //  Stop after the first valid parse tree
            //
            break;
        }

        return node;
    }
}