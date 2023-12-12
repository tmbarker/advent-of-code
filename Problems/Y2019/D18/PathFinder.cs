namespace Problems.Y2019.D18;

public sealed class PathFinder(Field field)
{
    public int Run(bool ignoreDoors)
    {
        var initialState = State.Initial(field.StartPos);
        var queue = new Queue<State>(new[] { initialState });
        var visited = new HashSet<State>(new[] { initialState });
        
        while (queue.Count > 0)
        {
            var state = queue.Dequeue();
            if (field.AllKeysFound(state))
            {
                return state.Steps;
            }
            
            foreach (var adj in field.GetAdj(state.Pos))
            {
                if (!ignoreDoors && field.CheckForDoorAt(adj, out var door) && !state.HasKey(char.ToLower(door)))
                {
                    continue;
                }

                var next = field.CheckForKeyAt(adj, out var key) && !state.HasKey(key)
                    ? state.AfterPickup(adj, key)
                    : state.AfterStep(adj);

                if (visited.Add(next))
                {
                    queue.Enqueue(next);
                }
            }
        }

        throw new NoSolutionException();
    }
}