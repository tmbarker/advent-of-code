using Utilities.Extensions;

namespace Utilities.Graph;

/// <summary>
/// A primitive directed graph template 
/// </summary>
/// <typeparam name="T">The type of value associated with each vertex</typeparam>
public class DirectedGraph<T> where T : IEquatable<T>
{
    public readonly record struct Edge(T From, T To);
    
    public Dictionary<T, HashSet<T>> Incoming { get; } = new();
    public Dictionary<T, HashSet<T>> Outgoing { get; } = new();

    public IEnumerable<T> Sources => Outgoing.Keys.Where(v => !Incoming.ContainsKey(v) || !Incoming[v].Any());
    public IEnumerable<T> Sinks => Incoming.Keys.Where(v => !Outgoing.ContainsKey(v) || !Outgoing[v].Any());

    public DirectedGraph(IEnumerable<Edge> edges)
    {
        foreach (var edge in edges)
        {
            AddEdge(edge);
        }
    }

    public void AddEdge(Edge edge)
    {
        AddEdge(edge.From, edge.To);
    }

    public void AddEdge(T from, T to)
    {
        Incoming.EnsureContainsKey(to,   new HashSet<T>());
        Incoming.EnsureContainsKey(from, new HashSet<T>());
        Incoming[to].Add(from);
        
        Outgoing.EnsureContainsKey(to,   new HashSet<T>());
        Outgoing.EnsureContainsKey(from, new HashSet<T>());
        Outgoing[from].Add(to);
    }

    public void RemoveEdge(Edge edge)
    {
        RemoveEdge(edge.From, edge.To);
    }
    
    public void RemoveEdge(T from, T to)
    {
        Incoming[to].Remove(from);
        Outgoing[from].Remove(to);
    }
}