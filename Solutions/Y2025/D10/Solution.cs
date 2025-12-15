namespace Solutions.Y2025.D10;

[PuzzleInfo("Factory", Topics.LinearProgramming, Difficulty.Hard)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var machines = ParseInputLines(Machine.Parse);
        return part switch
        {
            1 => machines.Sum(Part1),
            2 => machines.Sum(Part2),
            _ => PuzzleNotSolvedString
        };
    }
    
    private static int Part1(Machine machine)
    {
        var queue = new Queue<(int Mask, int Presses)>([(Mask: 0, Presses: 0)]);
        var visited = new HashSet<int>();
        
        while (queue.Count > 0)
        {
            var (lightMask, presses) = queue.Dequeue();
            if (lightMask == machine.LightMask)
            {
                return presses;
            }

            foreach (var buttonMask in machine.ButtonMasks)
            {
                var nextMask = lightMask ^ buttonMask;
                if (visited.Add(nextMask))
                {
                    queue.Enqueue((nextMask, presses + 1));
                }
            }
        }

        throw new NoSolutionException("Unreachable");
    }
    
    private static int Part2(Machine machine)
    {
        return SimplexBranchAndBound.Solve(machine);
    }
}