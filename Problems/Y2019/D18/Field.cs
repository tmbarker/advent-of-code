using Utilities.Cartesian;
using Utilities.Extensions;

namespace Problems.Y2019.D18;

using AdjacencyList = Dictionary<Vector2D, HashSet<Vector2D>>;
using EntityMap = Dictionary<Vector2D, char>;

public class Field
{
    private const char Start = '@';
    private const char Empty = '.';
    private const char Wall = '#';

    private readonly AdjacencyList _adjacency;
    private readonly EntityMap _entities;
    private readonly HashSet<char> _keys;
    private readonly HashSet<char> _doors;

    public Vector2D StartPos { get; }

    private Field(AdjacencyList adjacency, EntityMap entities, Vector2D startPos)
    {
        _adjacency = adjacency;
        _entities = entities;
        _keys  = new HashSet<char>(entities.Values.Where(char.IsLower));
        _doors = new HashSet<char>(entities.Values.Where(char.IsUpper));

        StartPos = startPos;
    }

    public bool AllKeysFound(State state)
    {
        return _keys.All(state.HasKey);
    }
    
    public IEnumerable<Vector2D> GetAdj(Vector2D pos)
    {
        return _adjacency[pos];
    }

    public bool CheckForDoorAt(Vector2D pos, out char door)
    {
        return _entities.TryGetValue(pos, out door) && _doors.Contains(door);
    }
    
    public bool CheckForKeyAt(Vector2D pos, out char key)
    {
        return _entities.TryGetValue(pos, out key) && _keys.Contains(key);
    }
    
    public static IEnumerable<Field> Parse(IList<string> input, bool applyInputOverrides)
    {
        var grid = Grid2D<char>.MapChars(input, c => c);
        var (nominalStartPos, _) = grid.Single(kvp => kvp.Value == Start);
        
        if (applyInputOverrides)
        {
            ApplyInputOverrides(grid, nominalStartPos);
        }

        var startPositions = grid
            .Where(kvp => kvp.Value == Start)
            .Select(kvp => kvp.Key);

        foreach (var startPos in startPositions)
        {
            yield return BuildField(grid, startPos);
        }
    }

    private static Field BuildField(Grid2D<char> grid, Vector2D startPos)
    {
        var adjacency = new AdjacencyList();
        var reachable = new Dictionary<Vector2D, char> { { startPos, Start } };
        var queue = new Queue<Vector2D>(new[] { startPos });

        bool Predicate(Vector2D p) => grid.IsInDomain(p) && grid[p] != Wall;
        
        while (queue.Any())
        {
            var current = queue.Dequeue();
            foreach (var adj in current.GetAdjacentSet(DistanceMetric.Taxicab).Where(Predicate))
            {
                if (reachable.ContainsKey(adj))
                {
                    continue;
                }

                reachable.Add(adj, grid[adj]);
                queue.Enqueue(adj);
            }
        }
        
        foreach (var (pos, _) in reachable.Where(kvp => kvp.Value != Wall))
        {
            adjacency.Add(pos, new HashSet<Vector2D>(pos.GetAdjacentSet(DistanceMetric.Taxicab).Where(Predicate)));
        }
        
        return new Field(
            adjacency: adjacency,
            entities: reachable.WhereValues(c => c != Wall && c != Empty),
            startPos: startPos);
    }

    private static void ApplyInputOverrides(Grid2D<char> grid, Vector2D startPos)
    {
        foreach (var adj in startPos.GetAdjacentSet(DistanceMetric.Chebyshev))
        {
            grid[adj] = Start;
        }
        
        foreach (var adj in startPos.GetAdjacentSet(DistanceMetric.Taxicab))
        {
            grid[adj] = Wall;
        }
        
        grid[startPos] = Wall;
    }
}