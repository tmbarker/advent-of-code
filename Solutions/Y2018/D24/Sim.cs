namespace Solutions.Y2018.D24;

using GroupKey = ValueTuple<Team, int>;

public static class Sim
{
    private static readonly IComparer<Group> TargetPriorityComparer = new TargetPriorityComparer();
    private static readonly IComparer<Group> AttackPriorityComparer = new AttackPriorityComparer();
    
    private static readonly Dictionary<Team, Team> EnemiesMap = new()
    {
        { Team.Infection, Team.ImmuneSystem },
        { Team.ImmuneSystem, Team.Infection }
    };

    public static Result Run(State state)
    {
        while (!state.Resolved)
        {
            state = Tick(state);
        }

        return state.GetResult();
    }
    
    private static State Tick(State state)
    {
        var establishedTargets = new Dictionary<GroupKey, GroupKey>();
        var availableTargets = new HashSet<GroupKey>(state.GroupKeys); 
        var targetSelectionQueue = BuildTargetSelectionQueue(state);

        while (targetSelectionQueue.Any())
        {
            var attackerCk = targetSelectionQueue.Dequeue();
            if (!TryFindTarget(state, attackerCk, availableTargets, out var targetCk))
            {
                continue;
            }

            availableTargets.Remove(targetCk);
            establishedTargets.Add(attackerCk, targetCk);
        }

        var nextGroups = new Dictionary<GroupKey, Group>(state.GroupsByCk);
        var unitsKilled = false;
        var attackQueue = BuildAttackQueue(state, establishedTargets);

        while (attackQueue.Any())
        {
            var attackerCk = attackQueue.Dequeue();
            if (nextGroups[attackerCk].Dead)
            {
                continue;
            }
            
            var targetCk = establishedTargets[attackerCk];
            var damage = ComputeDamage(
                attacker: nextGroups[attackerCk],
                target: nextGroups[targetCk]);

            var injured = InflictDamage(damage, nextGroups[targetCk]);
            if (injured.Size < nextGroups[targetCk].Size)
            {
                unitsKilled = true;
            }
            
            nextGroups[targetCk] = injured;
        }

        return new State(
            groups: nextGroups.Values.Where(group => !group.Dead), 
            drawn: !unitsKilled);
    }

    private static bool TryFindTarget(State state, GroupKey attackerCk, IEnumerable<GroupKey> availableTargetCks, out GroupKey targetCk)
    {
        targetCk = default;
        
        var attacker = state.GroupsByCk[attackerCk];
        var enemyGroups = availableTargetCks
            .Select(ck => state.GroupsByCk[ck])
            .Where(group => EnemiesMap[attacker.Team] == group.Team)
            .ToList();
        
        if (!enemyGroups.Any())
        {
            return false;
        }

        var groups = enemyGroups.GroupBy(target => ComputeDamage(attacker, target));
        var candidates = groups.MaxBy(grouping => grouping.Key);
        var damage = candidates!.Key;

        if (damage == 0)
        {
            return false;
        }

        targetCk = candidates.MaxBy(target => target, TargetPriorityComparer).Ck;
        return true;
    }

    private static int ComputeDamage(Group attacker, Group target)
    {
        if (target.Units.Immunities.Contains(attacker.Units.Attack.Type))
        {
            return 0;
        }
        
        if (target.Units.Weaknesses.Contains(attacker.Units.Attack.Type))
        {
            return 2 * attacker.EffectivePower;
        }

        return attacker.EffectivePower;
    }
    
    private static Group InflictDamage(int damage, Group target)
    {
        var wouldKill = damage / target.Units.Hp;
        var remaining = Math.Max(0, target.Size - wouldKill);
        
        return new Group(
            team: target.Team,
            id: target.Id,
            units: target.Units,
            size: remaining);
    }

    private static Queue<GroupKey> BuildTargetSelectionQueue(State state)
    {
        var order = state.GroupKeys
            .OrderByDescending(ck => state.GroupsByCk[ck], TargetPriorityComparer);
        return new Queue<GroupKey>(order);
    }

    private static Queue<GroupKey> BuildAttackQueue(State state, Dictionary<GroupKey, GroupKey> targets)
    {
        var order = targets.Keys
            .OrderByDescending(ck => state.GroupsByCk[ck], AttackPriorityComparer);
        return new Queue<GroupKey>(order);
    }
}