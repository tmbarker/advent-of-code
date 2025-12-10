using Microsoft.Z3;

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
        var ctx = new Context();
        var opt = ctx.MkOptimize();
        var buttonVars = new ArithExpr[machine.Buttons.Count];
        
        for (var b = 0; b < machine.Buttons.Count; b++)
        {
            buttonVars[b] = ctx.MkIntConst($"button_{b}");
            opt.Add(ctx.MkGe(buttonVars[b], ctx.MkInt(0)));
        }
        
        for (var j = 0; j < machine.Joltage.Length; j++)
        {
            var terms = new List<ArithExpr>();
            for (var b = 0; b < machine.Buttons.Count; b++)
            {
                if (machine.Buttons[b].Contains(j))
                {
                    terms.Add(buttonVars[b]);
                }
            }

            var sumExpr = ctx.MkAdd(terms);
            var tarExpr = ctx.MkInt(machine.Joltage[j]);
            opt.Add(ctx.MkEq(sumExpr, tarExpr));
        }
        
        opt.MkMinimize(ctx.MkAdd(buttonVars));
        opt.Check();
        
        return Enumerable
            .Range(0, machine.Buttons.Count)
            .Select(i => ((IntNum)opt.Model.Evaluate(buttonVars[i])).Int)
            .Sum();
    }
}