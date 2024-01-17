using Utilities.Geometry.Euclidean;

namespace Solutions.Y2021.D11;

public sealed class OctopusGrid(Grid2D<int> energyLevels)
{
    private const int ResetTo = 0;
    private const int FlashAt = 10;

    public event Action<Vec2D>? SingleFlashed;
    public event Action<int>? AllFlashed;

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
        var flashedSet = new HashSet<Vec2D>();
        var readyToFlash = new Queue<Vec2D>();
        
        foreach (var position in energyLevels)
        {
            IncrementAndEnqueueIfReady(position, readyToFlash);
        }

        while (readyToFlash.Count > 0)
        {
            var flashedPos = readyToFlash.Dequeue();
            if (!flashedSet.Add(flashedPos))
            {
                continue;
            }

            RaiseSingleFlashed(flashedPos);
            
            foreach (var adjacent in flashedPos.GetAdjacentSet(Metric.Chebyshev))
            {
                if (energyLevels.Contains(adjacent))
                {
                    IncrementAndEnqueueIfReady(adjacent, readyToFlash);
                }
            }
        }

        if (flashedSet.Count == energyLevels.Width * energyLevels.Height)
        {
            RaiseAllFlashed(stepIndex + 1);
        }
        
        foreach (var position in flashedSet)
        {
            energyLevels[position] = ResetTo;
        }
    }

    private void IncrementAndEnqueueIfReady(Vec2D pos, Queue<Vec2D> readyToFlashQueue)
    {
        energyLevels[pos]++;
        if (energyLevels[pos] >= FlashAt)
        {
            readyToFlashQueue.Enqueue(pos);
        }
    }
    
    private void RaiseSingleFlashed(Vec2D position)
    {
        SingleFlashed?.Invoke(position);
    }

    private void RaiseAllFlashed(int stepNumber)
    {
        AllFlashed?.Invoke(stepNumber);
    }
}