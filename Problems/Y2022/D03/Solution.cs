using Problems.Y2022.Common;

namespace Problems.Y2022.D03;

public class Solution : SolutionBase2022
{
    private const int AlphabetLength = 26;
    
    public override int Day => 3;
    public override int Parts => 1;
    
    public override string Run(int part)
    {
        AssertInputExists();

        return part switch
        {
            0 => SolvePart1().ToString(),
            _ => ProblemNotSolvedString,
        };
    }

    private int SolvePart1()
    {
        var lines = File.ReadAllLines(GetInputFilePath());
        var set = new HashSet<char>();
        
        var sum = 0;

        foreach (var line in lines)
        {
            var totalNumItems = line.Length;
            var numItemsPerCompartment = totalNumItems / 2;
            
            for (var i = 0; i < totalNumItems; i++)
            {
                if (i < numItemsPerCompartment)
                {
                    if (!set.Contains(line[i]))
                    {
                        set.Add(line[i]);   
                    }
                    continue;
                }

                if (!set.Contains(line[i]))
                {
                    continue;
                }
                
                set.Remove(line[i]);
                sum += GetPriority(line[i]);
            }
            
            set.Clear();
        }

        return sum;
    }

    private static int GetPriority(char item)
    {
        // The uppercase letters proceed the lowercase letters in the ASCII table
        return item < 'a' ? 
            item - 'A' + 1 + AlphabetLength : 
            item - 'a' + 1;
    }
}