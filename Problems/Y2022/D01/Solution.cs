using Problems.Y2022.Common;

namespace Problems.Y2022.D01;

public class Solution : SolutionBase2022
{
    protected override int Day => 1;
    
    public override string Run(int part = 0)
    {
        AssertInputExists();
        
        return part switch
        {
            0 => SolvePart1().ToString(),
            1 => SolvePart2().ToString(),
            _ => ProblemNotSolvedString,
        };
    }

    private int SolvePart1()
    {
        var lines = File.ReadAllLines(GetInputFilePath());
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

    private int SolvePart2()
    {
        const int numElves = 3;
        
        var lines = File.ReadAllLines(GetInputFilePath());
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
            .Take(numElves)
            .Sum();
    }
}