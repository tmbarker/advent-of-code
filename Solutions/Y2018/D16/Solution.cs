using Utilities.Collections;
using Utilities.Extensions;

namespace Solutions.Y2018.D16;

[PuzzleInfo("Chronal Classification", Topics.Assembly|Topics.Simulation, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();
        var observations = ParseObservations(input);
        var program = ParseProgram(input);
        
        return part switch
        {
            1 => CountCongruentOpcodes(observations, threshold: 3),
            2 => ExecuteProgram(observations, program),
            _ => PuzzleNotSolvedString
        };
    }

    private static int CountCongruentOpcodes(IEnumerable<Observation> observations, int threshold)
    {
        var cpu = new Cpu();
        return observations.Count(obs => GetCongruentOpcodes(cpu, obs).Count >= threshold);
    }

    private static int ExecuteProgram(IEnumerable<Observation> observations, IEnumerable<IList<int>> program)
    {
        var cpu = new Cpu();
        var mappings = BuildOpcodeMap(cpu, observations);

        cpu.ResetRegisters();
        foreach (var instruction in program)
        {
            cpu.Execute(
                opcode: mappings[instruction[0]],
                a: instruction[1],
                b: instruction[2],
                c: instruction[3]);
        }

        return cpu[0];
    }

    private static Dictionary<int, Cpu.Opcode> BuildOpcodeMap(Cpu cpu, IEnumerable<Observation> observations)
    {
        var valueMappings = new Dictionary<int, Cpu.Opcode>();
        var congruences = new DefaultDict<int, HashSet<Cpu.Opcode>>(defaultSelector: _ => []);
        
        foreach (var observation in observations)
        foreach (var congruent in GetCongruentOpcodes(cpu, observation))
        {
            congruences[observation.Instr[0]].Add(congruent);
        }

        while (congruences.Count != 0)
        {
            foreach (var resolvedMapping in congruences.WhereValues(c => c.Count == 1))
            {
                var value = resolvedMapping.Key;
                var opcode = resolvedMapping.Value.Single();

                valueMappings.Add(value, opcode);
                congruences.Remove(value);
                
                foreach (var remainingCandidates in congruences.Values)
                {
                    remainingCandidates.Remove(opcode);
                }
            }
        }
        
        return valueMappings;
    }

    private static HashSet<Cpu.Opcode> GetCongruentOpcodes(Cpu cpu, Observation observation)
    {
        var congruences = new HashSet<Cpu.Opcode>();
        var opcodes = Enum.GetValues<Cpu.Opcode>();

        foreach (var opcode in opcodes)
        {
            cpu.SetRegisters(observation.Before);
            cpu.Execute(
                opcode: opcode,
                a: observation.Instr[1],
                b: observation.Instr[2],
                c: observation.Instr[3]);

            if (cpu.Registers.SequenceEqual(observation.After))
            {
                congruences.Add(opcode);
            }
        }

        return congruences;
    }

    private static IEnumerable<IList<int>> ParseProgram(string[] input)
    {
        var reversedInstr = new List<IList<int>>();
        for (var i = input.Length - 1; i >= 0 && !string.IsNullOrWhiteSpace(input[i]); i--)
        {
            reversedInstr.Add(input[i].ParseInts());
        }

        return Enumerable.Reverse(reversedInstr);
    }

    private static IEnumerable<Observation> ParseObservations(string[] input)
    {
        for (var i = 0; input[i].StartsWith("Before:"); i += 4)
        {
            yield return ParseObservation(input[i..(i + 3)]);
        }
    }

    private static Observation ParseObservation(string[] chunk)
    {
        return new Observation(
            Before: chunk[0].ParseInts(),
            Instr:  chunk[1].ParseInts(),
            After:  chunk[2].ParseInts());
    }
}