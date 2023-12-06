using System.Text.RegularExpressions;
using Problems.Common;

namespace Problems.Y2019.D14;

/// <summary>
/// Space Stoichiometry: https://adventofcode.com/2019/day/14
/// </summary>
public sealed class Solution : SolutionBase
{
    private const string Fuel = "FUEL";
    private const string Ore = "ORE";
    private const long OreFunds = 1000000000000L;

    public override object Run(int part)
    {
        var reactions = ParseReactions(GetInputLines());
        return part switch
        {
            1 => ComputeFuelCost(1, reactions),
            2 => ComputeMaxFuelOutput(OreFunds, reactions),
            _ => ProblemNotSolvedString
        };
    }

    private static readonly Func<KeyValuePair<string, long>, bool> ShouldProcess = pair => pair.Key != Ore && pair.Value > 0L;

    private static long ComputeFuelCost(long amount, IReadOnlyDictionary<string, Reaction> reactions)
    {
        var need = reactions.Keys.ToDictionary(
            keySelector:     substance => substance,
            elementSelector: substance => substance == Fuel ? amount : 0L);

        need.Add(Ore, 0);
        
        while (need.Any(ShouldProcess))
        {
            foreach (var (needSubstance, needAmount) in need.Where(ShouldProcess))
            {
                var reaction = reactions[needSubstance];
                var produces = reaction.Product.Amount;
                var numReactions = (long)Math.Ceiling((double)needAmount / produces);
                need[needSubstance] -= numReactions * produces;
                
                foreach (var (precursorAmount, precursorSubstance) in reaction.Reactants)
                {
                    need[precursorSubstance] += numReactions * precursorAmount ;
                }
            }
        }

        return need[Ore];
    }

    private static long ComputeMaxFuelOutput(long oreFunds, IReadOnlyDictionary<string, Reaction> reactions)
    {
        var singleCost = ComputeFuelCost(1, reactions);
        var fuelGuess = oreFunds / singleCost;
        var guessCost = ComputeFuelCost(fuelGuess, reactions);

        while (true)
        {
            fuelGuess += (oreFunds - guessCost) / singleCost + 1;
            guessCost = ComputeFuelCost(fuelGuess, reactions);

            if (guessCost > oreFunds)
            {
                break;
            }
        }
        
        return fuelGuess - 1;
    }

    private static Dictionary<string, Reaction> ParseReactions(IEnumerable<string> input)
    {
        var reactions = new Dictionary<string, Reaction>();
        var reactantRegex = new Regex(@"(\d+) ([A-Z]+)(?:,| )");
        var productRegex = new Regex(@"=> (\d+) ([A-Z]+)");

        foreach (var line in input)
        {
            var reactants = new List<Term>();
            foreach (Match match in reactantRegex.Matches(line))
            {
                reactants.Add(new Term(
                    Amount: long.Parse(match.Groups[1].Value),
                    Substance: match.Groups[2].Value));
            }

            var productMatch = productRegex.Match(line);
            var productSubstance = productMatch.Groups[2].Value;
            var products = new Term(
                Amount: long.Parse(productMatch.Groups[1].Value),
                Substance: productSubstance);
            
            reactions.Add(productSubstance, new Reaction(reactants, products));
        }

        return reactions;
    }
}