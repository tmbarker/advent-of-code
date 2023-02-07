using Problems.Common;

namespace Problems.Y2019.D18;

public class PathFinder
{
    private readonly Field _field;
    
    public PathFinder(Field field)
    {
        _field = field;
    }

    public int Run(bool ignoreDoors)
    {
        var initialState = State.Initial(_field.StartPos);
        var queue = new Queue<State>(new[] { initialState });
        var visited = new HashSet<State>(new[] { initialState });
        
        while (queue.Any())
        {
            var state = queue.Dequeue();
            if (_field.AllKeysFound(state))
            {
                return state.Steps;
            }
            
            foreach (var adj in _field.GetAdj(state.Pos))
            {
                if (!ignoreDoors && _field.CheckForDoorAt(adj, out var door) && !state.HasKey(char.ToLower(door)))
                {
                    continue;
                }

                var next = _field.CheckForKeyAt(adj, out var key) && !state.HasKey(key)
                    ? state.AfterPickup(adj, key)
                    : state.AfterStep(adj);

                if (visited.Contains(next))
                {
                    continue;
                }
                
                visited.Add(next);
                queue.Enqueue(next);
            }
        }

        throw new NoSolutionException();
    }
}