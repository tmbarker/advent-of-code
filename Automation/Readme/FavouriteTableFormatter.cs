using System.Text.RegularExpressions;
using Problems.Attributes;

namespace Automation.Readme;

public static class FavouriteTableFormatter
{
    public const string ColumnNames = "Puzzle | My Solution | Date | Topic(s) | Difficulty";
    public const string ColumnAlignments = ":--- | :---: | :---: | :--- | :---";

    private static readonly Regex CamelCaseRegex = new(@"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))");

    public static string FormTitle(int year)
    {
        return $"### {year}";
    }
    
    public static string FormEntry(FavouriteEntry entry)
    {
        return string.Join(" | ",
            FormPuzzleLink(entry.Title, entry.Year, entry.Day),
            FormSolutionLink(entry.Year, entry.Day),
            FormDateString(entry.Year, entry.Day),
            FormTopics(entry.Topics),
            FormDifficulty(entry.Difficulty));
    }

    private static string FormPuzzleLink(string title, int year, int day)
    {
        return $"[{title}](https://adventofcode.com/{year}/day/{day})";
    }

    private static string FormSolutionLink(int year, int day)
    {
        return
            $"[Solution](https://github.com/tmbarker/advent-of-code/blob/main/Problems/Y{year}/D{day:D2}/Solution.cs)";
    }

    private static string FormDateString(int year, int day)
    {
        return $"{year}-{day:D2}";
    }

    private static string FormTopics(Topics topics)
    {
        var topicStrings = 
            from topic in Enum.GetValues<Topics>() 
            where (topics & topic) > 0
            select CamelCaseRegex.Replace(topic.ToString(), " $1");

        return string.Join(", ", topicStrings);
    }

    private static string FormDifficulty(Difficulty difficulty)
    {
        return difficulty switch
        {
            Difficulty.Easy => $":green_circle: {difficulty}",
            Difficulty.Medium => $":yellow_circle: {difficulty}",
            Difficulty.Hard => $":red_circle: {difficulty}",
            _ => difficulty.ToString()
        };
    }
}