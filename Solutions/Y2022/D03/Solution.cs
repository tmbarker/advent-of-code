namespace Solutions.Y2022.D03;

[PuzzleInfo("Rucksack Reorganization", Topics.None, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    private const int AlphabetLength = 26;
    private const int GroupSize = 3;

    public override object Run(int part)
    {
        return part switch
        {
            1 => GetDuplicateItemPrioritySum(),
            2 => GetBadgeItemPrioritySum(),
            _ => PuzzleNotSolvedString
        };
    }

    private int GetDuplicateItemPrioritySum()
    {
        var rucksacks = GetInputLines();
        var set = new HashSet<char>();
        var prioritySum = 0;

        foreach (var rucksack in rucksacks)
        {
            for (var i = 0; i < rucksack.Length; i++)
            {
                if (i < rucksack.Length / 2)
                {
                    set.Add(rucksack[i]);
                    continue;
                }

                if (set.Contains(rucksack[i]))
                {
                    set.Remove(rucksack[i]);
                    prioritySum += GetItemPriority(rucksack[i]);
                }
            }
            
            set.Clear();
        }

        return prioritySum;
    }

    private int GetBadgeItemPrioritySum()
    {
        var rucksacks = GetInputLines();
        var numGroups = rucksacks.Length / GroupSize;

        var badgeItemPrioritySum = 0;

        for (var i = 0; i < numGroups; i++)
        {
            var firstRucksackInGroup = rucksacks[i * GroupSize];
            var possibleBadgeItemSet = firstRucksackInGroup.Distinct();

            for (var k = 1; k < GroupSize; k++)
            {
                var groupMemberRucksack = rucksacks[i * GroupSize + k];
                possibleBadgeItemSet = possibleBadgeItemSet.Intersect(groupMemberRucksack);
            }

            badgeItemPrioritySum += GetItemPriority(possibleBadgeItemSet.Single());
        }

        return badgeItemPrioritySum;
    }
    
    private static int GetItemPriority(char item)
    {
        //  The uppercase letters proceed the lowercase letters in the ASCII table
        //
        return item < 'a' ? 
            item - 'A' + 1 + AlphabetLength : 
            item - 'a' + 1;
    }
}