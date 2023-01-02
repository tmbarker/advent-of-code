using Problems.Y2022.Common;
using Utilities.DataStructures.Cartesian;
using Utilities.Extensions;

namespace Problems.Y2022.D08;

/// <summary>
/// Treetop Tree House: https://adventofcode.com/2022/day/8
/// </summary>
public class Solution : SolutionBase2022
{
    private const int MinHeight = 0;
    private const int MaxHeight = 9;
    
    public override int Day => 8;
    
    public override object Run(int part)
    {
        var trees = ParseTrees(GetInputLines());
        
        return part switch
        {
            0 => CountVisibleTrees(trees),
            1 => GetMaxScenicScore(trees),
            _ => ProblemNotSolvedString,
        };
    }

    private static int CountVisibleTrees(Grid2D<int> trees)
    {
        var visibleSet = new HashSet<Vector2D>();
        var linesOfSight = GetLinesOfSight(trees.Height, trees.Width);

        foreach (var lineOfSight in linesOfSight)
        {
            EvaluateLineOfSight(trees, lineOfSight, visibleSet);
        }

        return visibleSet.Count;
    }

    private static List<IEnumerable<Vector2D>> GetLinesOfSight(int rows, int cols)
    {
        var los = new List<IEnumerable<Vector2D>>();
        
        // Horizontal
        for (var y = 0; y < rows; y++)
        {
            var left = new List<Vector2D>();
            var right = new List<Vector2D>();
            
            for (var x = 0; x < cols; x++)
            {
                left.Add(new Vector2D(x, y));
                right.Add(new Vector2D(cols - x - 1, y));
            }
            
            los.Add(left);
            los.Add(right);
        }
        
        // Vertical
        for (var x = 0; x < cols; x++)
        {
            var up = new List<Vector2D>();
            var down = new List<Vector2D>();
            
            for (var y = 0; y < rows; y++)
            {
                up.Add(new Vector2D(x, y));
                down.Add(new Vector2D(x, rows - y - 1));
            }
            
            los.Add(up);
            los.Add(down);
        }

        return los;
    }
    
    private static void EvaluateLineOfSight(Grid2D<int> trees, IEnumerable<Vector2D> positions, ISet<Vector2D> visible)
    {
        var max = MinHeight - 1;
        foreach (var position in positions)
        {
            var height = trees[position];
            if (height > max)
            {
                max = height;
                visible.EnsureContains(position);
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

        // NOTE: The scenic score of edge trees will always be 0 and do not need to be evaluated
        for (var j = 1; j < trees.Height - 1; j++)
        for (var i = 1; i < trees.Width - 1; i++)
        {
            maxScore = Math.Max(maxScore, GetScenicScore(trees, new Vector2D(i, j)));
        }

        return maxScore;
    }
    
    private static int GetScenicScore(Grid2D<int> trees, Vector2D position)
    {
        return GetViewingDistance(trees, position, Vector2D.Up) *
               GetViewingDistance(trees, position, Vector2D.Down) *
               GetViewingDistance(trees, position, Vector2D.Left) *
               GetViewingDistance(trees, position, Vector2D.Right);
    }

    private static int GetViewingDistance(Grid2D<int> trees, Vector2D position, Vector2D viewingVector)
    {
        var viewingDistance = 0;
        var viewingHeight = trees[position];
        var nextPosition = position + viewingVector;

        while (trees.IsInDomain(nextPosition))
        {
            viewingDistance++;
            
            var viewedHeight = trees[nextPosition];
            if (viewedHeight >= viewingHeight)
            {
                break;
            }

            nextPosition += viewingVector;
        }

        return viewingDistance;
    }
    
    private static Grid2D<int> ParseTrees(IEnumerable<string> data)
    {
        var enumeratedData = data.ToList();
        var rows = enumeratedData.Count;
        var cols = enumeratedData[0].Length;

        var trees = Grid2D<int>.WithDimensions(rows, cols);

        for (var y = 0; y < rows; y++)
        for (var x = 0; x < cols; x++)
        {
            trees[x, rows - y - 1] = enumeratedData[y][x] - '0';
        }

        return trees;
    }
}