namespace Problems.Y2020.D21;

public sealed class Food
{
    public Food(IEnumerable<string> ingredients, IEnumerable<string> listedAllergens)
    {
        Ingredients = new HashSet<string>(ingredients);
        ListedAllergens = new HashSet<string>(listedAllergens);
    } 
    
    public HashSet<string> Ingredients { get; }
    public HashSet<string> ListedAllergens { get; }
}