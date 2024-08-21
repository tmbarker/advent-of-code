using Utilities.Geometry.Euclidean;

namespace Solutions.Y2020.D11;

[PuzzleInfo("Seating System", Topics.Vectors|Topics.Simulation, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{ 
    public override object Run(int part)
    {
        var seatMap = SeatMap.Parse(GetInputLines());
        return part switch
        {
            1 => CountOccupiedAtSteadyState(seatMap, Concern.Adjacent, moveThreshold: 4),
            2 => CountOccupiedAtSteadyState(seatMap, Concern.Visible, moveThreshold: 5),
            _ => PuzzleNotSolvedString
        };
    }

    private static int CountOccupiedAtSteadyState(SeatMap map, Concern concern, int moveThreshold)
    {
        var changed = true;
        var nextOccupied = new HashSet<Vec2D>();
        
        while (changed)
        {
            changed = false;
            nextOccupied.Clear();
            
            foreach (var (seat, occupied) in map)
            {
                var occupiedOfConcernCount = CountOccupiedOfConcern(seat, map, concern);
                var willFill = !occupied && occupiedOfConcernCount == 0;
                var willEmpty = occupied && occupiedOfConcernCount >= moveThreshold;

                if (willFill || (occupied && !willEmpty))
                {
                    nextOccupied.Add(seat);
                }

                if (willFill || willEmpty)
                {
                    changed = true;
                }
            }

            map.UpdateOccupancy(nextOccupied);
        }

        return map.CountOccupied();
    }

    private static int CountOccupiedOfConcern(Vec2D seat, SeatMap map, Concern concern)
    {
        return concern switch
        {
            Concern.Adjacent => CountOccupiedAdjacent(seat, map),
            Concern.Visible => CountOccupiedFirstVisible(seat, map),
            _ => throw new ArgumentOutOfRangeException(nameof(concern))
        };
    }
    
    private static int CountOccupiedAdjacent(Vec2D seat, SeatMap map)
    {
        return seat
            .GetAdjacentSet(Metric.Chebyshev)
            .Count(adj => map.SeatExistsAt(adj) && map[adj]);
    }
    
    private static int CountOccupiedFirstVisible(Vec2D seat, SeatMap map)
    {
        var count = 0;
        var directions = Vec2D.Zero.GetAdjacentSet(Metric.Chebyshev);
        
        foreach (var direction in directions)
        {
            var pos = seat + direction;
            while (map.IsPosInBounds(pos))
            {
                if (map.SeatExistsAt(pos))
                {
                    count += map[pos] ? 1 : 0;
                    break;
                }

                pos += direction;
            }
        }

        return count;
    }
}