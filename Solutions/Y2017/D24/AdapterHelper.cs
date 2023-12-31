using Utilities.Collections;

namespace Solutions.Y2017.D24;

public sealed class AdapterHelper
{
    private readonly Dictionary<string, Adapter> _adapters = new();
    private readonly DefaultDict<int, HashSet<Compatibility>> _compatibilities = new(defaultSelector: _ => []);

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
        _compatibilities[adapter.Port1].Add(compatibility2);
        _compatibilities[adapter.Port2].Add(compatibility1);
    }
}