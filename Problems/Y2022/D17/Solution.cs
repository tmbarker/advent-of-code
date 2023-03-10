using Problems.Common;
using Problems.Y2022.Common;
using Utilities.Cartesian;

namespace Problems.Y2022.D17;

/// <summary>
/// Pyroclastic Flow: https://adventofcode.com/2022/day/17
/// </summary>
public class Solution : SolutionBase2022
{
    private const int ChamberWidth = 7;
    private const int SpawnHeight = 3;
    private const int SpawnOffset = 2;

    private static readonly Vector2D Gravity = Vector2D.Down;

    public override int Day => 17;
    
    public override object Run(int part)
    {
        return part switch
        {
            1 => GetHeightNaive(numRocks: 2022L, out _),
            2 => GetHeightCycle(numRocks: 1000000000000L),
            _ => ProblemNotSolvedString
        };
    }

    private long GetHeightNaive(long numRocks, out List<int> heightDeltas)
    {
        heightDeltas = new List<int>();
        
        var jetPattern = ParseJetPattern();
        var rockPile = new HashSet<Vector2D>(GetFloorPositions());
        var rockPileHeight = 0;

        for (var i = 0; i < numRocks; i++)
        {
            AddRockToPile(
                rock: RockSource.Get(i), 
                pos: new Vector2D(SpawnOffset, SpawnHeight + rockPileHeight + 1), 
                rockPile: rockPile, 
                jetPattern: jetPattern);
            
            heightDeltas.Add(rockPile.Max(r => r.Y) - rockPileHeight);
            rockPileHeight += heightDeltas.Last();
        }

        return rockPileHeight;
    }

    private long GetHeightCycle(long numRocks)
    {
        const int warmupDuration = 500;
        const int cycleFindSeed = 5000;
        
        GetHeightNaive(cycleFindSeed, out var heightDeltas);
        heightDeltas = heightDeltas.Skip(warmupDuration).ToList();

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
    
    private static void AddRockToPile(Rock rock, Vector2D pos, HashSet<Vector2D> rockPile, JetPattern jetPattern)
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
        return rock.Shape
            .Select(p => pos + desiredMove + p)
            .All(p => PositionAvailable(p, rockPile));
    }

    private static bool PositionAvailable(Vector2D targetPos, IReadOnlySet<Vector2D> rockPile)
    {
        return targetPos.X is >= 0 and < ChamberWidth && !rockPile.Contains(targetPos);
    }

    private static void MarkPositions(Rock rock, Vector2D pos, ISet<Vector2D> rockPile)
    {
        foreach (var position in rock.Shape.Select(p => pos + p))
        {
            rockPile.Add(position);
        }
    }

    private static IEnumerable<Vector2D> GetFloorPositions()
    {
        for (var x = 0; x < ChamberWidth; x++)
        {
            yield return new Vector2D(x, y: 0);
        }
    }
    
    private JetPattern ParseJetPattern()
    {
        return JetPattern.Parse(GetInputText());
    }
}