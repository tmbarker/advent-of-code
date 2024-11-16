using System.Diagnostics.CodeAnalysis;
using Utilities.Collections;
using Utilities.Graph;

namespace Utilities.Language.ContextFree;

using Triple = (int, int, string);
using BackRef = (int, string, string);

/// <summary>
///     A parser which executes the Cocke-Younger-Kasami (CYK) recognition algorithm.
/// </summary>
[SuppressMessage("ReSharper", "ForCanBeConvertedToForeach")]
[SuppressMessage("ReSharper", "InvertIf")]
public class CykParser
{
    private readonly Grammar _grammar;
    private readonly Production[] _units;
    private readonly Production[] _bins;
    private readonly Dictionary<string, int> _index;

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
        _units = [..grammar.Productions.Where(grammar.IsUnitTerminal)];
        _bins = [..grammar.Productions.Where(grammar.IsBinaryNonTerminal)];
        _index = new Dictionary<string, int>(capacity: grammar.NonTerminals.Count);

        foreach (var nonTerminal in grammar.NonTerminals)
        {
            _index[nonTerminal] = _index.Count;
        }
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
        var r = _grammar.NonTerminals.Count;
        var table = new bool[n + 1, n, r];
        var back = new DefaultDict<Triple, HashSet<BackRef>>(defaultSelector: _ => []);
        
        for (var s = 0; s < n; s++)
        for (var i = 0; i <_units.Length; i++)  // Productions of the form A -> b
        {
            var nt = _units[i].NonTerminal;
            var t = _units[i].Yields[0];
            
            if (sentence[s] == t)
            {
                table[1, s, _index[nt]] = true;
            }
        }

        for (var l = 2; l <= n;     l++)  // Length of span (l)
        for (var s = 0; s <= n - l; s++)  // Start of span (s)
        for (var p = 1; p <= l - 1; p++)  // Partition of span (p)
        {
            foreach (var bin in _bins)  // Productions of the form Ra -> Rb Rc
            {
                var a = bin.NonTerminal;
                var b = bin.Yields[0];
                var c = bin.Yields[1];
                
                if (table[p, s, _index[b]] && table[l - p, s + p, _index[c]])
                {
                    table[l, s, _index[a]] = true;
                    if (buildParseTree)
                    {
                        back[(l, s, a)].Add((p, b, c));
                    }
                }
            }
        }

        var recognize = table[n, 0, _index[_grammar.Start]];
        parseTree = recognize && buildParseTree
            ? new BinaryTree<string>(root: BuildParseTree(nt: _grammar.Start, s: 0, l: n, back, sentence))
            : null;

        return recognize;
    }

    /// <summary>
    ///     Build a single valid parse tree.
    /// </summary>
    /// <param name="nt">The non-terminal to parse</param>
    /// <param name="s">The substring start index</param>
    /// <param name="l">The substring length</param>
    /// <param name="back">The populated back-reference table</param>
    /// <param name="sentence">The input sentence</param>
    /// <returns>A valid parse tree</returns>
    private static BinaryTreeNode<string> BuildParseTree(string nt, int s, int l, 
        DefaultDict<Triple, HashSet<BackRef>> back,
        IReadOnlyList<string> sentence)
    {
        var node = new BinaryTreeNode<string>(value: nt);
        if (l == 1)
        {
            node.Left = new BinaryTreeNode<string>(value: sentence[s]);
            return node;
        }

        foreach (var (p, lhs, rhs) in back[(l, s, nt)])
        {
            node.Left =  BuildParseTree(nt: lhs, s: s,     l: p,     back, sentence);
            node.Right = BuildParseTree(nt: rhs, s: s + p, l: l - p, back, sentence);

            //  Stop after the first valid parse
            //
            break;
        }

        return node;
    }
}