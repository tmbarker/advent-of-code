using Problems.Common;

namespace Problems.Y2018.D24;

using GroupKey = ValueTuple<Team, int>;

public class State
{
    private bool Drawn { get; }
    private Dictionary<Team, HashSet<int>> MembersByTeam { get; } = new()
    {
        { Team.ImmuneSystem, new HashSet<int>() },
        { Team.Infection, new HashSet<int>() }
    };
    
    public Dictionary<GroupKey, Group> GroupsByCk { get; } = new();

    public IEnumerable<GroupKey> GroupKeys => GroupsByCk.Keys;
    public bool Resolved => Drawn || MembersByTeam.Values.Any(members => members.Count == 0);

    public State(IEnumerable<Group> groups, bool drawn = false)
    {
        Drawn = drawn;
        RegisterGroups(groups);
    }

    public Result GetResult()
    {
        if (!Resolved)
        {
            throw new NoSolutionException();
        }

        return new Result(
            Resolution: Drawn ? Resolution.Draw : Resolution.Winner,
            Winner: Drawn ? default : GetWinningTeamInternal(),
            Score: GroupsByCk.Values.Sum(group => group.Size));
    }

    private Team GetWinningTeamInternal()
    {
        return MembersByTeam[Team.Infection].Count == 0
            ? Team.ImmuneSystem
            : Team.Infection;
    }

    private void RegisterGroups(IEnumerable<Group> groups)
    {
        foreach (var group in groups)
        {
            GroupsByCk[(group.Team, group.Id)] = group;
            MembersByTeam[group.Team].Add(group.Id);
        }
    }
}