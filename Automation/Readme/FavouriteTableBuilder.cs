using System.Reflection;
using System.Text.RegularExpressions;
using Solutions.Attributes;
using Utilities.Extensions;

namespace Automation.Readme;

public static class FavouriteTableBuilder
{
    private const string SectionStart = "## My Favourite Puzzles and Solutions";
    private const string SectionEnd =   "## Running a Solution";

    private static readonly Regex SolutionRegex = new(@"Solutions.Y(?<Year>\d{4}).D(?<Day>\d{2})");

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

        //  Look for the correct place to stitch the favourite tables into, if it's found, replace those lines.
        //
        if (readmeLines.Contains(item: SectionStart) && readmeLines.Contains(SectionEnd))
        {
            var favouritesStartIndex = readmeLines.IndexOf(SectionStart) + 1;
            var favouritesEndIndex = readmeLines.IndexOf(SectionEnd) - 1;

            readmeLines.RemoveRange(favouritesStartIndex, favouritesEndIndex - favouritesStartIndex + 1);
            readmeLines.InsertRange(favouritesStartIndex, favouriteLines);
            return;
        }
        
        //  If we can't find an existing table, append to the bottom
        //
        readmeLines.AddRange(favouriteLines);
    }

    private static IEnumerable<FavouriteTable> GroupIntoTables(IEnumerable<FavouriteTable.Entry> entries)
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

    private static IEnumerable<FavouriteTable.Entry> ResolveEntries()
    {
        var puzzleAttributeType = typeof(PuzzleInfoAttribute);
        var puzzlesAssembly = puzzleAttributeType.Assembly;
        var puzzles = FindDecoratedTypes(
            assembly: puzzlesAssembly, 
            attributeType: puzzleAttributeType);

        foreach (var puzzle in puzzles)
        {
            var attributeInstance = (PuzzleInfoAttribute)puzzle.GetCustomAttribute(puzzleAttributeType)!;
            if (!attributeInstance.Favourite)
            {
                continue;
            }
            
            var match = SolutionRegex.Match(puzzle.FullName!);
            var year = match.Groups["Year"].ParseInt();
            var day = match.Groups["Day"].ParseInt();

            yield return new FavouriteTable.Entry(
                Title: attributeInstance.Title,
                Year: year,
                Day: day,
                Topics: attributeInstance.Topics,
                Difficulty: attributeInstance.Difficulty);
        }
    }

    private static IEnumerable<Type> FindDecoratedTypes(Assembly assembly, Type attributeType)
    {
        return assembly
            .GetTypes()
            .Where(t => t.IsDefined(attributeType));
    }
}