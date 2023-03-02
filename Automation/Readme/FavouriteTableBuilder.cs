using System.Reflection;
using System.Text.RegularExpressions;
using Problems.Attributes;

namespace Automation.Readme;

public static class FavouriteTableBuilder
{
    private const string SectionStart = "## My Favourite Puzzles and Solutions";
    private const string SectionEnd =   "## Running a Solution";

    private static readonly Regex ProblemRegex = new(@"Problems.Y(?<Year>\d{4}).D(?<Day>\d{2})");

    public static void Run(List<string> readmeLines)
    {
        var entries = ResolveEntries();
        var tables = GroupIntoTables(entries);
        var favouriteLines = new List<string> { string.Empty };

        foreach (var table in tables)
        {
            favouriteLines.Add(FavouriteTableFormatter.FormTitle(table.Year));
            favouriteLines.Add(FavouriteTableFormatter.ColumnNames);
            favouriteLines.Add(FavouriteTableFormatter.ColumnAlignments);
            favouriteLines.AddRange(table.Entries.Select(FavouriteTableFormatter.FormEntry));
            favouriteLines.Add(string.Empty);
        }
        
        var favouritesStartIndex = readmeLines.IndexOf(SectionStart) + 1;
        var favouritesEndIndex = readmeLines.IndexOf(SectionEnd) - 1;

        readmeLines.RemoveRange(favouritesStartIndex, favouritesEndIndex - favouritesStartIndex + 1);
        readmeLines.InsertRange(favouritesStartIndex, favouriteLines);
    }

    private static IEnumerable<FavouriteTable> GroupIntoTables(IEnumerable<FavouriteEntry> entries)
    {
        var grouped = entries
            .GroupBy(f => f.Year)
            .OrderByDescending(g => g.Key);
        
        foreach (var group in grouped)
        {
            var year = group.Key;
            var tableEntries = group.OrderBy(f => f.Day);

            yield return new FavouriteTable(
                year: year,
                entries: tableEntries);
        }
    }

    private static IEnumerable<FavouriteEntry> ResolveEntries()
    {
        var favouriteAttributeType = typeof(FavouriteAttribute);
        var problemsAssembly = favouriteAttributeType.Assembly;
        var favourites = FindDecoratedTypes(
            assembly: problemsAssembly, 
            attributeType: favouriteAttributeType);

        foreach (var favourite in favourites)
        {
            var attributeInstance = (FavouriteAttribute)favourite.GetCustomAttribute(favouriteAttributeType)!;
            var match = ProblemRegex.Match(favourite.FullName!);
            var year = int.Parse(match.Groups["Year"].Value);
            var day = int.Parse(match.Groups["Day"].Value);

            yield return new FavouriteEntry(
                title: attributeInstance.Title,
                year: year,
                day: day,
                topics: attributeInstance.Topics,
                difficulty: attributeInstance.Difficulty);
        }
    }

    private static IEnumerable<Type> FindDecoratedTypes(Assembly assembly, Type attributeType)
    {
        return assembly
            .GetTypes()
            .Where(t => t.IsDefined(attributeType));
    }
}