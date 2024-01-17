using System.Text;
using System.Text.RegularExpressions;
using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2021.D13;

public static class Origami
{
    private const string FoldRegex = @"(x|y)=(\d+)";
    private const char Delimiter = ',';
    private const char HorizontalSignifier = 'y';
    private const char Marked = '#';
    private const char Empty = '.';

    public static void Parse(IEnumerable<string> input, out HashSet<Vec2D> dots, out List<(FoldType Type, int At)> folds)
    {
        dots = []; folds = [];
        
        var breakFound = false;
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                breakFound = true;
                continue;
            }

            if (!breakFound)
            {
                var components = line.Split(Delimiter);
                var x = int.Parse(components[0]);
                var y = int.Parse(components[1]);
                dots.Add(new Vec2D(x, y));
            }
            else
            {
                var match = Regex.Match(line, FoldRegex);
                var fold = match.Groups[1].Value.Contains(HorizontalSignifier) ? FoldType.Horizontal : FoldType.Vertical;
                var foldAt = match.Groups[2].ParseInt();
                folds.Add((fold, foldAt));
            }
        }
    }
    
    public static string FormPrintout(IReadOnlySet<Vec2D> dots)
    {
        var sb = new StringBuilder();
        var xMax = dots.Max(d => d.X);
        var yMax = dots.Max(d => d.Y);
        
        sb.Append('\n');
        for (var y = 0; y <= yMax; y++)
        {
            for (var x = 0; x <= xMax; x++)
            {
                sb.Append(dots.Contains(item: new Vec2D(x, y)) ? Marked : Empty);
            }
            sb.Append('\n');
        }

        return sb.ToString();
    }
}