using Utilities.Geometry.Euclidean;

namespace Solutions.Y2021.D11;

public sealed class OctopusGrid(Grid2D<int> energyMap)
{
    private const int ResetTo = 0;
    private const int FlashAt = 10;

    public enum FlashType
    {
        Single,
        All
    }

    public int CountFlashes(int steps, FlashType type)
    {
        return Enumerable
            .Range(1, steps)
            .Sum(_ => Step(type));
    }

    public int CountStepsUntilFlash(FlashType type)
    {
        var stepIndex = 1;
        
        while (Step(target: type) == 0)
        {
            stepIndex++;
        }

        return stepIndex;
    }

    private int Step(FlashType target)
    {
        var count = 0;
        var flashedPositions = new HashSet<Vec2D>();
        var readyPositions = new Queue<Vec2D>();
        
        foreach (var position in energyMap)
        {
            IncrementAndEnqueueIfReady(position, readyPositions);
        }

        while (readyPositions.Count > 0)
        {
            var flashedPos = readyPositions.Dequeue();
            if (!flashedPositions.Add(flashedPos))
            {
                continue;
            }

            if (target == FlashType.Single)
            {
                count++;
            }
            
            foreach (var adjacent in flashedPos.GetAdjacentSet(Metric.Chebyshev).Where(energyMap.Contains))
            {
                IncrementAndEnqueueIfReady(adjacent, readyPositions);
            }
        }

        if (target == FlashType.All && flashedPositions.Count == energyMap.Width * energyMap.Height)
        {
            count++;
        }
        
        foreach (var position in flashedPositions)
        {
            energyMap[position] = ResetTo;
        }

        return count;
    }

    private void IncrementAndEnqueueIfReady(Vec2D pos, Queue<Vec2D> readyToFlashQueue)
    {
        energyMap[pos]++;
        if (energyMap[pos] >= FlashAt)
        {
            readyToFlashQueue.Enqueue(pos);
        }
    }
}