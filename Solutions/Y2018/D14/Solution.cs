using Utilities.Extensions;

namespace Solutions.Y2018.D14;

[PuzzleInfo("Chocolate Charts", Topics.StringParsing, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private static readonly List<int> InitialRecipes = [3, 7];
    private static readonly List<int> InitialElves = [0, 1];

    public override object Run(int part)
    {
        var input = GetInputText();
        var number = int.Parse(input);
        
        return part switch
        {
            1 => GetScoreSequence(startAt: number, numScores: 10),
            2 => GetCountBeforeSequence(sequenceStr: input),
            _ => PuzzleNotSolvedString
        };
    }

    private static string GetScoreSequence(int startAt, int numScores)
    {
        var recipes = new List<int>(InitialRecipes);
        var elves = new List<int>(InitialElves);

        while (recipes.Count < startAt + numScores)
        {
            TickRecipes(recipes, elves);
        }

        return string.Join(string.Empty, recipes.Skip(startAt).Take(numScores));
    }

    private static int GetCountBeforeSequence(string sequenceStr)
    {
        var recipes = new List<int>(InitialRecipes);
        var elves = new List<int>(InitialElves);

        var sequence = new List<int>(sequenceStr.Select(StringExtensions.AsDigit));
        var matchHead = 0;

        while (true)
        {
            TickRecipes(recipes, elves);
            
            while (recipes.Count - matchHead >= sequence.Count)
            {
                if (TryMatchSequence(recipes, sequence, matchHead))
                {
                    return matchHead;
                }

                matchHead++;
            }
        }
    }

    private static void TickRecipes(List<int> recipes, List<int> elves)
    {
        var sum = elves.Sum(e => recipes[e]);
        var newRecipes = GetDigits(sum);
            
        recipes.AddRange(newRecipes);

        for (var i = 0; i < elves.Count; i++)
        {
            var steps = 1 + recipes[elves[i]];
            var result = (elves[i] + steps) % recipes.Count;
                
            elves[i] = result;
        }
    }
    
    private static bool TryMatchSequence(List<int> recipes, IEnumerable<int> sequence, int start)
    {
        return !sequence.Where((t, i) => recipes[start + i] != t).Any();
    }
    
    private static IEnumerable<int> GetDigits(int number)
    {
        var stack = new Stack<int>();
        do
        {
            stack.Push(number % 10);
            number /= 10;
        } 
        while (number > 0);

        while (stack.Count > 0)
        {
            yield return stack.Pop();
        }
    }
}