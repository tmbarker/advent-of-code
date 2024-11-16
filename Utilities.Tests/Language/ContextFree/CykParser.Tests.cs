using System.Diagnostics.CodeAnalysis;
using Utilities.Language.ContextFree;

namespace Utilities.Tests.Language.ContextFree;

/// <summary>
///     Tests associated with <see cref="CnfConverter"/> and <see cref="CykParser"/>.
/// </summary>
[SuppressMessage("Performance", "CA1861:Avoid constant arrays as arguments")]
public sealed class CykParserTests
{
    public readonly record struct GrammarTheoryData(
        Grammar Grammar,
        List<string[]> ValidSentences,
        List<string[]> InvalidSentences);
    
    [Theory]
    [MemberData(nameof(GrammarTestData))]
    public void Grammar_CykParser_ShouldOnlyRecognizeValidSentences(GrammarTheoryData theoryData)
    {
        // Arrange
        var cnf = CnfConverter.Convert(theoryData.Grammar);
        var cyk = new CykParser(cnf);
        
        // Act & Assert
        Assert.True(cnf.IsInCnf);
        Assert.All(theoryData.ValidSentences, s =>
        {
            Assert.True(cyk.Recognize(s, out var parseTree));
            Assert.NotNull(parseTree);
        });
        Assert.All(theoryData.InvalidSentences, s =>
        {
            Assert.False(cyk.Recognize(s, out var parseTree));
            Assert.Null(parseTree);
        });
    }

    public static TheoryData<GrammarTheoryData> GrammarTestData => [
        GetPalindromicGrammarTheory(),
        GetBalancedParenthesisGrammarTheory(),
        GetArithmeticGrammarTheory(),
        GetSimpleSentenceGrammarTheory(),
    ];
    
    private static GrammarTheoryData GetPalindromicGrammarTheory()
    {
        var grammar = new Grammar(start: "S", epsilon: "ε", productions:
        [
            new Production(nonTerminal: "S", yields: ["a"]),
            new Production(nonTerminal: "S", yields: ["b"]),
            new Production(nonTerminal: "S", yields: ["a", "S", "a"]),
            new Production(nonTerminal: "S", yields: ["b", "S", "b"]),
            new Production(nonTerminal: "S", yields: ["ε"])
        ]);

        return new GrammarTheoryData(
            Grammar: grammar,
            ValidSentences:
            [
                ["a"],
                ["b"],
                ["a", "a"],
                ["b", "b"],
                ["a", "b", "a"],
                ["b", "a", "b"],
                ["a", "b", "b", "a"],
                ["b", "a", "a", "b"],
                ["ε"]
            ],
            InvalidSentences:
            [
                ["a", "b"],
                ["a", "a", "b"],
                ["a", "b", "c"]
            ]);
    }

    private static GrammarTheoryData GetBalancedParenthesisGrammarTheory()
    {
        var grammar = new Grammar(start: "S", epsilon: "ε", productions:
        [
            new Production(nonTerminal: "S", yields: ["(", "S", ")"]),
            new Production(nonTerminal: "S", yields: ["ε"])
        ]);

        return new GrammarTheoryData(
            Grammar: grammar,
            ValidSentences:
            [
                ["(", ")"],
                ["(", "(", ")", ")"],
                ["ε"]
            ],
            InvalidSentences:
            [
                ["(", "(", ")"],
                ["(", "(", ")", ")", "("],
                ["(", "(", ")", "(", ")"]
            ]);
    }
    
    private static GrammarTheoryData GetArithmeticGrammarTheory()
    {
        var grammar = new Grammar(start: "E", productions:
        [
            new Production(nonTerminal: "E", yields: ["E", "+", "T"]),
            new Production(nonTerminal: "E", yields: ["T"]),
            new Production(nonTerminal: "T", yields: ["T", "*", "F"]),
            new Production(nonTerminal: "T", yields: ["F"]),
            new Production(nonTerminal: "F", yields: ["(", "E", ")"]),
            new Production(nonTerminal: "F", yields: ["n"])
        ]);

        return new GrammarTheoryData(
            Grammar: grammar,
            ValidSentences:
            [
                ["n"],
                ["n", "+", "n"],
                ["n", "*", "n"],
                ["n", "+", "n", "*", "n"],
                ["n", "*", "(", "n", "+", "n", ")"]
            ],
            InvalidSentences:
            [
                ["n", "+", "n", "*"],
                ["n", "*", "+", "n"]
            ]);
    }
    
    private static GrammarTheoryData GetSimpleSentenceGrammarTheory()
    {
        var grammar = new Grammar(start: "S", productions:
        [
            new Production(nonTerminal: "S",  yields: ["NP", "VP"]),
            new Production(nonTerminal: "NP", yields: ["the", "N"]),
            new Production(nonTerminal: "VP", yields: ["V", "NP"]),
            new Production(nonTerminal: "N",  yields: ["cat"]),
            new Production(nonTerminal: "N",  yields: ["dog"]),
            new Production(nonTerminal: "V",  yields: ["chases"]),
            new Production(nonTerminal: "V",  yields: ["sees"])
        ]);

        return new GrammarTheoryData(
            Grammar: grammar,
            ValidSentences:
            [
                ["the", "cat", "chases", "the", "dog"],
                ["the", "dog", "sees", "the", "cat"]
            ],
            InvalidSentences:
            [
                ["the", "cat", "chases"],
                ["the", "dog", "the", "cat", "chases"]
            ]);
    }
}