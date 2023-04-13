using Utilities.Cartesian;

namespace Problems.Y2021.D11;

public class OctopusGrid
{
    private const int ResetTo = 0;
    private const int FlashAt = 10;
    
    private readonly Grid2D<int> _octopusStates;

    public event Action<Vector2D>? SingleFlashed;
    public event Action<int>? AllFlashed;

    public OctopusGrid(Grid2D<int> initialEnergyLevels)
    {
        _octopusStates = initialEnergyLevels;
    }

    public void Observe(int steps)
    {
        for (var i = 0; i < steps; i++)
        {
            ExecuteStep(i);
        }
    }

    public Task ObserveContinuously(CancellationToken cancellationToken)
    {
        var stepsCounter = 0;
        while (!cancellationToken.IsCancellationRequested)
        {
            ExecuteStep(stepsCounter++);
        }

        return Task.CompletedTask;
    }

    private void ExecuteStep(int stepIndex)
    {
        var flashedSet = new HashSet<Vector2D>();
        var readyToFlash = new Queue<Vector2D>();
        
        var enumeratedPositions = _octopusStates
            .GetAllPositions()
            .ToList();
        
        foreach (var position in enumeratedPositions)
        {
            IncrementAndEnqueueIfReady(position, readyToFlash);
        }

        while (readyToFlash.Count > 0)
        {
            var flashedPos = readyToFlash.Dequeue();
            if (flashedSet.Contains(flashedPos))
            {
                continue;
            }

            flashedSet.Add(flashedPos);
            RaiseSingleFlashed(flashedPos);
            
            foreach (var adjacent in flashedPos.GetAdjacentSet(Metric.Chebyshev))
            {
                if (_octopusStates.IsInDomain(adjacent))
                {
                    IncrementAndEnqueueIfReady(adjacent, readyToFlash);
                }
            }
        }

        if (flashedSet.Count == _octopusStates.Width * _octopusStates.Height)
        {
            RaiseAllFlashed(stepIndex + 1);
        }
        
        foreach (var position in flashedSet)
        {
            _octopusStates[position] = ResetTo;
        }
    }

    private void IncrementAndEnqueueIfReady(Vector2D pos, Queue<Vector2D> readyToFlashQueue)
    {
        _octopusStates[pos]++;
        if (_octopusStates[pos] >= FlashAt)
        {
            readyToFlashQueue.Enqueue(pos);
        }
    }
    
    private void RaiseSingleFlashed(Vector2D position)
    {
        SingleFlashed?.Invoke(position);
    }

    private void RaiseAllFlashed(int stepNumber)
    {
        AllFlashed?.Invoke(stepNumber);
    }
}