using System.Text.RegularExpressions;
using Utilities.Extensions;
using Utilities.Numerics;

namespace Solutions.Y2020.D16;

using Ticket = IList<int>;
using FieldMap = IDictionary<string, int>;
using FieldValidators = IDictionary<string, Predicate<int>>;

[PuzzleInfo("Ticket Translation", Topics.StringParsing, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        ParseInput(
            input:           GetInputLines(),
            yourTicket:      out var yours,
            otherTickets:    out var others,
            fieldValidators: out var validators,
            departureFields: out var fields);
        
        return part switch
        {
            1 => SumCompletelyInvalidFields(validators, others),
            2 => ComputeTicketFieldsProduct(yours, fields, BuildFieldMap(yours, others, validators)),
            _ => ProblemNotSolvedString
        };
    }

    private static int SumCompletelyInvalidFields(FieldValidators validators, IEnumerable<Ticket> tickets)
    {
        return tickets
            .SelectMany(ticket => ticket)
            .Where(field => !validators.Values.Any(validator => validator.Invoke(field)))
            .Sum();
    }

    private static FieldMap BuildFieldMap(Ticket yours, IEnumerable<Ticket> others, FieldValidators validators)
    {
        var confirmedMappings = new Dictionary<string, int>();
        var tickets = new List<Ticket>(others) { yours };
        var valid = tickets
            .Where(fields => fields.All(field => validators.Values.Any(v => v.Invoke(field))))
            .ToList();

        var candidateMappings = validators.ToDictionary(
            keySelector: kvp => kvp.Key,
            elementSelector: _ => new HashSet<int>());

        foreach (var field in candidateMappings.Keys)
        {
            for (var i = 0; i < validators.Count; i++)
            {
                if (valid.All(fields => validators[field].Invoke(fields[i])))
                {
                    candidateMappings[field].Add(i);
                }
            }
        }

        while (confirmedMappings.Count < validators.Count)
        {
            foreach (var (field, candidateIndices) in candidateMappings)
            {
                if (candidateIndices.Count != 1)
                {
                    continue;
                }

                var index = candidateIndices.Single();
                confirmedMappings.Add(field, index);

                foreach (var candidateSet in candidateMappings.Values)
                {
                    candidateSet.Remove(index);
                }
            }
        }
        
        return confirmedMappings;
    }

    private static long ComputeTicketFieldsProduct(Ticket ticket, IEnumerable<string> fields, FieldMap fieldIndices)
    {
        return fields.Aggregate(1L, (current, field) => current * ticket[fieldIndices[field]]);
    }

    private static void ParseInput(IList<string> input, out Ticket yourTicket, out IList<Ticket> otherTickets,
        out FieldValidators fieldValidators, out IList<string> departureFields)
    {
        yourTicket = ParseTicket(input[input.IndexOf("your ticket:") + 1]);
        otherTickets = input
            .Skip(input.IndexOf("nearby tickets:") + 1)
            .Select(ParseTicket)
            .ToList();
        
        fieldValidators = new Dictionary<string, Predicate<int>>();
        departureFields = new List<string>();
        
        for (var i = 0; !string.IsNullOrWhiteSpace(input[i]); i++)
        {
            var match = Regex.Match(input[i], @"([a-z ]+): (\d+)-(\d+) or (\d+)-(\d+)");
            var field = match.Groups[1].Value;
            var r1 = new Range<int>(
                Min: match.Groups[2].ParseInt(),
                Max: match.Groups[3].ParseInt());
            var r2 = new Range<int>(
                Min: match.Groups[4].ParseInt(),
                Max: match.Groups[5].ParseInt());

            fieldValidators.Add(
                key: field,
                value: value => r1.Contains(value) || r2.Contains(value));

            if (field.StartsWith("departure"))
            {
                departureFields.Add(field);
            }
        }
    }

    private static Ticket ParseTicket(string line)
    {
        return new List<int>(line.Split(separator: ',').Select(int.Parse));
    }
}