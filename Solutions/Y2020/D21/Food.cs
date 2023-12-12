namespace Solutions.Y2020.D21;

public sealed class Food(IEnumerable<string> ingredients, IEnumerable<string> listedAllergens)
{
    public HashSet<string> Ingredients { get; } = [..ingredients];
    public HashSet<string> ListedAllergens { get; } = [..listedAllergens];
}