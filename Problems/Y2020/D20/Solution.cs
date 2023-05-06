using Problems.Attributes;
using Problems.Common;
using Utilities.Cartesian;
using Utilities.Extensions;

namespace Problems.Y2020.D20;

using TileMap = IDictionary<int, Tile>;
using CongruenceMap = IDictionary<int, List<Tile.Congruence>>;
using PositionsMap = IDictionary<int, Vector2D>;

/// <summary>
/// Jurassic Jigsaw: https://adventofcode.com/2020/day/20
/// </summary>
[Favourite("Jurassic Jigsaw", Topics.Vectors, Difficulty.Hard)]
public class Solution : SolutionBase
{
    private const int TileSize = 10;
    private const int ContentSize = 8;

    public override object Run(int part)
    {
        var tiles = ParseTiles(GetInputLines());
        var congruences = BuildCongruenceMap(tiles);
        
        return part switch
        {
            1 => GetCornerTilesIdProduct(congruences),
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
        foreach (var (tileId2, tile2) in tiles.WhereKeys(id => id != tileId1))
        {
            foreach (var (edgeId1, edge1) in tile1.EdgeFingerprints)
            foreach (var (edgeId2, edge2) in tile2.EdgeFingerprints)
            {
                if (edge1.IsCongruentTo(edge2))
                {
                    congruences[tileId1].Add(new Tile.Congruence(
                        FromEdge: edgeId1,
                        ToEdge:   edgeId2,
                        ToTile:   tileId2));
                }
            }
        }

        return congruences;
    }
    
    private static long GetCornerTilesIdProduct(CongruenceMap congruences)
    {
        return congruences
            .WhereValues(set => set.Count == 2).Keys
            .Aggregate(1L, (product, next) => product * next);
    }

    private static int FindSeaMonsters(TileMap tiles, CongruenceMap congruences)
    {
        var assembled = AssembleTiles(tiles, congruences);
        var composite = BuildCompositeImage(tiles, assembled);
        var seaMonsterChrCount = composite.Count(kvp => kvp.Value == SeaMonster.Chr);

        //  We need to check 8 orientations: 0, 90, 180, and 270 deg on either side of the image
        // 
        for (var i = 0; i < 2; i++)
        {
            if (CheckRotationsForSeaMonster(composite, out var numFound))
            {
                return seaMonsterChrCount - SeaMonster.Pattern.Count * numFound;
            }
            
            composite.Flip(Axis.Y);
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

    private static Grid2D<char> BuildCompositeImage(TileMap tiles, PositionsMap assembled)
    {
        var tilesPerSide = (int)Math.Round(Math.Sqrt(tiles.Count));
        var image = Grid2D<char>.WithDimensions(
            rows: tilesPerSide * ContentSize,
            cols: tilesPerSide * ContentSize);

        foreach (var (tileId, tilePos) in assembled)
        {
            for (var y = 0; y < ContentSize; y++)
            for (var x = 0; x < ContentSize; x++)
            {
                var tile = tiles[tileId];
                var xOffset = tilePos.X * ContentSize;
                var yOffset = tilePos.Y * ContentSize;

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

        positions.NormalizeValues();
        return positions;
    }

    private static TileMap ParseTiles(IEnumerable<string> input)
    {
        const int idLines = 1;
        const int breakLines = 1;

        var tiles = new Dictionary<int, Tile>();
        var tileChunks = input
            .Chunk(idLines + TileSize + breakLines)
            .Select(c => (c.First(), new List<string>(c.Skip(idLines).Take(TileSize))));

        foreach (var (idLine, tileLines) in tileChunks)
        {
            var id = idLine.ParseInt();
            var content = Grid2D<char>.MapChars(tileLines, c => c);

            tiles.Add(id, new Tile(content));
        }

        return tiles;
    }
}