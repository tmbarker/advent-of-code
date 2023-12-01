using Problems.Common;
using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Problems.Y2018.D20;

/// <summary>
/// A Regular Map: https://adventofcode.com/2018/day/20
/// </summary>
public class Solution : SolutionBase
{
    private static readonly Vector2D Start = Vector2D.Zero;
    private static readonly Dictionary<char, Vector2D> Directions = new()
    {
        { 'N', Vector2D.Up },
        { 'S', Vector2D.Down },
        { 'W', Vector2D.Left },
        { 'E', Vector2D.Right }
    };

    public override object Run(int part)
    {
        var regex = GetInputText();
        var map = BuildMap(regex, Start);
        var costs = BuildCosts(map, Start);
        
        return part switch
        {
            1 => costs.Values.Max(),
            2 => costs.Values.Count(c => c >= 1000),
            _ => ProblemNotSolvedString
        };
    }

    private static Dictionary<Vector2D, char> BuildMap(string regex, Vector2D start)
    {
        var map = new Dictionary<Vector2D, char> { { start, MapChars.Start } };
        var pos = start;
        var backtrack = new Stack<Vector2D>();

        foreach (var step in regex)
        {
            switch (step)
            {
                case '^':
                case '$':
                    continue;
                case '(':
                    backtrack.Push(pos);
                    continue;
                case ')':
                    backtrack.Pop();
                    continue;
                case '|':
                    pos = backtrack.Peek();
                    continue;
                case 'N':
                case 'S':
                case 'E':
                case 'W':
                    pos = StepAndPlot(
                        pos: pos,
                        step: step,
                        map: map);
                    continue;
            }
        }

        return map;
    }

    private static Vector2D StepAndPlot(Vector2D pos, char step, IDictionary<Vector2D, char> map)
    {
        var dir = Directions[step];
        var moveTo = pos + 2 * dir;
            
        map[pos + dir] = MapChars.DoorChars[dir];
        map[moveTo] = MapChars.Empty;

        return moveTo;
    }

    private static Dictionary<Vector2D, int> BuildCosts(Dictionary<Vector2D, char> map, Vector2D start)
    {
        var visited = new HashSet<Vector2D> { start };
        var heap = new PriorityQueue<Vector2D, int>(new[] { (start, 0) });
        
        var aabb = new Aabb2D(extents: map.Keys);
        var costs = aabb.ToDictionary(
            keySelector: p => p,
            elementSelector: p => p == start ? 0 : int.MaxValue);

        while (heap.Count > 0)
        {
            var pos = heap.Dequeue();
            foreach (var dir in Directions.Values)
            {
                var targetDoor = pos + dir;
                var targetRoom = pos + 2 * dir;

                var doorValid = map.ContainsKey(targetDoor) && MapChars.DoorChars.ContainsValue(map[targetDoor]);
                var roomValid = map.ContainsKey(targetRoom) && map[targetRoom] == MapChars.Empty;
                
                if (!doorValid || !roomValid || visited.Contains(targetRoom))
                {
                    continue;
                }

                var distanceViaCurrent = costs[pos] + 1;
                if (distanceViaCurrent < costs[targetRoom])
                {
                    costs[targetRoom] = distanceViaCurrent;
                }

                visited.Add(targetRoom);
                heap.Enqueue(targetRoom, costs[targetRoom]);
            }
        }

        return costs.WhereValues(v => v != int.MaxValue);
    }
}