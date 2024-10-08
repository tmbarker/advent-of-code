using System.Text;
using Utilities.Extensions;

namespace Solutions.Y2019.D08;

[PuzzleInfo("Space Image Format", Topics.Simulation, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private const int Cols = 25;
    private const int Rows = 6;
    private const int PixelsPerLayer = Cols * Rows;

    private static readonly Dictionary<int, char> DrawChars = new()
    {
        { 0, '.' },
        { 1, '#' }
    };

    public override object Run(int part)
    {
        var layers = ParseLayers(GetInputText());
        return part switch
        {
            1 => ComputeMinLayerProduct(layers),
            2 => BuildResultingImage(layers),
            _ => PuzzleNotSolvedString
        };
    }

    private static int ComputeMinLayerProduct(IEnumerable<List<int>> layers)
    {
        var minLayer = layers.MinBy(l => l.Count(d => d == 0));
        var ones = minLayer!.Count(d => d == 1);
        var twos = minLayer!.Count(d => d == 2);

        return ones * twos;
    }

    private static string BuildResultingImage(IList<List<int>> layers)
    {
        var image = new StringBuilder();
        for (var i = 0; i < PixelsPerLayer; i++)
        {
            if (i % Cols == 0)
            {
                image.Append('\n');
            }

            image.Append(DrawChars[layers.First(l => l[i] != 2)[i]]);
        }
        
        return image.ToString();
    }

    private static List<List<int>> ParseLayers(string input)
    {
        return input
            .Select(StringExtensions.AsDigit)
            .Chunk(PixelsPerLayer)
            .Select(digits => new List<int>(digits))
            .ToList();
    }
}