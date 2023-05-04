using Utilities.Cartesian;
using Utilities.Graph;

namespace Problems.Y2021.D23;

public class Field
{
    private const int  HallwayLength = 11;
    private const char Amber =  'A';
    private const char Bronze = 'B';
    private const char Copper = 'C';
    private const char Desert = 'D';
    
    private static readonly Dictionary<char, int> StepCosts = new()
    {
        {Amber,  1},
        {Bronze, 10},
        {Copper, 100},
        {Desert, 1000}
    };
    private static readonly Dictionary<char, int> SideRoomAbscissas = new()
    {
        {Amber,  2},
        {Bronze, 4},
        {Copper, 6},
        {Desert, 8}
    };

    private readonly Dictionary<(Vector2D, Vector2D), int> _moveDistances;

    public Dictionary<char, SideRoom> SideRooms { get; }
    public Dictionary<Vector2D, HashSet<Vector2D>> AdjacencyList { get; }
    public IReadOnlySet<Vector2D> WaitingPositions { get; }

    public Field(int sideRoomDepth)
    {
        var allPositions = new List<Vector2D>();
        var hallwayPositions = new List<Vector2D>();
        
        for (var x = 0; x < HallwayLength; x++)
        {
            var position = new Vector2D(x, sideRoomDepth);
            
            allPositions.Add(position);
            hallwayPositions.Add(position);
        }

        var destinations = new Dictionary<char, SideRoom>();
        foreach (var (actor, abscissa) in SideRoomAbscissas)
        {
            var positions = new List<Vector2D>();
            for (var y = 0; y < sideRoomDepth; y++)
            {
                positions.Add(new Vector2D(abscissa, y));
            }

            allPositions.AddRange(positions);
            destinations.Add(actor, new SideRoom(abscissa, sideRoomDepth));
        }

        var adjacencyList = new Dictionary<Vector2D, HashSet<Vector2D>>();
        foreach (var position in allPositions)
        {
            var adjacencies = position
                .GetAdjacentSet(Metric.Taxicab)
                .Where(allPositions.Contains)
                .ToHashSet();
            adjacencyList.Add(position, adjacencies);
        }

        _moveDistances = GraphHelper.FloydWarshallUnweighted(adjacencyList);
        SideRooms = destinations;
        AdjacencyList = adjacencyList;
        WaitingPositions = hallwayPositions.Where(p => !SideRoomAbscissas.ContainsValue(p.X)).ToHashSet();
    }
    
    public Move FormMove(char actor, Vector2D from, Vector2D to)
    {
        return new Move(
            From: from,
            To: to,
            Cost: StepCosts[actor] * _moveDistances[(from, to)],
            Type: WaitingPositions.Contains(to) ? MoveType.ToWaiting : MoveType.ToSideRoom);
    }
}