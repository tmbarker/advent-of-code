using Utilities.Extensions;

namespace Solutions.Y2025.D01;

[PuzzleInfo("Secret Entrance", Topics.Math, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => CountZeroes(includePasses: false),
            2 => CountZeroes(includePasses: true),
            _ => PuzzleNotSolvedString
        };
    }

    private int CountZeroes(bool includePasses)
    {
        const int slots = 100;
        var position = 50;
        var count = 0;

        foreach (var line in GetInputLines())
        {
            var steps = int.Parse(line[1..]);
            var clockwise = line[0] == 'R';
            
            if (includePasses)
            {
                count += steps / slots;
                
                var partial = steps % slots;
                var passesZero = clockwise 
                    ? position + partial >= slots 
                    : position > 0 && position <= partial;

                if (passesZero)
                {
                    count++;
                }
            }
            
            position = clockwise 
                ? (position + steps).Modulo(slots) 
                : (position - steps).Modulo(slots);

            if (!includePasses && position == 0)
            {
                count++;
            }
        }
        
        return count;
    }
}