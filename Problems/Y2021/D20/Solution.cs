using Problems.Y2021.Common;
using Utilities.DataStructures.Cartesian;
using Utilities.Extensions;

namespace Problems.Y2021.D20;

/// <summary>
/// Trench Map: https://adventofcode.com/2021/day/20
/// </summary>
public class Solution : SolutionBase2021
{
    private const char Lit = '#';
    private const int Steps1 = 2;
    private const int Steps2 = 50;
    
    public override int Day => 20;
    
    public override object Run(int part)
    {
        ParseInput(GetInputLines(), out var litInImage, out var algorithm);
        return part switch
        {
            0 => EnhanceImage(litInImage, algorithm, Steps1).Count,
            1 => EnhanceImage(litInImage, algorithm, Steps2).Count,
            _ => ProblemNotSolvedString,
        };
    }

    private static ISet<Vector2D> EnhanceImage(ISet<Vector2D> litInImage, IList<bool> algorithm, int numSteps)
    {
        var imageRect = new Rect2D(litInImage, true);
        
        for (var n = 0; n < numSteps; n++)
        {
            imageRect += 1;
            litInImage = EnhanceImage(imageRect, litInImage, algorithm, algorithm[0] && n % 2 == 1);
        }
        
        return litInImage;
    }
    
    private static ISet<Vector2D> EnhanceImage(Rect2D imageRect, ICollection<Vector2D> litInImage, IList<bool> algorithm, bool backgroundLit)
    {
        var litInEnhanced = new HashSet<Vector2D>();

        for (var x = imageRect.XMin; x <= imageRect.XMax; x++)
        for (var y = imageRect.YMin; y <= imageRect.YMax; y++)
        {
            var pixel = new Vector2D(x, y);
            var index = PixelToIndex(pixel, imageRect, litInImage, backgroundLit);
            var lit = algorithm[index];
            
            if (lit)
            {
                litInEnhanced.EnsureContains(pixel);
            }
        }

        return litInEnhanced;
    }

    private static int PixelToIndex(Vector2D pixel, Rect2D imageRect, ICollection<Vector2D> litInInput, bool backgroundLit)
    {
        var index = 0;
        
        for (var y = 0; y < 3; y++)
        for (var x = 0; x < 3; x++)
        {
            var v = new Vector2D(pixel.X + x - 1, pixel.Y + y - 1);
            var bitSet = imageRect.ContainsExclusive(v)
                ? litInInput.Contains(v)
                : backgroundLit;

            if (!bitSet)
            {
                continue;
            }

            var bitIndex = 8 - (x + 3 * y);
            var bitValue = (int)Math.Pow(2, bitIndex);

            index += bitValue;
        }

        return index;
    }
    
    private static void ParseInput(IList<string> input, out ISet<Vector2D> litInImage, out IList<bool> algorithm)
    {
        var algorithmStr = input[0];
        var imageStr = input.Skip(2).ToList();

        algorithm = algorithmStr.Select(c => c == Lit).ToList();
        litInImage = new HashSet<Vector2D>();
        
        for (var y = 0; y < imageStr.Count; y++)
        for (var x = 0; x < imageStr[0].Length; x++)
        {
            if (imageStr[y][x] == Lit)
            {
                litInImage.Add(new Vector2D(x, y));
            }
        }
    }
}