using Utilities.Cartesian;

namespace Problems.Y2021.D04;

public class BingoCard
{
    private readonly Grid2D<int> _squares;
    private readonly Dictionary<int, Vector2D> _numberMap = new();
    private readonly HashSet<int> _markedNumbers = new();

    public bool HasWon { get; private set; }
    
    public BingoCard(Grid2D<int> squares)
    {
        _squares = squares;

        for (var x = 0; x < squares.Width; x++)
        for (var y = 0; y < squares.Height; y++)
        {
            _numberMap[_squares[x, y]] = new Vector2D(x, y);
        }
    }

    public bool Evaluate(int number, out int score)
    {
        score = 0;
        if (_markedNumbers.Contains(number) || !_numberMap.ContainsKey(number))
        {
            return false;
        }

        _markedNumbers.Add(number);
        var square = _numberMap[number];

        if (!CheckRow(square.Y) && !CheckColumn(square.X))
        {
            return false;
        }
        
        score = number * SumUnmarked();
        HasWon = true;
        return true;

    }

    private bool CheckColumn(int col)
    {
        for (var y = 0; y < _squares.Height; y++)
        {
            if (!_markedNumbers.Contains(_squares[col, y]))
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckRow(int row)
    {
        for (var x = 0; x < _squares.Width; x++)
        {
            if (!_markedNumbers.Contains(_squares[x, row]))
            {
                return false;
            }
        }

        return true;
    }

    private int SumUnmarked()
    {
        var score = 0;
        
        for (var x = 0; x < _squares.Width; x++)
        for (var y = 0; y < _squares.Height; y++)
        {
            var number = _squares[x, y];
            if (!_markedNumbers.Contains(number))
            {
                score += number;
            }
        }

        return score;
    }
}