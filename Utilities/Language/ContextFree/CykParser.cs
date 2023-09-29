using System.Runtime.CompilerServices;
using Utilities.Graph;

namespace Utilities.Language.ContextFree;

/// <summary>
/// A parser which executes the Cocke-Younger-Kasami (CYK) recognition algorithm
/// </summary>
public static class CykParser
{
    /// <summary>
    /// Execute the Cocke-Younger-Kasami (CYK) algorithm to attempt to parse the input using the provided grammar
    /// </summary>
    /// <param name="grammar">The grammar definition</param>
    /// <param name="sentence">The input to attempt to parse</param>
    /// <returns>A boolean representing if the input is recognized as part of the grammar</returns>
    public static bool Recognize(Grammar grammar, List<string> sentence)
    {
        return RecognizeInternal(
            grammar: grammar,
            sentence: sentence,
            buildParseTree: false,
            tree: out _);
    }
    
    /// <summary>
    /// Execute the Cocke-Younger-Kasami (CYK) algorithm to attempt to parse the input using the provided grammar
    /// </summary>
    /// <param name="grammar">The grammar definition</param>
    /// <param name="sentence">The input to attempt to parse</param>
    /// <param name="parseTreeRoot">The root node of a valid parse tree if the input is recognized</param>
    /// <returns>A boolean representing if the input is recognized as part of the grammar</returns>
    public static bool Recognize(Grammar grammar, List<string> sentence, out GenericTreeNode<string>? parseTreeRoot)
    {
        return RecognizeInternal(
            grammar: grammar,
            sentence: sentence,
            buildParseTree: true,
            tree: out parseTreeRoot);
    }

    /// <summary>
    /// Execute the Cocke-Younger-Kasami (CYK) algorithm to attempt to parse the input using the provided grammar
    /// </summary>
    /// <param name="grammar">The grammar definition</param>
    /// <param name="sentence">The input to attempt to parse</param>
    /// <param name="buildParseTree">Attempt to build a parse tree when set</param>
    /// <param name="tree">The root node of a valid parse tree if the input is recognized
    /// and <paramref name="buildParseTree"/> is set</param>
    /// <returns>A boolean representing if the input is recognized as part of the grammar</returns>
    private static bool RecognizeInternal(
        Grammar grammar, 
        IReadOnlyList<string> sentence, 
        bool buildParseTree,
        out GenericTreeNode<string>? tree)
    {
        var n = sentence.Count;
        var table = new CykTable();

        var units = grammar.Productions
            .Where(p => IsUnitTerminal(p, grammar.Terminals))
            .ToHashSet();
        var bins = grammar.Productions
            .Where(p => IsBinaryNonTerminal(p, grammar.NonTerminals))
            .ToHashSet();
        
        for (var s = 0; s < n; s++)
        {
            foreach (var unit in units)
            {
                if (unit.Yields[0] == sentence[s])
                {
                    table.P[(1, s, unit.NonTerminal)] = true;
                }
            }   
        }

        for (var l = 2; l <= n; l++)        // Length of span
        for (var s = 0; s <= n - l; s++)    // Start of span
        for (var p = 1; p < l; p++)         // Partition of span
        {
            foreach (var bin in bins)
            {
                if (!table.P[(p, s, bin.Yields[0])] || !table.P[(l - p, s + p, bin.Yields[1])])
                {
                    continue;
                }

                table.P[(l, s, bin.NonTerminal)] = true;
                table.B[(l, s, bin.NonTerminal)].Add((p, bin.Yields[0], bin.Yields[1]));
            }
        }

        var recognize = table.P[(n, 0, grammar.Start)];
        tree = recognize && buildParseTree
            ? BuildParseTree(nt: grammar.Start, s: 0, l: n, t: table, i: sentence)
            : null;
        
        return recognize;
    }
    
    /// <summary>
    /// Does this production yield exactly one terminal?
    /// </summary>
    /// <param name="p">The production</param>
    /// <param name="t">The set of terminals</param>
    /// <returns>A boolean representing if <paramref name="p"/> produces exactly one terminal</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsUnitTerminal(Production p, IReadOnlySet<string> t)
    {
        return p.Yields.Count == 1 && t.Contains(p.Yields[0]);
    }

    /// <summary>
    /// Does this production yield exactly two non terminals? 
    /// </summary>
    /// <param name="p">The production</param>
    /// <param name="nt">The set of non-terminals</param>
    /// <returns>A boolean representing if <paramref name="p"/> produces exactly two non-terminals</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsBinaryNonTerminal(Production p, IReadOnlySet<string> nt)
    {
        return p.Yields.Count == 2 && nt.Contains(p.Yields[0]) && nt.Contains(p.Yields[1]);
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
            node.Children.Add(item: BuildParseTree(
                nt: lhs, 
                s: s, 
                l: p, 
                t: t, 
                i: i));
            node.Children.Add(item: BuildParseTree(
                nt: rhs,
                s: s + p,
                l: l - p,
                t: t,
                i: i));

            //  Stop after the first valid parse tree
            //
            break;
        }

        return node;
    }
}