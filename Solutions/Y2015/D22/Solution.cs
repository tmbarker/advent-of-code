using Utilities.Extensions;

namespace Solutions.Y2015.D22;

[PuzzleInfo("Wizard Simulator 20XX", Topics.Simulation, Difficulty.Hard)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();
        var state = State.Initial(
            player: new Wizard(Hp: 50, Mana: 500, Armor: 0),
            boss: new Boss(Hp: input[0].ParseInt(), Dmg: input[1].ParseInt()));
        
        return part switch
        {
            1 => FindMinMana(state, tickDmg: 0),
            2 => FindMinMana(state, tickDmg: 1),
            _ => ProblemNotSolvedString
        };
    }

    private static int FindMinMana(State initial, int tickDmg)
    {
        var visited = new HashSet<State>(collection: [initial]);
        var queue = new PriorityQueue<State, int>(items: [(initial, 0)]);

        while (queue.Count > 0)
        {
            var state = queue.Dequeue();
            if (state.IsResolved)
            {
                if (state.Boss.Hp <= 0)
                {
                    return state.ManaUsed;
                }
                
                continue;
            }

            var adjacent = GetAdjacentStates(state, tickDmg);
            var unvisited = adjacent.Where(adj => !visited.Contains(adj));

            foreach (var adj in unvisited)
            {
                visited.Add(adj);
                queue.Enqueue(adj, adj.ManaUsed);
            }
        }

        throw new NoSolutionException();
    }
    
    private static IEnumerable<State> GetAdjacentStates(State state, int tickDmg)
    {
        state = TickPlayerDmg(state, tickDmg);
        if (state.Player.Hp <= 0)
        {
            yield break;
        }
        
        state = TickEffects(state);
        if (state.IsResolved)
        {
            if (state.Player.Hp > 0)
            {
                yield return state;   
            }
            yield break;
        }

        var cachedPlayer = state.Player;
        var canCast = GameData.Spells
            .Where(s => !state.ActiveEffects.ContainsKey(s))
            .Where(s => GameData.SpellCosts[s] <= cachedPlayer.Mana)
            .ToArray();

        if (canCast.Length == 0)
        {
            yield break;
        }
        
        foreach (var spell in canCast)
        {
            var adjState = ExecutePlayerAttack(state, spell);

            if (!adjState.IsResolved)
            {
                adjState = TickEffects(adjState);
            }

            if (!adjState.IsResolved)
            {
                adjState = ExecuteBossAttack(adjState);
            }

            yield return adjState;
        }
    }

    private static State ExecutePlayerAttack(State state, Spell spell)
    {
        var player = state.Player;
        var boss = state.Boss;
        
        if (GameData.SpellDurations[spell] > 0)
        {
            var updatedEffects = new Dictionary<Spell, int>(state.ActiveEffects)
                { { spell, GameData.SpellDurations[spell] } };
            return new State(
                player: player with { Mana = player.Mana - GameData.SpellCosts[spell] },
                boss: boss,
                manaUsed: state.ManaUsed + GameData.SpellCosts[spell],
                activeEffects: updatedEffects);
        }

        var agents = CastSpell(spell, player, boss);
        return new State(
            player: agents.Player,
            boss: agents.Boss,
            manaUsed: state.ManaUsed + GameData.SpellCosts[spell],
            activeEffects: state.ActiveEffects);
    }
    
    private static State ExecuteBossAttack(State state)
    {
        var player = state.Player;
        var boss = state.Boss;
        player = state.Player with { Hp = player.Hp - Math.Max(1, boss.Dmg - player.Armor) };
        return new State(player, boss, state.ManaUsed, state.ActiveEffects);
    }

    private static (Wizard Player, Boss Boss) CastSpell(Spell spell, Wizard player, Boss boss)
    {
        switch (spell)
        {
            case Spell.MagicMissile:
                boss = boss with { Hp = boss.Hp - 4 };
                break;
            case Spell.Drain:
                player = player with { Hp = player.Hp + 2 };
                boss = boss with { Hp = boss.Hp - 2 };
                break;
            case Spell.Shield:
            case Spell.Poison:
            case Spell.Recharge:
                break;
            default:
                throw new NoSolutionException();
        }

        player = player with { Mana = player.Mana - GameData.SpellCosts[spell] };
        return (player, boss);
    }

    private static State TickPlayerDmg(State state, int amount)
    {
        var player = state.Player with { Hp = state.Player.Hp - amount };
        return new State(
            player,
            state.Boss,
            state.ManaUsed,
            state.ActiveEffects);
    }
    
    private static State TickEffects(State state)
    {
        var agents = (state.Player, state.Boss);
        var nextActive = new Dictionary<Spell, int>();
        
        foreach (var (effect, timer) in state.ActiveEffects)
        {
            if (timer >= 1)
            {
                agents = TickEffect(effect, agents.Player, agents.Boss);
                nextActive[effect] = timer - 1;
            }
            
            if (nextActive[effect] == 0)
            {
                agents = RemoveEffect(effect, agents.Player, agents.Boss);
                nextActive.Remove(effect);
            }
        }

        return new State(agents.Player, agents.Boss, state.ManaUsed, activeEffects: nextActive);
    }
    
    private static (Wizard Player, Boss Boss) TickEffect(Spell spell, Wizard player, Boss boss)
    {
        switch (spell)
        {
            case Spell.Poison:
                boss = boss with { Hp = boss.Hp - 3 };
                break;
            case Spell.Recharge:
                player = player with { Mana = player.Mana + 101 };
                break;
            case Spell.Shield:
                player = player with { Armor = 7 };
                break;
            case Spell.MagicMissile:
            case Spell.Drain:
                break;
            default:
                throw new NoSolutionException();
        }

        return (player, boss);
    }
    
    private static (Wizard Player, Boss Boss) RemoveEffect(Spell spell, Wizard player, Boss boss)
    {
        switch (spell)
        {
            case Spell.Shield:
                player = player with { Armor = 0 };
                break;
            case Spell.MagicMissile:
            case Spell.Drain:
            case Spell.Poison:
            case Spell.Recharge:
                break;
            default:
                throw new NoSolutionException();
        }

        return (player, boss);
    }
}