using Utilities.Geometry.Euclidean;

namespace Solutions.Y2024.D15;

[PuzzleInfo("Warehouse Woes", Topics.Vectors, Difficulty.Medium)]
public class Solution : SolutionBase
{
    private static readonly Dictionary<char, Vec2D> MoveDirs = new()
    {
        ['^'] = Vec2D.Up,
        ['v'] = Vec2D.Down,
        ['<'] = Vec2D.Left,
        ['>'] = Vec2D.Right
    };
    private static readonly Dictionary<char, Vec2D> BoxAdj = new()
    {
        ['['] = Vec2D.Right,
        [']'] = Vec2D.Left,
    };
    
    public override object Run(int part)
    {
        return part switch
        {
            1 => Simulate(wide: false),
            2 => Simulate(wide: true),
            _ => PuzzleNotSolvedString
        };
    }
    
    private int Simulate(bool wide)
    {
        var input = ChunkInputByNonEmpty();
        var grid = wide ? ParseGrid(input[0]) : Grid2D<char>.MapChars(input[0]); 
        var robot = grid.Single(pos => grid[pos] == '@');

        foreach (var move in string.Concat(input[1]).Select(c => MoveDirs[c]))
        {
            var queue = new Queue<Vec2D>([robot]);
            var shift = new Dictionary<Vec2D, char> { [robot] = grid[robot] };
            var valid = true;

            while (queue.Count != 0)
            {
                var pos = queue.Dequeue() + move;
                var chr = grid[pos];

                switch (chr)
                {
                    case '#':
                        valid = false;
                        break;
                    case 'O':
                        queue.Enqueue(pos);
                        shift[pos] = chr;
                        continue;
                    case '[':
                    case ']':
                        var adj = pos + BoxAdj[chr];
                        if (shift.TryAdd(pos, chr))       queue.Enqueue(pos);
                        if (shift.TryAdd(adj, grid[adj])) queue.Enqueue(adj);
                        continue;
                }
            }

            if (valid)
            {
                foreach (var pos in shift.Keys) grid[pos] = '.';
                foreach (var (pos, chr) in shift) grid[pos + move] = chr;
                robot += move;
            }
        }

        return grid
            .Where(pos => grid[pos] is 'O' or '[')
            .Sum(pos => pos.X + 100 * (grid.Height - pos.Y - 1));
    }
    
    private static Grid2D<char> ParseGrid(string[] lines)
    {
        var height = lines.Length;
        var width =  lines[0].Length;
        var grid = Grid2D<char>.WithDimensions(rows: height, cols: 2 * width);
        
        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
        {
            (grid[2 * x, y], grid[2 * x + 1, y]) = lines[height - 1 - y][x] switch
            {
                '#' => ('#', '#'),
                'O' => ('[', ']'),
                '.' => ('.', '.'),
                '@' => ('@', '.'),
                _ => throw new NoSolutionException("Invalid input")
            };
        }
        
        return grid;
    }
}