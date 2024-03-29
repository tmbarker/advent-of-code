using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2018.D17;

internal delegate IEnumerable<Vec2D> VeinBuilder(int fixedComponent, int variableMin, int variableMax);

public sealed class Reservoir
{
    private static readonly Dictionary<char, VeinBuilder> VeinBuilders = new()
    {
        { 'y', ParseRow },
        { 'x', ParseCol }
    };

    private readonly Dictionary<Vec2D, char> _positionStates = new();

    private Reservoir(IEnumerable<Vec2D> clayPositions)
    {
        foreach (var position in clayPositions)
        {
            _positionStates[position] = Terrain.Clay;
        }
        
        MinDepth = _positionStates.Keys.Min(p => p.Y);
        MaxDepth = _positionStates.Keys.Max(p => p.Y);
        
        _positionStates[SpringPos] = Terrain.Spring;
    }

    public static Vec2D SpringPos => new (500, 0);
    public int MinDepth { get; }
    public int MaxDepth { get; }
    
    private char this[int x, int y] => this[new Vec2D(x, y)];
    
    public char this[Vec2D pos]
    {
        get => _positionStates.ContainsKey(pos) ? _positionStates[pos] : Terrain.Empty;
        set => _positionStates[pos] = value;
    }

    public void Print()
    {
        var aabb = new Aabb2D(
            extents: _positionStates.Keys.Where(p => _positionStates[p] != Terrain.Empty).ToList());

        for (var y = aabb.Min.Y; y <= aabb.Max.Y; y++)
        {
            for (var x = aabb.Min.X - 1; x <= aabb.Max.X + 1; x++)
            {
                Console.Write(this[x, y]);
            }
            Console.WriteLine();
        }
    }

    public int GetMaterialsCount(params char[] materials)
    {
        var validPositions = _positionStates.Keys.Where(p => p.Y >= MinDepth && p.Y <= MaxDepth);
        return materials.Sum(m => validPositions.Count(p => _positionStates[p] == m));
    }
    
    public static Reservoir Parse(IEnumerable<string> input)
    {
        return new Reservoir(clayPositions: input.SelectMany(ParseClayVein));
    }
    
    private static IEnumerable<Vec2D> ParseClayVein(string line)
    {
        var numbers = line.ParseInts();
        return VeinBuilders[line[0]].Invoke(
            fixedComponent: numbers[0],
            variableMin: numbers[1],
            variableMax: numbers[2]);
    }

    private static IEnumerable<Vec2D> ParseRow(int y, int xMin, int xMax)
    {
        for (var x = xMin; x <= xMax; x++)
        {
            yield return new Vec2D(x, y);
        }
    }
    
    private static IEnumerable<Vec2D> ParseCol(int x, int yMin, int yMax)
    {
        for (var y = yMin; y <= yMax; y++)
        {
            yield return new Vec2D(x, y);
        }
    }
}