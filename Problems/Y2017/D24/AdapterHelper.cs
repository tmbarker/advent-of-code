namespace Problems.Y2017.D24;

public class AdapterHelper
{
    private readonly Dictionary<string, Adapter> _adapters = new();
    private readonly Dictionary<int, HashSet<Compatibility>> _compatibilities = new();

    public IReadOnlySet<Compatibility> GetCompatibilities(int port, HashSet<string> used)
    {
        return _compatibilities[port]
            .Where(c => !used.Contains(c.ViaAdapter))
            .ToHashSet();
    }

    public int GetStrength(IEnumerable<string> used)
    {
        return used.Sum(key => _adapters[key].Port1 + _adapters[key].Port2);
    }

    public void RegisterAdapter(Adapter adapter)
    {
        var compatibility1 = new Compatibility(ResultingPort: adapter.Port1, ViaAdapter: adapter.Key);
        var compatibility2 = new Compatibility(ResultingPort: adapter.Port2, ViaAdapter: adapter.Key);
        
        _adapters[adapter.Key] = adapter;
        _compatibilities.TryAdd(adapter.Port1, new HashSet<Compatibility>());
        _compatibilities.TryAdd(adapter.Port2, new HashSet<Compatibility>());
        _compatibilities[adapter.Port1].Add(compatibility2);
        _compatibilities[adapter.Port2].Add(compatibility1);
    }
}