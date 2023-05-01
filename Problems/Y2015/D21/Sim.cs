namespace Problems.Y2015.D21;

public static class Sim
{
    public static CombatResult Run(Unit player, Unit enemy)
    {
        while (!player.Dead && !enemy.Dead)
        {
            enemy = enemy.InflictDamage(Math.Max(1, player.Damage - enemy.Armor));

            if (!enemy.Dead)
            {
                player = player.InflictDamage(Math.Max(1, enemy.Damage - player.Armor));   
            }
        }

        return new CombatResult(
            Resolution: player.Dead ? Resolution.Lose : Resolution.Win,
            Player: player);
    }
}