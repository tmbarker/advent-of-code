using System.Text.RegularExpressions;
using Problems.Y2020.Common;
using Utilities.Cartesian;
using Utilities.Extensions;

namespace Problems.Y2020.D20;

using TileMap = IDictionary<int, Tile>;
using CongruenceMap = IDictionary<int, List<Congruence>>;
using PositionsMap = IDictionary<int, Vector2D>;

/// <summary>
/// Jurassic Jigsaw: https://adventofcode.com/2020/day/20
/// </summary>
public class Solution : SolutionBase2020
{
    private const int TileSize = 10;
    private const int ContentSize = 8;

    public override int Day => 20;
    
    public override object Run(int part)
    {
        var tiles = ParseTiles(GetInputLines());
        var congruences = BuildCongruenceMap(tiles);
        return part switch
        {
            0 => GetCornerTilesIdProduct(congruences),
            1 => FindSeaMonsters(tiles, congruences),
            _ => ProblemNotSolvedString,
        };
    }

    private static CongruenceMap BuildCongruenceMap(TileMap tiles)
    {
        var congruences = tiles.ToDictionary(
            keySelector: kvp => kvp.Key,
            elementSelector: _ => new List<Congruence>());

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
                    congruences[tileId1].Add(new Congruence(
                        toTile:   tileId2,
                        fromEdge: edgeId1,
                        toEdge:   edgeId2));
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
        
        while (true)
        {
            for (var r = 0; r < 360; r += 90)
            {
                composite.Rotate(new Rotation3D(Axis.Z, r));
                
                var seaMonsterChrCount = 0;
                var numSeaMonstersFound = 0;

                for (var y = 0; y < composite.Height; y++)
                for (var x = 0; x < composite.Width;  x++)
                {
                    var pos = new Vector2D(x, y);
                    if (composite[x, y] == SeaMonster.Chr)
                    {
                        seaMonsterChrCount++;
                    }
                    if (SeaMonster.Pattern.All(v => composite.IsInDomain(pos + v) && composite[pos + v] == SeaMonster.Chr))
                    {
                        numSeaMonstersFound++;
                    }
                }

                if (numSeaMonstersFound > 0)
                {
                    return seaMonsterChrCount - numSeaMonstersFound * SeaMonster.Pattern.Count;
                }
            }

            composite.Flip(Axis.Y);
        }
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
        var placed = new Dictionary<int, Vector2D> { { firstId, Vector2D.Zero } };
        var queue = new Queue<int>(new[] { firstId });

        while (queue.Any())
        {
            var placedTileId = queue.Dequeue();
            var placedTilePos = placed[placedTileId];
            var placedTile = tiles[placedTileId];
            
            foreach (var congruence in congruences[placedTileId])
            {
                var toTileId = congruence.ToTile;
                var toTile = tiles[toTileId];

                if (placed.ContainsKey(toTileId))
                {
                    continue;
                }

                toTile.OrientToMatch(
                    matchEdge:   congruence.ToEdge,
                    toOtherEdge: congruence.FromEdge,
                    onOtherTile: placedTile);
                
                placed.Add(toTileId, placedTilePos + placedTile.EdgeDirections[congruence.FromEdge]);
                queue.Enqueue(toTileId);
            }
        }

        placed.NormalizeValues();
        return placed;
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
            var id = int.Parse(Regex.Match(idLine, @"(\d+)").Groups[1].Value);
            var content = Grid2D<char>.WithDimensions(TileSize, TileSize);
            
            for (var y = 0; y < TileSize; y++)
            for (var x = 0; x < TileSize; x++)
            {
                content[x, y] = tileLines[TileSize - y - 1][x];
            }

            tiles.Add(id, new Tile(content));
        }

        return tiles;
    }
}