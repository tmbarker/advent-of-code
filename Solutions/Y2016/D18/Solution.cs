namespace Solutions.Y2016.D18;

[PuzzleInfo("Like a Rogue", Topics.StringParsing, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private delegate bool TrapPredicate(char left, char center, char right);
    
    private const char Safe = '.';
    private const char Trap = '^';

    private static readonly List<TrapPredicate> TrapPredicates =
    [
        (left, center, right) => left == Trap && center == Trap && right == Safe,
        (left, center, right) => left == Safe && center == Trap && right == Trap,
        (left, center, right) => left == Trap && center == Safe && right == Safe,
        (left, center, right) => left == Safe && center == Safe && right == Trap
    ];

    public override object Run(int part)
    {
        var seedRow = GetInputText();
        return part switch
        {
            1 => CountSafeTiles(seedRow, rows: 40),
            2 => CountSafeTiles(seedRow, rows: 400000),
            _ => ProblemNotSolvedString
        };
    }

    private static int CountSafeTiles(string seedRow, int rows)
    {
        var cols = seedRow.Length;
        var count = seedRow.Count(tile => tile == Safe);
        var currRow = new string(c: Safe, count: cols + 2).ToCharArray();
        var prevRow = new string(c: Safe, count: cols + 2).ToCharArray();

        for (var i = 0; i < cols; i++)
        {
            currRow[i + 1] = seedRow[i];
        }
        
        for (var y = 0; y < rows - 1; y++)
        {
            (prevRow, currRow) = (currRow, prevRow);
            
            for (var x = 0; x < cols; x++)
            {
                var left  = prevRow[1 + x - 1];
                var center= prevRow[1 + x + 0];
                var right = prevRow[1 + x + 1];

                if (TrapPredicates.Any(predicate => predicate(left, center, right)))
                {
                    currRow[1 + x] = Trap;
                }
                else
                {
                    currRow[1 + x] = Safe;
                    count++;
                }
            }
        }

        return count;
    }
}