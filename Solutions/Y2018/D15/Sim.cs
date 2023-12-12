using Utilities.Geometry.Euclidean;

namespace Solutions.Y2018.D15;

public static class Sim
{
    public static CombatResult Run(GameState state, bool print)
    {
        var success = true;
        while (success)
        {
            if (print)
            {
                state.Print();
            }

            var turnOrder = GetTurnOrder(state);
            for (var i = 0; i < turnOrder.Count; i++)
            {
                if (!TickUnit(state, turnOrder[i]))
                {
                    success = false;
                    break;
                }
                
                if (i == turnOrder.Count - 1)
                {
                    state.Tick++;
                }
            }
        }

        var result = CombatResult.FromState(state);
        if (print)
        {
            result.Print();   
        }

        return result;
    }

    private static IList<int> GetTurnOrder(GameState state)
    {
        return state.Units.Values
            .Where(unit => !unit.Dead)
            .OrderBy(unit => unit.Pos, GameData.SquareComparer)
            .Select(unit => unit.Id)
            .ToList();
    }
    
    private static bool TickUnit(GameState state, int unitId)
    {
        var unit = state.Units[unitId];
        if (unit.Dead)
        {
            return true;
        }
        
        if (!FindTargets(state, unitId, out var targetPositions))
        {
            return false;
        }
        
        var adjacentToTarget = IsAdjacentToAny(state, unitId, targetPositions);
        if (!adjacentToTarget)
        {
            if (!FindOpenInRange(state, targetPositions, out var inRange))
            {
                return true;
            }
            
            if (!FindNearestReachable(state, unitId, inRange, out var nearest))
            {
                return true;
            }
            
            StepTowardsPos(state, unitId, nearest);
        }
        
        if (GetAdjacentTarget(state, unitId, out var targetId))
        {
            Attack(state, unitId, targetId);   
        }

        return true;
    }

    private static bool FindTargets(GameState state, int unitId, out IList<Vector2D> targetPositions)
    {
        var unit = state.Units[unitId];
        targetPositions = state.Units.Values
            .Where(candidate => GameData.EnemyMap[unit.Team] == candidate.Team && !candidate.Dead)
            .Select(candidate => candidate.Pos)
            .ToList();

        return targetPositions.Any();
    }
    
    private static bool IsAdjacentToAny(GameState state, int unitId, IEnumerable<Vector2D> targets)
    {
        return targets.Any(pos => Vector2D.IsAdjacent(a: pos, b: state.Units[unitId].Pos, Metric.Taxicab));
    }
    
    private static bool FindOpenInRange(GameState state, IEnumerable<Vector2D> targetPositions, out HashSet<Vector2D> inRangePositions)
    {
        var field = state.Field;
        inRangePositions = targetPositions
            .SelectMany(p => p.GetAdjacentSet(Metric.Taxicab))
            .Where(p => field.IsInDomain(p) && field[p] == GameData.Empty)
            .ToHashSet();

        return inRangePositions.Any();
    }
    
    private static bool FindNearestReachable(GameState state, int unitId, HashSet<Vector2D> targetPositions, out Vector2D nearest)
    {
        var unit = state.Units[unitId];
        var start = unit.Pos;

        return Pathfinding.FindNearestReachable(
            field: state.Field,
            start: start,
            targetPositions: targetPositions,
            nearest: out nearest);
    }
    
    private static void StepTowardsPos(GameState state, int unitId, Vector2D targetPos)
    {
        var unit = state.Units[unitId];
        var pos = unit.Pos;
        
        state.Field[unit.Pos] = GameData.Empty;
        unit.Pos = Pathfinding.GetStepPos(
            field: state.Field,
            start: pos,
            goal: targetPos);
        
        state.Field[unit.Pos] = unit.Team;
    }
    
    private static bool GetAdjacentTarget(GameState state, int attackerId, out int targetId)
    {
        var attacker = state.Units[attackerId];
        var adjacentPositions = attacker.Pos.GetAdjacentSet(Metric.Taxicab);
        var candidates = state.Units.Values
            .Where(unit => adjacentPositions.Contains(unit.Pos))
            .Where(unit => !unit.Dead && unit.Team == GameData.EnemyMap[attacker.Team])
            .ToList();

        if (!candidates.Any())
        {
            targetId = -1;
            return false;
        }
        
        var weakest = candidates.Min(unit => unit.Hp);
        var targets = candidates
            .Where(unit => unit.Hp == weakest)
            .ToList();

        targetId = targets.Count == 1
            ? targets.Single().Id
            : targets.MinBy(unit => unit.Pos, GameData.SquareComparer)!.Id;
        return true;
    }
    
    private static void Attack(GameState state, int attackerId, int targetId)
    {
        var attacker = state.Units[attackerId];
        var target = state.Units[targetId];
        
        target.InflictDamage(attacker.Dmg);
        if (!target.Dead)
        {
            return;
        }
        
        state.Field[target.Pos] = GameData.Empty;
        state.Casualties[target.Team]++;
    }
}