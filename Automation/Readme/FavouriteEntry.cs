using Problems.Attributes;

namespace Automation.Readme;

public readonly struct FavouriteEntry
{
    public string Title { get; }
    public int Year { get; }
    public int Day { get; }
    public Topics Topics { get; }
    public Difficulty Difficulty { get; }

    public FavouriteEntry(string title, int year, int day, Topics topics, Difficulty difficulty)
    {
        Title = title;
        Year = year;
        Day = day;
        Topics = topics;
        Difficulty = difficulty;
    }
}