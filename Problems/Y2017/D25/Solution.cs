using Problems.Common;
using Utilities.Extensions;

namespace Problems.Y2017.D25;

/// <summary>
/// The Halting Problem: https://adventofcode.com/2017/day/25
/// </summary>
public class Solution : SolutionBase
{
    public override int Parts => 1;

    public override object Run(int part)
    {
        return part switch
        {
            1 => Simulate(),
            _ => ProblemNotSolvedString
        };
    }

    private int Simulate()
    {
        ParseInput(input: GetInputLines(), out var state, out var rules, out var steps);

        var machine = new TuringMachine(rules);
        var count = machine.Run(state, steps);

        return count;
    }
    
    private static void ParseInput(IReadOnlyList<string> input, 
        out char state, out Dictionary<char, TuringMachine.State> rules, out int steps)

    {
        state = input[0][^2];
        rules = new Dictionary<char, TuringMachine.State>();
        steps = input[1].Split(' ')[^2].ParseInt();

        foreach (var chunk in input.Skip(3).Chunk(10))
        {
            var id = chunk[0][^2];
            var falseTransition = new TuringMachine.Transition(
                Write: chunk[2][^2] == '1',
                Move: chunk[3].Contains("right") ? 1 : -1,
                Next: chunk[4][^2]);
            var trueTransition = new TuringMachine.Transition(
                Write: chunk[6][^2] == '1',
                Move: chunk[7].Contains("right") ? 1 : -1,
                Next: chunk[8][^2]);

            rules[id] = new TuringMachine.State(falseTransition, trueTransition);
        }
    }
}