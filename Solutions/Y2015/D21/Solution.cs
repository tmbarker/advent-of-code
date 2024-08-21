using Utilities.Extensions;

namespace Solutions.Y2015.D21;

[PuzzleInfo("RPG Simulator 20XX", Topics.Simulation, Difficulty.Hard)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();
        var enemy = ParseEnemy(input);
        
        return part switch
        {
            1 => GetMinWinningCost(enemy, playerHp: 100),
            2 => GetMaxLosingCost(enemy, playerHp: 100),
            _ => PuzzleNotSolvedString
        };
    }

    private static int GetMinWinningCost(Unit enemy, int playerHp)
    {
        var min = int.MaxValue;
        var combinations = GetGearCombinations();

        foreach (var set in combinations)
        {
            var player = Unit.Spawn(gear: set, hp: playerHp);
            var result = Sim.Run(player, enemy);

            if (result.Resolution == Resolution.Win)
            {
                min = Math.Min(min, result.Player.GearCost);
            }
        }

        return min;
    }
    
    private static int GetMaxLosingCost(Unit enemy, int playerHp)
    {
        var max = int.MinValue;
        var combinations = GetGearCombinations();

        foreach (var set in combinations)
        {
            var player = Unit.Spawn(gear: set, hp: playerHp);
            var result = Sim.Run(player, enemy);

            if (result.Resolution == Resolution.Lose)
            {
                max = Math.Max(max, result.Player.GearCost);
            }
        }

        return max;
    }

    private static IEnumerable<IList<Gear>> GetGearCombinations()
    {
        var wcs = new List<List<Gear>>();
        var acs = new List<List<Gear>>();
        var rcs = new List<List<Gear>>();

        wcs.AddRange(Shop.Weapons.Combinations(k: 1).Select(c => c.ToList()));
        
        acs.AddRange(Shop.Armor.Combinations(k: 0).Select(c => c.ToList()));
        acs.AddRange(Shop.Armor.Combinations(k: 1).Select(c => c.ToList()));
        
        rcs.AddRange(Shop.Rings.Combinations(k: 0).Select(c => c.ToList()));
        rcs.AddRange(Shop.Rings.Combinations(k: 1).Select(c => c.ToList()));
        rcs.AddRange(Shop.Rings.Combinations(k: 2).Select(c => c.ToList()));
        
        foreach (var wc in wcs)
        foreach (var ac in acs)
        foreach (var rc in rcs)
        {
            yield return
                wc.Concat(ac).Concat(rc).ToList();
        }
    }

    private static Unit ParseEnemy(IList<string> input)
    {
        return Unit.Spawn(
            hp: input[0].ParseInt(),
            damage: input[1].ParseInt(),
            armor: input[2].ParseInt());
    }
}