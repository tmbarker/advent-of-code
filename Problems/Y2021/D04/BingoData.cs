using Utilities.Cartesian;

namespace Problems.Y2021.D04;

public static class BingoData
{
    private const char DrawDelimiter = ',';
    private const char NumberDelimiter = ' ';
    
    public static void Parse(IList<string> lines, out Queue<int>? draw, out IList<BingoCard>? cards)
    {
        draw = new Queue<int>();
        cards = new List<BingoCard>();
        
        foreach (var number in lines[0].Split(DrawDelimiter))
        {
            draw.Enqueue(int.Parse(number));
        }

        var cardLines = new List<string>();
        for (var i = 2; i < lines.Count; i++)
        {
            if (!string.IsNullOrWhiteSpace(lines[i]))
            {
                cardLines.Add(lines[i]);
                continue;
            }
            
            cards.Add(ParseBingoCard(cardLines));
            cardLines.Clear();
        }
        
        // Handle the case where the last line is part of a card
        cards.Add(ParseBingoCard(cardLines));
    }
    
    private static BingoCard ParseBingoCard(IEnumerable<string> lines)
    {
        var numbers = lines
            .Select(ParseBingoCardRow)
            .ToList();

        var rows = numbers.Count;
        var cols = numbers[0].Count;
        var grid = Grid2D<int>.WithDimensions(rows, cols);

        for (var row = 0; row < rows; row++)
        for (var col = 0; col < cols; col++)
        {
            grid[col, rows - row - 1] = numbers[row][col];
        }
        
        return new BingoCard(grid);
    }

    private static IList<int> ParseBingoCardRow(string line)
    {
        return line
            .Split(NumberDelimiter, StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToList();
    }
}