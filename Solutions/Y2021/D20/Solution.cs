using Utilities.Geometry.Euclidean;

namespace Solutions.Y2021.D20;

[PuzzleInfo("Trench Map", Topics.Vectors|Topics.Simulation, Difficulty.Hard)]
public sealed class Solution : SolutionBase
{
    private const char Lit = '#';
    private const int WindowSize = 3;

    public override object Run(int part)
    {
        ParseInput(GetInputLines(), out var litInImage, out var algorithm);
        return part switch
        {
            1 => EnhanceImage(litInImage, algorithm, steps: 2),
            2 => EnhanceImage(litInImage, algorithm, steps: 50),
            _ => PuzzleNotSolvedString
        };
    }

    private static int EnhanceImage(ISet<Vec2D> litInImage, IList<bool> algorithm, int steps)
    {
        var imageRect = new Aabb2D(extents: litInImage);
        
        for (var n = 0; n < steps; n++)
        {
            imageRect++;
            litInImage = EnhanceImage(imageRect, litInImage, algorithm, backgroundLit: algorithm[0] && n % 2 == 1);
        }

        return litInImage.Count;
    }

    private static HashSet<Vec2D> EnhanceImage(Aabb2D imageRect, ICollection<Vec2D> litInImage, IList<bool> algorithm,
        bool backgroundLit)
    {
        var litInEnhanced = new HashSet<Vec2D>();
        
        foreach (var pixel in imageRect)
        {
            var index = PixelToIndex(pixel, imageRect, litInImage, backgroundLit);
            var lit = algorithm[index];
            
            if (lit)
            {
                litInEnhanced.Add(pixel);
            }
        }

        return litInEnhanced;
    }

    private static int PixelToIndex(Vec2D pixel, Aabb2D imageRect, ICollection<Vec2D> litInInput, bool backgroundLit)
    {
        var index = 0;
        
        for (var y = 0; y < WindowSize; y++)
        for (var x = 0; x < WindowSize; x++)
        {
            var v = new Vec2D(pixel.X + x - 1, pixel.Y + y - 1);
            var bitSet = imageRect.Contains(v, false)
                ? litInInput.Contains(v)
                : backgroundLit;

            if (!bitSet)
            {
                continue;
            }

            var bitIndex = WindowSize * WindowSize - (x + WindowSize * y) - 1;
            var bitValue = (int)Math.Pow(2, bitIndex);

            index += bitValue;
        }

        return index;
    }
    
    private static void ParseInput(string[] input, out ISet<Vec2D> litInImage, out IList<bool> algorithm)
    {
        var algorithmStr = input[0];
        var imageStr = input.Skip(2).ToList();

        algorithm = algorithmStr.Select(c => c == Lit).ToList();
        litInImage = new HashSet<Vec2D>();
        
        for (var y = 0; y < imageStr.Count; y++)
        for (var x = 0; x < imageStr[0].Length; x++)
        {
            if (imageStr[y][x] == Lit)
            {
                litInImage.Add(new Vec2D(x, y));
            }
        }
    }
}