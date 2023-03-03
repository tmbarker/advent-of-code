using Problems.Common;
using Problems.Y2022.Common;
using Utilities.Cartesian;

namespace Problems.Y2022.D17;

/// <summary>
/// Pyroclastic Flow: https://adventofcode.com/2022/day/17
/// </summary>
public class Solution : SolutionBase2022
{
    private const long NumRocksPart1 = 2022;
    private const long NumRocksPart2 = 1000000000000;
    private const long CycleFindSeed = 5000;
    
    private const int WarmupDuration = 500;
    private const int ChamberWidth = 7;
    private const int SpawnHeight = 3;
    private const int SpawnOffset = 2;
    
    private static readonly Vector2D Gravity = Vector2D.Down;

    public override int Day => 17;
    
    public override object Run(int part)
    {
        return part switch
        {
            1 => GetHeightNaive(NumRocksPart1, out _),
            2 => GetHeightUsingCycle(NumRocksPart2),
            _ => ProblemNotSolvedString
        };
    }

    private long GetHeightUsingCycle(long numRocks)
    {
        GetHeightNaive(CycleFindSeed, out var heightDeltas);

        heightDeltas = heightDeltas.Skip(WarmupDuration).ToList();

        var cycleFound = false;
        var cycleLength = 0;
        var cycleHeight = 0;

        for (var c = 1; c < heightDeltas.Count / 2; c++)
        {
            var cycleValid = true;
            for (var i = 0; i < heightDeltas.Count - c; i++)
            {
                if (heightDeltas[i] != heightDeltas[i + c])
                {
                    cycleValid = false;
                    break;
                }
            }

            if (cycleValid)
            {
                cycleFound = true;
                cycleLength = c;
                cycleHeight = heightDeltas.Take(cycleLength).Sum();
                break;
            }
        }

        if (!cycleFound)
        {
            throw new NoSolutionException();
        }

        var calculateNaive = numRocks % cycleLength;
        var numCycles = (numRocks - calculateNaive) / cycleLength;
        
        return numCycles * cycleHeight + GetHeightNaive(calculateNaive, out _);
    }
    
    private long GetHeightNaive(long numRocks, out List<int> heightDeltas)
    {
        heightDeltas = new List<int>();
        
        var jetPattern = ParseJetPattern();
        var rockPile = new HashSet<Vector2D>(GetFloorPositions());
        var rockPileHeight = 0;

        for (var i = 0; i < numRocks; i++)
        {
            AddRockAndMeasure(
                rock: RockSource.Get(i), 
                pos: new Vector2D(SpawnOffset, rockPileHeight + SpawnHeight + 1), 
                rockPile: rockPile, 
                jetPattern: jetPattern);
            
            heightDeltas.Add(rockPile.Max(r => r.Y) - rockPileHeight);
            rockPileHeight += heightDeltas.Last();
        }

        return rockPileHeight;
    }

    private static void AddRockAndMeasure(Rock rock, Vector2D pos, HashSet<Vector2D> rockPile, JetPattern jetPattern)
    {
        while (true)
        {
            var jetVector = jetPattern.Next();
            if (CanMove(rock, pos, jetVector, rockPile))
            {
                pos += jetVector;
            }
            
            if (CanMove(rock, pos, Gravity, rockPile))
            {
                pos += Gravity;
                continue;
            }
    
            MarkPositions(rock, pos, rockPile);
            break;
        }
    }
    
    private static bool CanMove(Rock rock, Vector2D pos, Vector2D desiredMove, IReadOnlySet<Vector2D> rockPile)
    {
        var resultingPositions = rock.Shape
            .GetAllPositions()
            .Where(p => rock.Shape[p])
            .Select(p => pos + desiredMove + p);

        return resultingPositions.All(resultingPosition => PositionAvailable(resultingPosition, rockPile));
    }

    private static bool PositionAvailable(Vector2D targetPos, IReadOnlySet<Vector2D> rockPile)
    {
        if (targetPos.Y < 0 || targetPos.X is < 0 or >= ChamberWidth) return false;
        return !rockPile.Contains(targetPos);
    }

    private static void MarkPositions(Rock rock, Vector2D pos, ISet<Vector2D> rockPile)
    {
        var occupiedPositions = rock.Shape
            .GetAllPositions()
            .Where(p => rock.Shape[p])
            .Select(p => pos + p);

        foreach (var occupiedPosition in occupiedPositions)
        {
            rockPile.Add(occupiedPosition);
        }
    }

    private static IEnumerable<Vector2D> GetFloorPositions()
    {
        for (var x = 0; x < ChamberWidth; x++)
        {
            yield return new Vector2D(x, 0);
        }
    }
    
    private JetPattern ParseJetPattern()
    {
        return JetPattern.Parse(GetInputText());
    }
}