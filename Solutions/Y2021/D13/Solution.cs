using System.Collections.Frozen;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2021.D13;

[PuzzleInfo("Transparent Origami", Topics.Vectors, Difficulty.Easy, favourite: true)]
public sealed class Solution : SolutionBase
{
    private delegate HashSet<Vector2D> FoldTransform(int foldAt, HashSet<Vector2D> dots);
    private static readonly Dictionary<FoldType, FoldTransform> FoldTransforms = new()
    {
        { FoldType.Horizontal, HorizontalFoldTransform},
        { FoldType.Vertical,   VerticalFoldTransform}
    };

    public override object Run(int part)
    {
        Origami.Parse(GetInputLines(), out var dots, out var folds);
        return part switch
        {
            1 => PerformFolds(dots, folds.Take(1)).Count,
            2 => GetOrigamiPrintout(dots, folds),
            _ => ProblemNotSolvedString
        };
    }

    private static string GetOrigamiPrintout(HashSet<Vector2D> dots, IEnumerable<(FoldType Type, int At)> folds)
    {
        return Origami.FormPrintout(PerformFolds(dots, folds));
    } 
    
    private static HashSet<Vector2D> PerformFolds(HashSet<Vector2D> dots, IEnumerable<(FoldType Type, int At)> folds)
    {
        return folds.Aggregate(
            seed: dots,
            func: (current, fold) => FoldTransforms[fold.Type](fold.At, current));
    }

    private static HashSet<Vector2D> HorizontalFoldTransform(int foldAt, HashSet<Vector2D> dots)
    {
        foreach (var point in dots.ToFrozenSet())
        {
            if (point.Y < foldAt)
            {
                continue;
            }

            dots.Remove(point);
            dots.Add(new Vector2D(point.X, 2 * foldAt - point.Y));
        }

        return dots;
    }

    private static HashSet<Vector2D> VerticalFoldTransform(int foldAt, HashSet<Vector2D> dots)
    {
        foreach (var point in dots.ToFrozenSet())
        {
            if (point.X < foldAt)
            {
                continue;
            }

            dots.Remove(point);
            dots.Add(new Vector2D(2 * foldAt - point.X, point.Y));
        }

        return dots;
    }
}