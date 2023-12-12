namespace Automation.Readme;

public static class ReadmeUtils
{
    private const string ReadmeSearchPattern = "*.md";
    private const string ReadmeFilename = "README.md";

    public static void UpdateReadme()
    {
        UpdateFavouritePuzzlesSection();
    }
    
    private static void UpdateFavouritePuzzlesSection()
    {
        if (!ResolveReadmeFilepath(out var readmeFilepath))
        {
            Log($"Cannot find {ReadmeFilename}", ConsoleColor.Red);
            return;
        }

        var readmeLines = File
            .ReadAllLines(readmeFilepath)
            .ToList();
        
        FavouriteTableBuilder.Run(readmeLines);
        File.WriteAllLines(readmeFilepath, readmeLines);

        Log($"{ReadmeFilename} updated", ConsoleColor.Green);
    }
    
    private static bool ResolveReadmeFilepath(out string filepath)
    {
        var directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
        while (directory != null)
        {
            var candidates = directory
                .GetFiles(ReadmeSearchPattern)
                .Select(f => f.FullName)
                .ToList();

            if (candidates.Any())
            {
                filepath = candidates.First();
                return true;
            }
            
            directory = directory.Parent;
        }

        filepath = string.Empty;
        return false;
    }
    
    private static void Log(string log, ConsoleColor consoleColor)
    {
        Console.ForegroundColor = consoleColor;
        Console.WriteLine(log);
        Console.ResetColor();
    }
}