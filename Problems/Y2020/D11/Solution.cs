using Problems.Y2020.Common;
using Utilities.Cartesian;

namespace Problems.Y2020.D11;

/// <summary>
/// Seating System: https://adventofcode.com/2020/day/11
/// </summary>
public class Solution : SolutionBase2020
{ 
    public override int Day => 11;
    
    public override object Run(int part)
    {
        var seatMap = SeatMap.Parse(GetInputLines());
        return part switch
        {
            1 =>  CountOccupiedSeatsAtSteadyState(seatMap, Concern.Adjacent, moveThreshold: 4),
            2 =>  CountOccupiedSeatsAtSteadyState(seatMap, Concern.Visible, moveThreshold: 5),
            _ => ProblemNotSolvedString
        };
    }

    private static int CountOccupiedSeatsAtSteadyState(SeatMap map, Concern concern, int moveThreshold)
    {
        var changed = true;
        var nextOccupied = new HashSet<Vector2D>();
        
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

    private static int CountOccupiedOfConcern(Vector2D seat, SeatMap map, Concern concern)
    {
        return concern switch
        {
            Concern.Adjacent => CountOccupiedAdjacent(seat, map),
            Concern.Visible => CountOccupiedFirstVisible(seat, map),
            _ => throw new ArgumentOutOfRangeException(nameof(concern))
        };
    }
    
    private static int CountOccupiedAdjacent(Vector2D seat, SeatMap map)
    {
        return seat
            .GetAdjacentSet(DistanceMetric.Chebyshev)
            .Count(adj => map.SeatExistsAt(adj) && map[adj]);
    }
    
    private static int CountOccupiedFirstVisible(Vector2D seat, SeatMap map)
    {
        var count = 0;
        var directions = Vector2D.Zero.GetAdjacentSet(DistanceMetric.Chebyshev);
        
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