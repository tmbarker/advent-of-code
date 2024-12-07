using System.Runtime.CompilerServices;
using Utilities.Extensions;

namespace Solutions.Y2024.D07;

[PuzzleInfo("Bridge Repair", Topics.Math, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    private readonly record struct State(long Val, int Idx)
    {
        public State Advance(long val) => new(Val: val, Idx: Idx + 1);
    }
    
    public override object Run(int part)
    {
        return part switch
        {
            1 => Calibrate(concat: false),
            2 => Calibrate(concat: true),
            _ => PuzzleNotSolvedString
        };
    }
    
    private long Calibrate(bool concat)
    {
        var total = 0L;
        var queue = new Queue<State>(capacity: 32);
        
        foreach (var eq in  ParseInputLines(parseFunc: line => line.ParseLongs()))
        {
            queue.Clear();
            queue.Enqueue(new State(Val: eq[1], Idx: 2));
            
            while (queue.Count != 0)
            {
                var state = queue.Dequeue();
                if (state.Idx >= eq.Length)
                {
                    if (state.Val == eq[0])
                    {
                        total += eq[0];
                        break;   
                    }
                    
                    continue;
                }

                if (state.Val > eq[0])
                {
                    continue;
                }
                
                queue.Enqueue(state.Advance(state.Val + eq[state.Idx]));
                queue.Enqueue(state.Advance(state.Val * eq[state.Idx]));

                if (concat)
                {
                    queue.Enqueue(state.Advance(Concat(state.Val, eq[state.Idx])));
                }
            }
        }

        return total;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static long Concat(long a, long b)
    {
        var d = (int)Math.Log10(b) + 1;
        return a * (long)Math.Pow(10, d) + b;
    }
}