using Problems.Common;

namespace Problems.Y2022.D01;

/// <summary>
/// Calorie Counting: https://adventofcode.com/2022/day/1
/// </summary>
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => GetMaxCalories(num: 1),
            2 => GetMaxCalories(num: 3),
            _ => ProblemNotSolvedString
        };
    }

    private int GetMaxCalories(int num)
    {
        var lines = GetInputLines();
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
        
        //  Handle the case where the last line in the input isn't empty
        //
        calories.Add(currentCalories);

        return calories
            .OrderDescending()
            .Take(num)
            .Sum();
    }
}