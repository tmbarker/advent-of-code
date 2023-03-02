using System.Text;
using Problems.Y2019.Common;

namespace Problems.Y2019.D08;

/// <summary>
/// Space Image Format: https://adventofcode.com/2019/day/8
/// </summary>
public class Solution : SolutionBase2019
{
    private const int Cols = 25;
    private const int Rows = 6;
    private const int PixelsPerLayer = Cols * Rows;

    private static readonly Dictionary<int, char> DrawChars = new()
    {
        { 0, '.' },
        { 1, '#' },
    };

    public override int Day => 8;
    
    public override object Run(int part)
    {
        var layers = ParseLayers(GetInputText());
        return part switch
        {
            1 =>  ComputeMinLayerProduct(layers),
            2 =>  BuildResultingImage(layers),
            _ => ProblemNotSolvedString
        };
    }

    private static int ComputeMinLayerProduct(IEnumerable<IList<int>> layers)
    {
        var minLayer = layers.MinBy(l => l.Count(d => d == 0));
        var ones = minLayer!.Count(d => d == 1);
        var twos = minLayer!.Count(d => d == 2);

        return ones * twos;
    }

    private static string BuildResultingImage(IList<IList<int>> layers)
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

    private static IList<IList<int>> ParseLayers(string input)
    {
        return input
            .Select(chars => chars - '0')
            .Chunk(PixelsPerLayer)
            .Select(digits => (IList<int>)new List<int>(digits))
            .ToList();
    }
}