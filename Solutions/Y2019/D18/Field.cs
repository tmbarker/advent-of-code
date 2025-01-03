using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2019.D18;

using AdjacencyList = Dictionary<Vec2D, HashSet<Vec2D>>;
using EntityMap = Dictionary<Vec2D, char>;

public sealed class Field
{
    private const char Start = '@';
    private const char Empty = '.';
    private const char Wall = '#';

    private readonly AdjacencyList _adjacency;
    private readonly EntityMap _entities;
    private readonly HashSet<char> _keys;
    private readonly HashSet<char> _doors;

    public Vec2D StartPos { get; }

    private Field(AdjacencyList adjacency, EntityMap entities, Vec2D startPos)
    {
        _adjacency = adjacency;
        _entities = entities;
        _keys  = [..entities.Values.Where(char.IsLower)];
        _doors = [..entities.Values.Where(char.IsUpper)];

        StartPos = startPos;
    }

    public bool AllKeysFound(State state)
    {
        return _keys.All(state.HasKey);
    }
    
    public IEnumerable<Vec2D> GetAdj(Vec2D pos)
    {
        return _adjacency[pos];
    }

    public bool CheckForDoorAt(Vec2D pos, out char door)
    {
        return _entities.TryGetValue(pos, out door) && _doors.Contains(door);
    }
    
    public bool CheckForKeyAt(Vec2D pos, out char key)
    {
        return _entities.TryGetValue(pos, out key) && _keys.Contains(key);
    }
    
    public static IEnumerable<Field> Parse(IList<string> input, bool applyInputOverrides)
    {
        var grid = Grid2D<char>.MapChars(input);
        var start = grid.Single(pos => grid[pos] == Start);
        
        if (applyInputOverrides)
        {
            ApplyInputOverrides(grid, start);
        }

        foreach (var startPos in grid.Where(pos => grid[pos] == Start))
        {
            yield return BuildField(grid, startPos);
        }
    }

    private static Field BuildField(Grid2D<char> grid, Vec2D startPos)
    {
        var adjacency = new AdjacencyList();
        var reachable = new EntityMap { { startPos, Start } };
        var queue = new Queue<Vec2D>(collection:[startPos]);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            var candidates = current
                .GetAdjacentSet(Metric.Taxicab)
                .Where(p => IsTraversable(p, grid));
            
            foreach (var pos in candidates)
            {
                if (reachable.ContainsKey(pos))
                {
                    continue;
                }

                reachable.Add(pos, grid[pos]);
                queue.Enqueue(pos);
            }
        }
        
        foreach (var (pos, _) in reachable.Where(kvp => kvp.Value != Wall))
        {
            var traversable = pos
                .GetAdjacentSet(Metric.Taxicab)
                .Where(p => IsTraversable(p, grid))
                .ToHashSet();
            
            adjacency.Add(pos, traversable);
        }
        
        return new Field(
            adjacency: adjacency,
            entities: reachable.WhereValues(c => c != Wall && c != Empty),
            startPos: startPos);
    }

    private static bool IsTraversable(Vec2D pos, Grid2D<char> grid)
    {
        return grid.Contains(pos) && grid[pos] != Wall;
    }
    
    private static void ApplyInputOverrides(Grid2D<char> grid, Vec2D startPos)
    {
        foreach (var adj in startPos.GetAdjacentSet(Metric.Chebyshev))
        {
            grid[adj] = Start;
        }
        
        foreach (var adj in startPos.GetAdjacentSet(Metric.Taxicab))
        {
            grid[adj] = Wall;
        }
        
        grid[startPos] = Wall;
    }
}