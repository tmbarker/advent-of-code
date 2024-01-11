using Utilities.Collections;

namespace Utilities.Graph;

/// <summary>
/// A primitive directed graph template 
/// </summary>
/// <typeparam name="T">The type of value associated with each vertex</typeparam>
public sealed class DirectedGraph<T> where T : IEquatable<T>
{
    public readonly record struct Edge(T From, T To);
    
    public DefaultDict<T, HashSet<T>> Incoming { get; } = new(defaultSelector: _ => []);
    public DefaultDict<T, HashSet<T>> Outgoing { get; } = new(defaultSelector: _ => []);

    public IEnumerable<T> Sources => Outgoing.Keys.Where(v => Incoming[v].Count == 0);
    public IEnumerable<T> Sinks => Incoming.Keys.Where(v => Outgoing[v].Count == 0);

    public DirectedGraph()
    {
    }
    
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
        Incoming[to].Add(from);
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