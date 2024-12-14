using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2022.D08;

[PuzzleInfo("Treetop Tree House", Topics.Vectors, Difficulty.Easy, favourite: true)]
public sealed class Solution : SolutionBase
{
    private const int MinHeight = 0;
    private const int MaxHeight = 9;

    public override object Run(int part)
    {
        var input = GetInputLines();
        var trees = Grid2D<int>.MapChars(input, elementFunc: StringExtensions.AsDigit);
        
        return part switch
        {
            1 => CountVisibleTrees(trees),
            2 => GetMaxScenicScore(trees),
            _ => PuzzleNotSolvedString
        };
    }

    private static int CountVisibleTrees(Grid2D<int> trees)
    {
        var visibleSet = new HashSet<Vec2D>();
        var linesOfSight = GetLinesOfSight(trees);

        foreach (var lineOfSight in linesOfSight)
        {
            EvaluateLineOfSight(trees, lineOfSight, visibleSet);
        }

        return visibleSet.Count;
    }

    private static List<IEnumerable<Vec2D>> GetLinesOfSight(Grid2D<int> trees)
    {
        var los = new List<IEnumerable<Vec2D>>();
        
        // Horizontal
        //
        for (var y = 0; y < trees.Height; y++)
        {
            var row = y;
            var positions = Enumerable.Range(start: 0, count: trees.Width).Select(x => new Vec2D(x, Y: row));
            var enumerated = positions.ToArray();
            
            los.Add(enumerated);
            los.Add(enumerated.Reverse());
        }
        
        // Vertical
        //
        for (var x = 0; x < trees.Width; x++)
        {
            var col = x;
            var positions = Enumerable.Range(start: 0, count: trees.Height).Select(y => new Vec2D(X: col, y));
            var enumerated = positions.ToArray();
            
            los.Add(enumerated);
            los.Add(enumerated.Reverse());
        }

        return los;
    }
    
    private static void EvaluateLineOfSight(Grid2D<int> trees, IEnumerable<Vec2D> positions, HashSet<Vec2D> visible)
    {
        var max = MinHeight - 1;
        foreach (var position in positions)
        {
            var height = trees[position];
            if (height > max)
            {
                max = height;
                visible.Add(position);
            }

            if (height >= MaxHeight)
            {
                return;
            }
        }
    }
    
    private static int GetMaxScenicScore(Grid2D<int> trees)
    {
        var maxScore = 0;

        // NOTE: The scenic score of edge trees will always be 0 and does not need to be evaluated
        //
        for (var j = 1; j < trees.Height - 1; j++)
        for (var i = 1; i < trees.Width - 1; i++)
        {
            maxScore = Math.Max(maxScore, GetScenicScore(trees, new Vec2D(i, j)));
        }

        return maxScore;
    }
    
    private static int GetScenicScore(Grid2D<int> trees, Vec2D position)
    {
        return 
            GetViewingDistance(trees, position, Vec2D.Up) *
            GetViewingDistance(trees, position, Vec2D.Down) *
            GetViewingDistance(trees, position, Vec2D.Left) *
            GetViewingDistance(trees, position, Vec2D.Right);
    }

    private static int GetViewingDistance(Grid2D<int> trees, Vec2D position, Vec2D viewingVec)
    {
        var viewingDistance = 0;
        var viewingHeight = trees[position];
        var nextPosition = position + viewingVec;

        while (trees.Contains(nextPosition))
        {
            viewingDistance++;
            
            var viewedHeight = trees[nextPosition];
            if (viewedHeight >= viewingHeight)
            {
                break;
            }

            nextPosition += viewingVec;
        }

        return viewingDistance;
    }
}