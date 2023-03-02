namespace Automation.Readme;

public class FavouriteTable
{
    public int Year { get; }
    public readonly IReadOnlyList<FavouriteEntry> Entries;

    public FavouriteTable(int year, IEnumerable<FavouriteEntry> entries)
    {
        Year = year;
        Entries = new List<FavouriteEntry>(entries);
    }
}