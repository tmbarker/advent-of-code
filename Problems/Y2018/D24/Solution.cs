using Problems.Common;

namespace Problems.Y2018.D24;

/// <summary>
/// Immune System Simulator 20XX: https://adventofcode.com/2018/day/24
/// </summary>
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var lines = GetInputLines();
        var initialState = Input.ParseState(lines);

        return part switch
        {
            1 => GetDefaultScore(initialState),
            2 => GetBoostedScore(initialState),
            _ => ProblemNotSolvedString
        };
    }

    private static int GetDefaultScore(State initialState)
    {
        return Sim.Run(initialState).Score;
    }

    private static int GetBoostedScore(State initialState)
    {
        var boost = 0;
        var state = initialState;
        var result = Sim.Run(state);

        while (result.Resolution != Resolution.Winner || result.Winner != Team.ImmuneSystem)
        {
            state = Boost(initialState, ++boost);
            result = Sim.Run(state);
        }

        return result.Score;
    }

    private static State Boost(State state, int boost)
    {
        var boosted = state.GroupsByCk.Values.Select(group =>
        {
            if (group.Team == Team.Infection)
            {
                return group;
            }

            var defaultAttack = group.Units.Attack;
            var boostedAttack = defaultAttack with { Damage = defaultAttack.Damage + boost };
            var boostedUnit = group.Units with { Attack = boostedAttack };

            return new Group(
                team: group.Team,
                id: group.Id,
                units: boostedUnit,
                size: group.Size);
        });

        return new State(boosted);
    }
}