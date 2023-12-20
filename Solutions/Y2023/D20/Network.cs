using System.Text.RegularExpressions;
using Utilities.Collections;
using Utilities.Extensions;
using Utilities.Graph;

namespace Solutions.Y2023.D20;

public class Network
{
    private const string Button = "button";
    private const string Broadcaster = "broadcaster";
    private const string Receiver = "rx";
    
    private readonly record struct Pulse(bool Value, string Src, string Dst);
    
    private readonly DirectedGraph<string> _graph;
    private readonly DefaultDict<string, ModuleType> _types;
    private readonly DefaultDict<string, bool> _memFlip = new(defaultValue: false);
    private readonly DefaultDict<string, Dictionary<string, bool>> _memConj = new(defaultSelector: _ => []);

    public IReadOnlySet<string> ReceiverInputs => _graph.Incoming[_graph.Incoming[Receiver].Single()];
    
    private Network(DirectedGraph<string> graph, DefaultDict<string, ModuleType> types)
    {
        _graph = graph;
        _types = types;
        InitMemory();
    }

    private void InitMemory()
    {
        foreach (var id in _types.Keys.Where(id => _types[id] == ModuleType.Conjunction))
        foreach (var input in _graph.Incoming[id])
        {
            _memConj[id][input] = false;
        }
    }

    public Trace Simulate(int b)
    {
        var pulses = new DefaultDict<bool, long>(defaultValue: 0L);
        var watches = new DefaultDict<string, List<long>>(defaultSelector: _ => []);
        
        for (var i = 0L; i < b; i++)
        {
            var queue = new Queue<Pulse>(collection: [new Pulse(Value: false, Src: Button, Dst: Broadcaster)]);
            while (queue.Count > 0)
            {
                var (val, src, dst) = queue.Dequeue();
                
                pulses[val]++;
                if (val && _types[dst] == ModuleType.Conjunction && !_memConj[dst][src])
                {
                    watches[dst].Add(item: i + 1);   
                }
                
                bool propagate;
                switch (_types[dst])
                {
                    case ModuleType.Broadcaster:
                        propagate = val;
                        break;
                    case ModuleType.FlipFlop when !val:
                        propagate = _memFlip[dst] = !_memFlip[dst];
                        break;
                    case ModuleType.Conjunction:
                        _memConj[dst][src] = val;
                        propagate = !_memConj[dst].Values.All(v => v);
                        break;
                    case ModuleType.Untyped:
                    default:
                        continue;
                }
                
                foreach (var output in _graph.Outgoing[dst])
                {
                    queue.Enqueue(item: new Pulse(Value: propagate, Src: dst, Dst: output));
                }
            }
        }

        return new Trace(pulses, watches);
    }
    
    public static Network Parse(IEnumerable<string> input)
    {
        var types = new DefaultDict<string, ModuleType>(defaultValue: ModuleType.Untyped);
        var graph = new DirectedGraph<string>();
        
        foreach (var line in input)
        {
            var match = Regex.Match(line, pattern: "(?<Id>[%&a-z]+) -> (?:(?<Adj>[a-z]+)(?:, )?)+");
            var outputs = match.Groups["Adj"].Captures.Select(c => c.Value);
            var id = match.Groups["Id"].Value;
            
            ModuleType type;
            switch (id[0])
            {
                case '%':
                    type = ModuleType.FlipFlop;
                    id = id[1..];
                    break;
                case '&':
                    type = ModuleType.Conjunction;
                    id = id[1..];
                    break;
                default:
                    type = ModuleType.Broadcaster;
                    break;
            }
            
            types[id] = type;
            outputs.ForEach(to => graph.AddEdge(from: id, to: to));
        }

        return new Network(graph, types);
    }
}