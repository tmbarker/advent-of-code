using Utilities.Graph;

namespace Utilities.Language.ContextFree;

/// <summary>
///     A parser which executes the Cocke-Younger-Kasami (CYK) recognition algorithm
/// </summary>
public class CykParser
{
    private readonly Grammar _grammar;
    private readonly Production[] _units;
    private readonly Production[] _bins;

    /// <summary>
    ///     Instantiate a <see cref="CykParser" /> specific to the provided CNF <paramref name="grammar" />.
    /// </summary>
    /// <param name="grammar">A CNF <see cref="Grammar" /> instance</param>
    /// <exception cref="ArgumentException">
    ///     The specified <paramref name="grammar" /> must be in Chomsky Normal
    ///     Form (CNF)
    /// </exception>
    public CykParser(Grammar grammar)
    {
        if (!grammar.IsInCnf)
        {
            throw new ArgumentException(
                message: $"{nameof(grammar)} must be in Chomsky Normal Form (CNF)",
                paramName: nameof(grammar));
        }

        _grammar = grammar;
        _units = grammar.Productions.Where(p => Grammar.IsUnitTerminal(p, grammar.Terminals)).ToArray();
        _bins = grammar.Productions.Where(p => Grammar.IsBinaryNonTerminal(p, grammar.NonTerminals)).ToArray();
    }

    /// <summary>
    ///     Execute the Cocke-Younger-Kasami (CYK) algorithm to attempt to parse the input using the provided grammar
    /// </summary>
    /// <param name="sentence">The input to attempt to parse</param>
    /// <returns>A boolean representing if the input is recognized as part of the grammar</returns>
    public bool Recognize(IReadOnlyList<string> sentence)
    {
        return RecognizeInternal(
            sentence: sentence,
            buildParseTree: false,
            parseTree: out _);
    }

    /// <summary>
    ///     Execute the Cocke-Younger-Kasami (CYK) algorithm to attempt to parse the input using the provided grammar
    /// </summary>
    /// <param name="sentence">The input to attempt to parse</param>
    /// <param name="parseTree">A tree representing a single valid parse if the input is recognized</param>
    /// <returns>A boolean representing if the input is recognized as part of the grammar</returns>
    public bool Recognize(IReadOnlyList<string> sentence, out BinaryTree<string>? parseTree)
    {
        return RecognizeInternal(
            sentence: sentence,
            buildParseTree: true,
            parseTree: out parseTree);
    }

    /// <summary>
    ///     Execute the Cocke-Younger-Kasami (CYK) algorithm to attempt to parse the input using the provided grammar
    /// </summary>
    /// <param name="sentence">The input to attempt to parse</param>
    /// <param name="buildParseTree">Attempt to build a parse parseTreeRoot when set</param>
    /// <param name="parseTree">
    ///     A tree representing a single valid parse if the input is recognized
    ///     and <paramref name="buildParseTree" /> is set
    /// </param>
    /// <returns>A boolean representing if the input is recognized as part of the grammar</returns>
    private bool RecognizeInternal(IReadOnlyList<string> sentence,
        bool buildParseTree,
        out BinaryTree<string>? parseTree)
    {
        var n = sentence.Count;
        var table = new CykTable();

        for (var s = 0; s < n; s++)
        {
            foreach (var unit in _units)
            {
                if (unit.Yields[0] == sentence[s])
                {
                    table.P[(1, s, unit.NonTerminal)] = true;
                }
            }
        }

        for (var l = 2; l <= n; l++) // Length of span
        for (var s = 0; s <= n - l; s++) // Start of span
        for (var p = 1; p < l; p++) // Partition of span
        {
            foreach (var bin in _bins)
            {
                if (!table.P[(p, s, bin.Yields[0])] || !table.P[(l - p, s + p, bin.Yields[1])])
                {
                    continue;
                }

                table.P[(l, s, bin.NonTerminal)] = true;
                table.B[(l, s, bin.NonTerminal)].Add((p, bin.Yields[0], bin.Yields[1]));
            }
        }

        var recognize = table.P[(n, 0, _grammar.Start)];
        parseTree = recognize && buildParseTree
            ? new BinaryTree<string>(root: BuildParseTree(nt: _grammar.Start, s: 0, l: n, t: table, i: sentence))
            : null;

        return recognize;
    }

    /// <summary>
    ///     Build a single valid parse parse tree.
    /// </summary>
    /// <param name="nt">The non-terminal to parse</param>
    /// <param name="s">The substring start position</param>
    /// <param name="l">The substring length</param>
    /// <param name="t">The populated <see cref="CykTable" /></param>
    /// <param name="i">The input sentence</param>
    /// <returns>A valid parse tree</returns>
    private static BinaryTreeNode<string> BuildParseTree(string nt, int s, int l, CykTable t, IReadOnlyList<string> i)
    {
        var node = new BinaryTreeNode<string>(value: nt);
        if (l == 1)
        {
            node.Left = new BinaryTreeNode<string>(value: i[s]);
            return node;
        }

        foreach (var (p, lhs, rhs) in t.B[(l, s, nt)])
        {
            node.Left = BuildParseTree(
                nt: lhs,
                s: s,
                l: p,
                t: t,
                i: i);
            node.Right = BuildParseTree(
                nt: rhs,
                s: s + p,
                l: l - p,
                t: t,
                i: i);

            //  Stop after the first valid parse
            //
            break;
        }

        return node;
    }
}