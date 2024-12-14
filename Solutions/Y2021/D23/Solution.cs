using Utilities.Geometry.Euclidean;

namespace Solutions.Y2021.D23;

[InputSpecificSolution("Inputs for this puzzle are difficult to parse arbitrarily, as such mine are hardcoded")]
[PuzzleInfo("Amphipod", Topics.Graphs, Difficulty.Hard)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => FindMinCost(State.FromInitialPositions(Input.Part1), new Field(sideRoomDepth: 2)),
            2 => FindMinCost(State.FromInitialPositions(Input.Part2), new Field(sideRoomDepth: 4)),
            _ => PuzzleNotSolvedString
        };
    }

    private static int FindMinCost(State initial, Field field)
    {
        var costs = new Dictionary<State, int> { { initial, 0 } };
        var queue = new PriorityQueue<State, int>(items: [(initial, 0)]);

        while (queue.Count > 0)
        {
            var state = queue.Dequeue();
            if (AllActorsFinished(state, field))
            {
                return state.Cost;   
            }

            var adjacent = GetReasonableMoves(state, field)
                .Select(adj => state.AfterMove(adj))
                .Where(adj => !costs.TryGetValue(adj, out var cost) || adj.Cost < cost);
            
            foreach (var adj in adjacent)
            {
                costs[adj] = adj.Cost;
                queue.Enqueue(adj, adj.Cost); 
            }
        }

        throw new NoSolutionException();
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

    private static List<Move> FindActorMoves(char actor, Vec2D fromPos, State state, Field field)
    {
        var atWaitingPos = field.WaitingPositions.Contains(fromPos);
        var destination = field.SideRooms[actor];
        var freeAdjacent = field.AdjacencyList[fromPos]
            .Where(p => !state.ActorsMap.ContainsKey(p))
            .ToList();

        var visited = new HashSet<Vec2D>(freeAdjacent) { fromPos };
        var queue = new Queue<Vec2D>(freeAdjacent);
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
    
    private static bool CanActorEnterSideRoom(char actor, State state, Field field, out Vec2D enterTo)
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
                current += Vec2D.Up;
                continue;
            }
            
            break;
        }

        enterTo = Vec2D.Zero;
        return false;
    }
    
    private static bool IsActorFinished(char actor, Vec2D pos, State state, Field field)
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
            
            current += Vec2D.Up;
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