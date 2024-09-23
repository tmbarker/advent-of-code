using Solutions.Attributes;

namespace Automation.Readme;

public sealed class FavouriteTable(int year, IEnumerable<FavouriteTable.Entry> entries)
{
    public readonly record struct Entry(string Title, int Year, int Day, Topics Topics, Difficulty Difficulty);
    
    public int Year { get; } = year;
    public IReadOnlyList<Entry> Entries { get; } = new List<Entry>(entries);
}