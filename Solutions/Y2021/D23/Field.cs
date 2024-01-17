using Utilities.Geometry.Euclidean;
using Utilities.Graph;

namespace Solutions.Y2021.D23;

public sealed class Field
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

    private readonly Dictionary<(Vec2D, Vec2D), int> _moveDistances;

    public Dictionary<char, SideRoom> SideRooms { get; }
    public Dictionary<Vec2D, HashSet<Vec2D>> AdjacencyList { get; }
    public IReadOnlySet<Vec2D> WaitingPositions { get; }

    public Field(int sideRoomDepth)
    {
        var allPositions = new List<Vec2D>();
        var hallwayPositions = new List<Vec2D>();
        
        for (var x = 0; x < HallwayLength; x++)
        {
            var position = new Vec2D(x, sideRoomDepth);
            
            allPositions.Add(position);
            hallwayPositions.Add(position);
        }

        var destinations = new Dictionary<char, SideRoom>();
        foreach (var (actor, abscissa) in SideRoomAbscissas)
        {
            var positions = new List<Vec2D>();
            for (var y = 0; y < sideRoomDepth; y++)
            {
                positions.Add(new Vec2D(abscissa, y));
            }

            allPositions.AddRange(positions);
            destinations.Add(actor, new SideRoom(abscissa, sideRoomDepth));
        }

        var adjacencyList = new Dictionary<Vec2D, HashSet<Vec2D>>();
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
    
    public Move FormMove(char actor, Vec2D from, Vec2D to)
    {
        return new Move(
            From: from,
            To: to,
            Cost: StepCosts[actor] * _moveDistances[(from, to)],
            Type: WaitingPositions.Contains(to) ? MoveType.ToHallway : MoveType.ToSideRoom);
    }
}