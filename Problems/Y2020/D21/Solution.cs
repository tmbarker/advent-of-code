using System.Text.RegularExpressions;
using Problems.Common;
using Utilities.Extensions;

namespace Problems.Y2020.D21;

/// <summary>
/// Allergen Assessment: https://adventofcode.com/2020/day/21
/// </summary>
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var foods = ParseFoods(GetInputLines());
        var allergens = ResolveAllergens(foods);
        
        return part switch
        {
            1 => CountAllergenFreeIngredients(allergens, foods),
            2 => FormAllergensList(allergens),
            _ => ProblemNotSolvedString
        };
    }

    private static Dictionary<string, string> ResolveAllergens(IList<Food> foods)
    {
        var allergenCandidates = new Dictionary<string, ISet<string>>();
        var allergensMap = new Dictionary<string, string>();
        var allergensSet = foods
            .SelectMany(f => f.ListedAllergens)
            .ToHashSet();
        
        foreach (var allergen in allergensSet)
        {
            var possibleIngredients = foods
                .Where(food => food.ListedAllergens.Contains(allergen))
                .Select(food => food.Ingredients)
                .IntersectAll();

            allergenCandidates.Add(allergen, possibleIngredients);
        }
        
        while (allergensMap.Count < allergensSet.Count)
        {
            foreach (var (allergen, candidates) in allergenCandidates.Freeze())
            {
                if (candidates.Count != 1)
                {
                    continue;
                }
                
                allergensMap.Add(allergen, candidates.Single());
                allergenCandidates.Remove(allergen);
                
                foreach (var (_, candidatesSet) in allergenCandidates)
                {
                    candidatesSet.Remove(allergensMap[allergen]);
                }
            }
        }

        return allergensMap;
    }

    private static int CountAllergenFreeIngredients(Dictionary<string, string> allergensMap, IEnumerable<Food> foods)
    {
        return foods
            .SelectMany(food => food.Ingredients)
            .Count(ingredient => !allergensMap.ContainsValue(ingredient));
    }

    private static string FormAllergensList(Dictionary<string, string> allergensMap)
    {
        return string.Join(',', allergensMap.Keys
            .Order()
            .Select(allergen => allergensMap[allergen]));
    }
    
    private static IList<Food> ParseFoods(IEnumerable<string> input)
    {
        var regex = new Regex(@"(?:([a-z]+)+\s)+\(contains(?: ([a-z]+)(?:,|\)))+");
        return input
            .Select(line => regex.Matches(line)[0])
            .Select(match => new Food(
                ingredients:     match.Groups[1].Captures.Select(c => c.Value), 
                listedAllergens: match.Groups[2].Captures.Select(c => c.Value)))
            .ToList();
    }
}