namespace Problems.Y2018.D24;

public sealed class TargetPriorityComparer : IComparer<Group>
{
    public int Compare(Group x, Group y)
    {
        return x.EffectivePower != y.EffectivePower
            ? x.EffectivePower.CompareTo(y.EffectivePower)
            : x.Units.Initiative.CompareTo(y.Units.Initiative);
    }
}

public sealed class AttackPriorityComparer : IComparer<Group>
{
    public int Compare(Group x, Group y)
    {
        return x.Units.Initiative.CompareTo(y.Units.Initiative);
    }
}