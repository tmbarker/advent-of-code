using Problems.Attributes;
using Problems.Common;
using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Problems.Y2020.D20;

using TileMap = IDictionary<int, Tile>;
using CongruenceMap = IDictionary<int, List<Tile.Congruence>>;
using PositionsMap = IDictionary<int, Vector2D>;

/// <summary>
/// Jurassic Jigsaw: https://adventofcode.com/2020/day/20
/// </summary>
[Favourite("Jurassic Jigsaw", Topics.Vectors, Difficulty.Hard)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var tiles = ParseTiles(GetInputLines());
        var congruences = BuildCongruenceMap(tiles);
        
        return part switch
        {
            1 => GetCornerProduct(congruences),
            2 => FindSeaMonsters(tiles, congruences),
            _ => ProblemNotSolvedString
        };
    }

    private static CongruenceMap BuildCongruenceMap(TileMap tiles)
    {
        var congruences = tiles.ToDictionary(
            keySelector: kvp => kvp.Key,
            elementSelector: _ => new List<Tile.Congruence>());

        foreach (var (tileId1, tile1) in tiles)
        foreach (var (tileId2, tile2) in tiles)
        {
            if (tileId1 == tileId2)
            {
                continue;
            }
            
            foreach (var (edgeId1, edge1) in tile1.EdgeFingerprints)
            foreach (var (edgeId2, edge2) in tile2.EdgeFingerprints)
            {
                if (edge1.IsCongruentTo(edge2))
                {
                    congruences[tileId1].Add(item: new Tile.Congruence(
                        FromEdge: edgeId1,
                        ToEdge:   edgeId2,
                        ToTile:   tileId2));
                }
            }
        }

        return congruences;
    }
    
    private static long GetCornerProduct(CongruenceMap congruences)
    {
        return congruences.Keys
            .Where(id => congruences[id].Count == 2)
            .Select((id, _) => id)
            .Aggregate(1L, (product, next) => product * next);
    }

    private static int FindSeaMonsters(TileMap tiles, CongruenceMap congruences)
    {
        var assembled = AssembleTiles(tiles, congruences);
        var composite = ExtractImage(tiles, assembled);
        var totalMonsterCharCount = composite.Count(kvp => kvp.Value == SeaMonster.Chr);
        
        for (var i = 0; i < 2; i++)
        {
            if (CheckRotationsForSeaMonster(composite, out var numFound))
            {
                return totalMonsterCharCount - SeaMonster.Pattern.Count * numFound;
            }

            composite.Flip(about: Axis.Y);
        }

        throw new NoSolutionException();
    }

    private static bool CheckRotationsForSeaMonster(Grid2D<char> image, out int foundCount)
    {
        foundCount = 0;
        foreach (var rot in Rotation3D.RotationsAroundAxis(Axis.Z))
        {
            image.Rotate(rot);
            for (var y = 0; y < image.Height - SeaMonster.Height; y++)
            for (var x = 0; x < image.Width  - SeaMonster.Width;  x++)
            {
                if (SeaMonster.Pattern.All(v => image[new Vector2D(x, y) + v] == SeaMonster.Chr))
                {
                    foundCount++;
                }
            }
        }
        return foundCount > 0;
    }

    private static Grid2D<char> ExtractImage(TileMap tiles, PositionsMap assembled)
    {
        var contentSize = tiles.Values.First().ContentSize;
        var tilesPerSide = (int)Math.Round(Math.Sqrt(tiles.Count));
        
        var image = Grid2D<char>.WithDimensions(
            rows: tilesPerSide * contentSize,
            cols: tilesPerSide * contentSize);

        foreach (var (tileId, tilePos) in assembled)
        {
            var tile = tiles[tileId];
            var xOffset = tilePos.X * contentSize;
            var yOffset = tilePos.Y * contentSize;
            
            for (var y = 0; y < contentSize; y++)
            for (var x = 0; x < contentSize; x++)
            {
                image[xOffset + x, yOffset + y] = tile[x + 1, y + 1];
            }
        }
        
        return image;
    }

    private static PositionsMap AssembleTiles(TileMap tiles, CongruenceMap congruences)
    {
        var firstId = tiles.Keys.First();
        var positions = new Dictionary<int, Vector2D> { { firstId, Vector2D.Zero } };
        var queue = new Queue<int>(new[] { firstId });

        //  Place the first piece arbitrarily, then continuously match all pieces with known congruences to previously
        //  placed pieces. Afterwards, normalize the positions such that the lower left piece has position (0,0)
        //
        while (queue.Any())
        {
            var placedTileId = queue.Dequeue();
            var placedTile = tiles[placedTileId];
            
            foreach (var (fromEdge, toEdge, toTile) in congruences[placedTileId])
            {
                if (positions.ContainsKey(toTile))
                {
                    continue;
                }

                tiles[toTile].OrientToMatch(
                    matchEdge:   toEdge,
                    toOtherEdge: fromEdge,
                    onOtherTile: placedTile);
                
                positions.Add(toTile, positions[placedTileId] + placedTile.EdgeDirections[fromEdge]);
                queue.Enqueue(toTile);
            }
        }

        NormalizePositions(positions);
        return positions;
    }

    private static void NormalizePositions(PositionsMap map)
    {
        var dx = int.MaxValue;
        var dy = int.MaxValue;
        foreach (var vector in map.Values)
        {
            dx = Math.Min(dx, vector.X);
            dy = Math.Min(dy, vector.Y);
        }
        
        var delta = new Vector2D(x: dx, y: dy);
        foreach (var key in map.Keys)
        {
            map[key] -= delta;
        }
    }
    
    private static TileMap ParseTiles(IEnumerable<string> input)
    {
        return input
            .ChunkBy(line => !string.IsNullOrWhiteSpace(line))
            .ToDictionary(
                keySelector: chunk => chunk[0].ParseInt(),
                elementSelector: chunk => ParseTile(chunk[1..]));
    }

    private static Tile ParseTile(IList<string> rows)
    {
        return new Tile(pixels: Grid2D<char>.MapChars(strings: rows, elementFunc: c => c));
    }
}