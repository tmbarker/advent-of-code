using System.Text.RegularExpressions;
using Utilities.Extensions;

namespace Solutions.Y2018.D24;

public static class Input
{
    private const string ImmuneSystem = "Immune System:";
    private const string Infection = "Infection:";
    
    private static readonly Regex AttackTypeRegex = new(@"(?<Type>[a-z]+) damage");
    private static readonly Regex WeaknessesRegex = new(@"weak to(?: ?(?<Types>[a-z]+)(?:,|\)|;))+");
    private static readonly Regex ImmunitiesRegex = new(@"immune to(?: ?(?<Types>[a-z]+)(?:,|\)|;))+");

    public static State ParseState(IList<string> input)
    {
        var immuneGroups = input
            .Skip(input.IndexOf(ImmuneSystem) + 1)
            .TakeWhile(s => !string.IsNullOrWhiteSpace(s))
            .Select((line, index) => ParseGroup(line, Team.ImmuneSystem, id: index + 1));

        var infectGroups = input
            .Skip(input.IndexOf(Infection) + 1)
            .TakeWhile(s => !string.IsNullOrWhiteSpace(s))
            .Select((line, index) => ParseGroup(line, Team.Infection, id: index + 1));
        
        return new State(immuneGroups.Concat(infectGroups));
    }

    private static Group ParseGroup(string line, Team team, int id)
    {
        var numbers = line.ParseInts();
        var weaknesses = new HashSet<string>();
        var immunities = new HashSet<string>();
        
        var attack = new Attack(
            Type: AttackTypeRegex.Match(line).Groups["Type"].Value,
            Damage: numbers[2]);

        var weaknessesMatch = WeaknessesRegex.Match(line);
        if (weaknessesMatch.Success)
        {
            foreach (Capture weakness in weaknessesMatch.Groups["Types"].Captures)
            {
                weaknesses.Add(weakness.Value);
            }
        }
        
        var immunitiesMatch = ImmunitiesRegex.Match(line);
        if (immunitiesMatch.Success)
        {
            foreach (Capture immunity in immunitiesMatch.Groups["Types"].Captures)
            {
                immunities.Add(immunity.Value);
            }
        }

        var unit = new Unit(
            Hp: numbers[1],
            Weaknesses: weaknesses,
            Immunities: immunities,
            Attack: attack,
            Initiative: numbers[3]);

        return new Group(
            team: team, 
            id: id,
            units: unit, 
            size: numbers[0]);
    }
}