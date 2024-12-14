using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2018.D20;

[PuzzleInfo("A Regular Map", Topics.Vectors|Topics.Graphs|Topics.StringParsing, Difficulty.Hard)]
public sealed class Solution : SolutionBase
{
    private static readonly Vec2D Start = Vec2D.Zero;
    private static readonly Dictionary<char, Vec2D> Directions = new()
    {
        { 'N', Vec2D.Up },
        { 'S', Vec2D.Down },
        { 'W', Vec2D.Left },
        { 'E', Vec2D.Right }
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
            _ => PuzzleNotSolvedString
        };
    }

    private static Dictionary<Vec2D, char> BuildMap(string regex, Vec2D start)
    {
        var map = new Dictionary<Vec2D, char> { { start, MapChars.Start } };
        var pos = start;
        var backtrack = new Stack<Vec2D>();

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

    private static Vec2D StepAndPlot(Vec2D pos, char step, Dictionary<Vec2D, char> map)
    {
        var dir = Directions[step];
        var moveTo = pos + 2 * dir;
            
        map[pos + dir] = MapChars.DoorChars[dir];
        map[moveTo] = MapChars.Empty;

        return moveTo;
    }

    private static Dictionary<Vec2D, int> BuildCosts(Dictionary<Vec2D, char> map, Vec2D start)
    {
        var visited = new HashSet<Vec2D>(collection: [start]);
        var heap = new PriorityQueue<Vec2D, int>(items: [(start, 0)]);
        
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