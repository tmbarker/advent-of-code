using System.Text.RegularExpressions;
using Utilities.Extensions;
using Utilities.Numerics;

namespace Solutions.Y2020.D16;

using Ticket = int[];
using FieldMap = Dictionary<string, int>;
using FieldValidators = Dictionary<string, Predicate<int>>;

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
            _ => PuzzleNotSolvedString
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

    private static void ParseInput(string[] input, out Ticket yourTicket, out IList<Ticket> otherTickets,
        out FieldValidators fieldValidators, out IList<string> departureFields)
    {
        var chunks = input.ChunkByNonEmpty();
        
        yourTicket = chunks[1][^1].ParseInts();
        otherTickets = chunks[^1][1..]
            .Select(line => line.ParseInts())
            .ToList();
        
        fieldValidators = new Dictionary<string, Predicate<int>>();
        departureFields = new List<string>();
        
        for (var i = 0; i < chunks[0].Length; i++)
        {
            var match = Regex.Match(input[i], @"([a-z ]+): (\d+)-(\d+) or (\d+)-(\d+)");
            var field = match.Groups[1].Value;
            var r1 = new Range<int>(
                min: match.Groups[2].ParseInt(),
                max: match.Groups[3].ParseInt());
            var r2 = new Range<int>(
                min: match.Groups[4].ParseInt(),
                max: match.Groups[5].ParseInt());

            fieldValidators.Add(
                key: field,
                value: value => r1.Contains(value) || r2.Contains(value));

            if (field.StartsWith("departure"))
            {
                departureFields.Add(field);
            }
        }
    }
}