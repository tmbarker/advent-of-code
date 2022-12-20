using Problems.Y2022.Common;

namespace Problems.Y2022.D01;

/// <summary>
/// Calorie Counting: https://adventofcode.com/2022/day/1
/// </summary>
public class Solution : SolutionBase2022
{
    private const int NumTopElvesToSum = 3;
    
    public override int Day => 1;

    public override object Run(int part)
    {
        return part switch
        {
            0 => GetMaxCalories(),
            1 => GetMaxCalories(NumTopElvesToSum),
            _ => ProblemNotSolvedString,
        };
    }

    private int GetMaxCalories()
    {
        var lines = GetInput();
        var maxCalories = 0;
        var currentCalories = 0;

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                maxCalories = Math.Max(currentCalories, maxCalories);
                currentCalories = 0;
                continue;
            }

            currentCalories += int.Parse(line);
        }

        // Handle the case where the last line in the input isn't empty
        return Math.Max(maxCalories, currentCalories);
    }

    private int GetMaxCalories(int num)
    {
        var lines = GetInput();
        var calories = new List<int>();
        var currentCalories = 0;
        
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                calories.Add(currentCalories);
                currentCalories = 0;
                continue;
            }

            currentCalories += int.Parse(line);
        }
        
        // Handle the case where the last line in the input isn't empty
        calories.Add(currentCalories);

        return calories
            .OrderByDescending(c => c)
            .Take(num)
            .Sum();
    }
}