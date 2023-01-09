using Problems.Y2021.Common;
using Utilities.Cartesian;

namespace Problems.Y2021.D23;

/// <summary>
/// Amphipod: https://adventofcode.com/2021/day/23
/// </summary>
public class Solution : SolutionBase2021
{
    private const int SideRoomDepth1 = 2;
    private const int SideRoomDepth2 = 4;

    public override int Day => 23;
    
    public override object Run(int part)
    {
        return part switch
        {
            0 => FindMinCost(State.FromInitialPositions(Input.Part1), new Field(SideRoomDepth1)),
            1 => FindMinCost(State.FromInitialPositions(Input.Part2), new Field(SideRoomDepth2)),
            _ => ProblemNotSolvedString,
        };
    }

    private static int FindMinCost(State state, Field field)
    {
        if (AllActorsFinished(state, field))
        {
            return state.Cost;   
        }

        var minCost = int.MaxValue;
        foreach (var move in GetReasonableMoves(state, field))
        {
            minCost = Math.Min(minCost, FindMinCost(
                state: state.AfterMove(move),
                field: field));
        }

        return minCost;
    }
    
    private static IEnumerable<Move> GetReasonableMoves(State state, Field field)
    {
        var movePool = new List<Move>();
        foreach (var (fromPos, actor) in state.ActorsMap)
        {
            if (IsActorFinished(actor, fromPos, state, field))
            {
                continue;
            }

            var moves = FindActorMoves(actor, fromPos, state, field);
            if (moves.Any(m => m.Type == MoveType.ToSideRoom))
            {
                return Enumerable.Repeat(moves.First(m => m.Type == MoveType.ToSideRoom), 1);
            }

            movePool.AddRange(moves);
        }
        
        return movePool;
    }

    private static IList<Move> FindActorMoves(char actor, Vector2D fromPos, State state, Field field)
    {
        var atWaitingPos = field.WaitingPositions.Contains(fromPos);
        var destination = field.SideRooms[actor];
        var freeAdjacent = field.AdjacencyList[fromPos]
            .Where(p => !state.ActorsMap.ContainsKey(p))
            .ToList();

        var visited = new HashSet<Vector2D>(freeAdjacent) { fromPos };
        var queue = new Queue<Vector2D>(freeAdjacent);
        var moves = new List<Move>();

        while (queue.Count > 0)
        {
            var toPos = queue.Dequeue();
            if (toPos == destination.Top && CanActorEnterSideRoom(actor, state, field, out var enterToPos))
            {
                return new List<Move> { field.FormMove(actor, fromPos, enterToPos) };
            }

            if (!atWaitingPos && field.WaitingPositions.Contains(toPos))
            {
                moves.Add(field.FormMove(actor, fromPos, toPos));
            }
            
            foreach (var adj in field.AdjacencyList[toPos].Where(adj => !visited.Contains(adj)))
            {
                if (state.ActorsMap.ContainsKey(adj))
                {
                    continue;
                }
                    
                visited.Add(adj);
                queue.Enqueue(adj);
            }
        }

        return moves;
    }
    
    private static bool CanActorEnterSideRoom(char actor, State state, Field field, out Vector2D enterTo)
    {
        var sideRoom = field.SideRooms[actor];
        var current = sideRoom.Bottom;

        while (sideRoom.Contains(current))
        {
            if (!state.ActorsMap.TryGetValue(current, out var occupant))
            {
                enterTo = current;
                return true;
            }

            if (occupant == actor)
            {
                current += Vector2D.Up;
                continue;
            }
            
            break;
        }

        enterTo = Vector2D.Zero;
        return false;
    }
    
    private static bool IsActorFinished(char actor, Vector2D pos, State state, Field field)
    {
        var sideRoom = field.SideRooms[actor];
        if (!sideRoom.Contains(pos))
        {
            return false;
        }

        var current = sideRoom.Bottom;
        while (current != pos)
        {
            if (!state.ActorsMap.TryGetValue(current, out var occupant) || occupant != actor)
            {
                return false;
            }
            
            current += Vector2D.Up;
        }
        return true;
    }

    private static bool AllActorsFinished(State state, Field field)
    {
        foreach (var (position, actor) in state.ActorsMap)
        {
            if (!field.SideRooms[actor].Contains(position))
            {
                return false;
            }
        }
        return true;
    }
}