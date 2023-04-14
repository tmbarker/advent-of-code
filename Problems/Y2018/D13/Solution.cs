using Problems.Common;
using Utilities.Cartesian;

namespace Problems.Y2018.D13;

/// <summary>
/// Mine Cart Madness: https://adventofcode.com/2018/day/13
/// </summary>
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var track = GetInputLines();
        var state = State.Parse(track);

        return part switch
        {
            1 => TickUntilCrash(track, state),
            2 => TickUntilLastRemaining(track, state),
            _ => ProblemNotSolvedString
        };
    }

    private static string TickUntilCrash(IList<string> track, State state)
    {
        var crashed = false;
        var crashedAt = Vector2D.Zero;
        
        while (!crashed)
        {
            crashed = !TickCarts(
                track: track, 
                state: state, 
                crashResponse: CrashResponse.Halt, 
                crashAt: out crashedAt);
        }

        return $"{crashedAt.X},{crashedAt.Y}";
    }
    
    private static string TickUntilLastRemaining(IList<string> track, State state)
    {
        while (state.Positions.Count > 1)
        {
            TickCarts(
                track: track,
                state: state,
                crashResponse: CrashResponse.Remove,
                crashAt: out _);
        }

        var final = state.Positions.Values.Single();
        return $"{final.X},{final.Y}";
    }

    private static bool TickCarts(IList<string> track, State state, CrashResponse crashResponse, out Vector2D crashAt)
    {
        foreach (var cartId in state.GetCurrentOrder())
        {
            if (!state.Positions.ContainsKey(cartId))
            {
                continue;
            }
            
            var facing = state.Facings[cartId];
            var stepTo = state.Positions[cartId] + facing;
            var element = track[stepTo.Y][stepTo.X];
                
            if (state.Positions.ContainsValue(stepTo))
            {
                if (crashResponse == CrashResponse.Halt)
                {
                    crashAt = stepTo;
                    return false;
                }

                var otherId = state.Positions.Keys.Single(id => state.Positions[id] == stepTo);
                state.RemoveCart(cartId);
                state.RemoveCart(otherId);
                continue;
            }
                
            switch (element)
            {
                case Track.Junction:
                    facing = Track.TurnChoices[state.NextTurns[cartId]] * facing;
                    state.NextTurns[cartId] = (state.NextTurns[cartId] + 1) % Track.TurnChoices.Count;
                    break;
                case Track.Left or Track.Right:
                    facing = Track.TurnForCorner(element, facing);
                    break;
            }

            state.Positions[cartId] = stepTo;
            state.Facings[cartId] = facing;
        }
        
        crashAt = Vector2D.Zero;
        return true;
    }
}