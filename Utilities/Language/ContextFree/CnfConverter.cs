using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Utilities.Extensions;

namespace Utilities.Language.ContextFree;

/// <summary>
///     A CNF converter exposing <see cref="Convert" />.
/// </summary>
[SuppressMessage(category: "ReSharper", checkId: "InconsistentNaming")]
public static class CnfConverter
{
    /// <summary>
    ///     An ordered set of transforms to convert any CFG to CNF
    /// </summary>
    private static readonly List<Func<Grammar, Grammar>> _orderedTransforms =
    [
        START,
        TERM,
        BIN,
        DEL,
        UNIT
    ];

    /// <summary>
    ///     Convert a Context Free <see cref="Grammar" /> to Chomsky Normal Form (CNF)
    /// </summary>
    /// <param name="original">The <see cref="Grammar" /> to convert</param>
    /// <returns>A <see cref="Grammar" /> instance, where all <see cref="Production" /> rules are in CNF</returns>
    public static Grammar Convert(Grammar original)
    {
        if (original.IsInCnf)
        {
            return original;
        }

        return _orderedTransforms.Aggregate(
            seed: original,
            func: (grammar, transform) => transform.Invoke(grammar));
    }

    /// <summary>
    ///     Eliminate the start symbol from the right-hand sides
    /// </summary>
    private static Grammar START(Grammar g)
    {
        if (!g.Productions.Any(p => p.Yields.Contains(g.Start)))
        {
            return g;
        }

        var s0 = GetNewNonTerminal(prefix: g.Start, nonTerminals: g.NonTerminals);
        var p0 = new Production(nonTerminal: s0, yields: g.Start);

        return new Grammar(
            start: s0,
            epsilon: g.Epsilon,
            productions: g.Productions.Prepend(p0));
    }

    /// <summary>
    ///     Eliminate rules with non-solitary terminals
    /// </summary>
    private static Grammar TERM(Grammar g)
    {
        var newNonTerminals = new HashSet<string>(g.NonTerminals);
        var newProductions = new HashSet<Production>(g.Productions);
        var nonSolitaries = g.Productions
            .Where(g.IsNonSolitary)
            .ToHashSet();

        foreach (var nonSolitaryProduction in nonSolitaries)
        {
            var nonTerminal = nonSolitaryProduction.NonTerminal;
            var newYieldedSymbols = new List<string>();

            foreach (var symbol in nonSolitaryProduction.Yields)
            {
                if (g.NonTerminals.Contains(symbol))
                {
                    newYieldedSymbols.Add(symbol);
                    continue;
                }

                var newNonTerminal = GetNewNonTerminal(prefix: nonTerminal, newNonTerminals);
                var newUnitProduction = new Production(
                    nonTerminal: newNonTerminal,
                    yields: symbol);

                newProductions.Add(newUnitProduction);
                newNonTerminals.Add(newNonTerminal);
                newYieldedSymbols.Add(newNonTerminal);
            }

            newProductions.Remove(nonSolitaryProduction);
            newProductions.Add(new Production(
                nonTerminal: nonTerminal,
                yields: newYieldedSymbols));
        }

        return new Grammar(
            start: g.Start,
            epsilon: g.Epsilon,
            productions: newProductions);
    }

    /// <summary>
    ///     Eliminate right-hand sides with more than 2 non-terminals
    /// </summary>
    private static Grammar BIN(Grammar g)
    {
        var newNonTerminals = new HashSet<string>(g.NonTerminals);
        var newProductions = new HashSet<Production>(g.Productions);
        var supraBinaries = g.Productions
            .Where(g.IsSupraBinaryNonTerminal)
            .ToHashSet();

        foreach (var initialProduction in supraBinaries)
        {
            var currentNonTerminal = initialProduction.NonTerminal;
            var nonTerminalSequence = initialProduction.Yields;
            var count = nonTerminalSequence.Count;

            for (var i = 0; i < count - 2; i++)
            {
                var newNonTerminal = GetNewNonTerminal(
                    prefix: currentNonTerminal,
                    nonTerminals: newNonTerminals);
                var newProduction = new Production(
                    nonTerminal: currentNonTerminal,
                    yields: [nonTerminalSequence[i], newNonTerminal]);

                newNonTerminals.Add(newNonTerminal);
                newProductions.Add(newProduction);
                currentNonTerminal = newNonTerminal;
            }

            var endProduction = new Production(
                nonTerminal: currentNonTerminal,
                yields: [nonTerminalSequence[count - 2], nonTerminalSequence[count - 1]]);

            newProductions.Add(endProduction);
            newProductions.Remove(initialProduction);
        }

        return new Grammar(
            start: g.Start,
            epsilon: g.Epsilon,
            productions: newProductions);
    }

    /// <summary>
    ///     Eliminate ε-rules
    /// </summary>
    private static Grammar DEL(Grammar g)
    {
        if (g.Epsilon == null)
        {
            return g;
        }

        var epsilons = g.Productions
            .Where(p => g.IsEpsilon(p) && p.NonTerminal != g.Start)
            .ToHashSet();
        var nullables = epsilons
            .Select(p => p.NonTerminal)
            .ToHashSet();
        var candidates = g.Productions
            .Where(p => p.Yields.All(g.NonTerminals.Contains))
            .ToArray();
        var foundNew = nullables.Count != 0;

        while (foundNew)
        {
            foundNew = candidates.Any(
                candidate => candidate.Yields.All(nullables.Contains) && nullables.Add(candidate.NonTerminal));
        }

        var newProductions = new HashSet<Production>();
        if (nullables.Contains(g.Start))
        {
            newProductions.Add(new Production(nonTerminal: g.Start, yields: g.Epsilon));
        }
        
        foreach (var oldProduction in g.Productions)
        {
            if (epsilons.Contains(oldProduction))
            {
                continue;
            }
            
            var length = oldProduction.Yields.Count;
            var seed = (0, ImmutableQueue<bool>.Empty);
            var queue = new Queue<(int, ImmutableQueue<bool>)>([seed]);

            while (queue.Count != 0)
            {
                var (pos, omitMask) = queue.Dequeue();
                if (pos >= length)
                {
                    var yields = oldProduction.Yields
                        .ZipWhere(omitMask)
                        .ToArray();
                    
                    if (yields.Length > 0)
                    {
                        newProductions.Add(new Production(oldProduction.NonTerminal, yields));
                    }
                    continue;
                }
                
                queue.Enqueue((pos + 1, omitMask.Enqueue(true)));
                if (nullables.Contains(oldProduction.Yields[pos]))
                {
                    queue.Enqueue((pos + 1, omitMask.Enqueue(false)));
                }
            }
        }
        
        return new Grammar(
            start: g.Start,
            epsilon: g.Epsilon,
            productions: newProductions);
    }
    
    /// <summary>
    ///     Eliminate unit rules
    /// </summary>
    private static Grammar UNIT(Grammar g)
    {
        var newProductions = new List<Production>(g.Productions);
        var unitProductions = newProductions
            .Where(g.IsUnitNonTerminal)
            .ToHashSet();

        while (unitProductions.Count != 0)
        {
            foreach (var unitProduction in unitProductions)
            {
                var yieldedNonTerminal = unitProduction.Yields[0];
                var associatedProductions = newProductions
                    .Where(p => p.NonTerminal == yieldedNonTerminal)
                    .ToHashSet();

                newProductions.Remove(unitProduction);
                newProductions.AddRange(associatedProductions.Select(associate =>
                    new Production(nonTerminal: unitProduction.NonTerminal, yields: associate.Yields)));
            }

            unitProductions = newProductions
                .Where(g.IsUnitNonTerminal)
                .ToHashSet();
        }

        return new Grammar(
            start: g.Start,
            epsilon: g.Epsilon,
            productions: newProductions);
    }

    private static string GetNewNonTerminal(string prefix, IReadOnlySet<string> nonTerminals)
    {
        //  Try to prevent unnecessarily appending digits to non-terminals which are already of the form ABC123
        //
        var match = Regex.Match(prefix, pattern: @"^(?<prefix>[^\d]*)(?<suffix>[\d])+$");
        var suffix = 0;
        
        if (match.Success)
        {
            prefix = match.Groups["prefix"].Value;
            suffix = match.Groups["suffix"].ParseInt();
        }
        
        var nonTerminal = $"{prefix}{suffix}";
        while (nonTerminals.Contains(nonTerminal))
        {
            nonTerminal = $"{prefix}{++suffix}";
        }

        return nonTerminal;
    }
}