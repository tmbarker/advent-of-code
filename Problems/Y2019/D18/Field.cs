using Utilities.Cartesian;

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
    
    public static Field Parse(IList<string> input)
    {
        var rows = input.Count;
        var cols = input[0].Length;

        var adjacency = new AdjacencyList();
        var entities = new EntityMap();
        var startPos = Vector2D.Zero;

        bool Predicate(Vector2D p) =>
            p.X >= 0 && p.X < cols && p.Y >= 0 && p.Y < rows && input[rows - p.Y - 1][p.X] != Wall;
        
        for (var y = 0; y < rows; y++)
        for (var x = 0; x < cols; x++)
        {
            var pos = new Vector2D(x, y);
            var chr = input[rows - pos.Y - 1][pos.X];

            if (chr == Wall)
            {
                continue;
            }
            
            adjacency.Add(pos, new HashSet<Vector2D>(pos.GetAdjacentSet(DistanceMetric.Taxicab).Where(Predicate)));

            if (chr == Start)
            {
                startPos = pos;
            }
            
            if (chr != Empty)
            {
                entities.Add(pos, chr);
            }
        }

        return new Field(
            adjacency: adjacency,
            entities: entities,
            startPos: startPos);
    }
}