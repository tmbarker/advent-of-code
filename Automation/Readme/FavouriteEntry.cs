using Problems.Attributes;

namespace Automation.Readme;

public readonly struct FavouriteEntry(string title, int year, int day, Topics topics, Difficulty difficulty)
{
    public string Title { get; } = title;
    public int Year { get; } = year;
    public int Day { get; } = day;
    public Topics Topics { get; } = topics;
    public Difficulty Difficulty { get; } = difficulty;
}