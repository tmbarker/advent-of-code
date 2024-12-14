using Utilities.Geometry.Euclidean;

namespace Solutions.Y2022.D17;

[PuzzleInfo("Pyroclastic Flow", Topics.Vectors|Topics.Simulation, Difficulty.Hard)]
public sealed class Solution : SolutionBase
{
    private const int ChamberWidth = 7;
    private const int SpawnHeight = 3;
    private const int SpawnOffset = 2;

    private static readonly Vec2D Gravity = Vec2D.Down;

    public override object Run(int part)
    {
        return part switch
        {
            1 => GetHeightNaive(numRocks: 2022L),
            2 => GetHeightCycle(numRocks: 1000000000000L),
            _ => PuzzleNotSolvedString
        };
    }

    private long GetHeightNaive(long numRocks)
    {
        var height = 0;
        var pile = new HashSet<Vec2D>(collection: GetFloorPositions());
        var jetPattern = JetPattern.Parse(sequence: GetInputText());

        for (var i = 0; i < numRocks; i++)
        {
            height = AddRockAndMeasure(
                rock: RockSource.Get(i),
                pos: GetSpawnPos(height),
                pile: pile,
                pattern: jetPattern);
        }

        return height;
    }

    private long GetHeightCycle(long numRocks)
    {
        var pile = new HashSet<Vec2D>(collection: GetFloorPositions());
        var jetPattern = JetPattern.Parse(sequence: GetInputText());
        
        var count = 0;
        var height = 0;
        var hash = string.Empty;
        var seen = new Dictionary<string, (int Rocks, int Height)>();

        while (seen.TryAdd(hash, (count, height)))
        {
            height = AddRockAndMeasure(
                rock: RockSource.Get(count),
                pos: GetSpawnPos(height),
                pile: pile,
                pattern: jetPattern);

            hash = HashState(count++ % 5, jetPattern.Index, height, pile);
        }
        
        var cycleLength = count - seen[hash].Rocks;
        var cycleHeight = height - seen[hash].Height;
        
        var remainder = numRocks % cycleLength;
        var numCycles = (numRocks - remainder) / cycleLength;
        
        return numCycles * cycleHeight + GetHeightNaive(remainder);
    }

    private static string HashState(int rockIndex, int jetIndex, int height, HashSet<Vec2D> pile)
    {
        var profile = new int[ChamberWidth];
        for (var x = 0; x < ChamberWidth; x++)
        {
            var depth = 0;
            while (!pile.Contains(new Vec2D(x, Y: height - depth)))
            {
                depth++;
            }
                
            profile[x] = depth;
        }

        return
            $"[Profile: {string.Join(',', profile)}]" +
            $"[Rock: {rockIndex}]" +
            $"[Jet: {jetIndex}]";
    }
    
    private static Vec2D GetSpawnPos(int pileHeight)
    {
        return new Vec2D(X: SpawnOffset, Y: SpawnHeight + pileHeight + 1);
    }
    
    private static int AddRockAndMeasure(Rock rock, Vec2D pos, HashSet<Vec2D> pile, JetPattern pattern)
    {
        while (true)
        {
            var jetVector = pattern.Next();
            if (CanMove(rock, pos, desiredMove: jetVector, pile))
            {
                pos += jetVector;
            }
            
            if (CanMove(rock, pos, desiredMove: Gravity, pile))
            {
                pos += Gravity;
                continue;
            }
    
            AddToPile(rock, pos, pile);
            break;
        }

        return pile.Max(item => item.Y);
    }
    
    private static bool CanMove(Rock rock, Vec2D pos, Vec2D desiredMove, HashSet<Vec2D> rockPile)
    {
        return rock.Shape
            .Select(p => pos + desiredMove + p)
            .All(p => p.X is >= 0 and < ChamberWidth && !rockPile.Contains(p));
    }

    private static void AddToPile(Rock rock, Vec2D pos, HashSet<Vec2D> rockPile)
    {
        foreach (var position in rock.Shape.Select(p => pos + p))
        {
            rockPile.Add(position);
        }
    }

    private static IEnumerable<Vec2D> GetFloorPositions()
    {
        for (var x = 0; x < ChamberWidth; x++)
        {
            yield return new Vec2D(x, Y: 0);
        }
    }
}