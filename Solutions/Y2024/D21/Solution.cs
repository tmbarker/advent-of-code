using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2024.D21;

[PuzzleInfo("Keypad Conundrum", Topics.Recursion, Difficulty.Hard, favourite: true)]
public sealed class Solution : SolutionBase
{
    private static readonly Pad NumPad = Pad.Parse(flat: "!0A123456789", cols: 3, omit: '!');
    private static readonly Pad DirPad = Pad.Parse(flat: "<v>!^A",       cols: 3, omit: '!');
    private static readonly Dictionary<(Vec2D, Vec2D, int), long> DirMemo = new();
    
    private readonly record struct State(Vec2D Pos, string Sequence)
    {
        public State Step(Vec2D dir, char key)
        {
            return new State(Pos: Pos + dir, Sequence: Sequence + key);
        }
    }
    
    public override object Run(int part)
    {
        var pads = part == 1 ? 2 : 25;
        var seqs = GetInputLines();
        return seqs.Sum(seq => GetComplexity(seq, pads));
    }
    
    private static long GetComplexity(string sequence, int numDirPads)
    {
        var total = 0L;
        var start = NumPad.KeyMap['A'];
        
        foreach (var end in sequence.Select(key => NumPad.KeyMap[key]))
        {
            total += GetSequenceCost(start, end, pad: NumPad, robot: numDirPads);
            start = end;
        }

        return total * sequence.ParseLong();
    }
    
    private static long GetSequenceCost(Vec2D start, Vec2D end, Pad pad, int robot)
    {
        if (pad == DirPad && DirMemo.TryGetValue((start, end, robot), out var cached))
        {
            return cached;
        }

        var result = long.MaxValue;
        var queue = new Queue<State>([new State(Pos: start, Sequence: "")]);

        while (queue.Count != 0)
        {
            var state = queue.Dequeue();
            if (!pad.PosMap.ContainsKey(state.Pos))
            {
                continue;
            }
            
            if (state.Pos == end)
            {
                var target = pad == NumPad
                    ? robot
                    : robot - 1;
                
                result = long.Min(result, GetRobotCost($"{state.Sequence}A", robot: target));
                continue;
            }

            if (state.Pos.Y < end.Y) queue.Enqueue(state.Step(dir: Vec2D.Up,    key: '^'));
            if (state.Pos.Y > end.Y) queue.Enqueue(state.Step(dir: Vec2D.Down,  key: 'v'));
            if (state.Pos.X < end.X) queue.Enqueue(state.Step(dir: Vec2D.Right, key: '>'));
            if (state.Pos.X > end.X) queue.Enqueue(state.Step(dir: Vec2D.Left,  key: '<'));
        }

        if (pad == DirPad)
        {
            DirMemo[(start, end, robot)] = result;
        }
        
        return result;
    }
    
    private static long GetRobotCost(string sequence, int robot)
    {
        if (robot == 0)
        {
            return sequence.Length;
        }
        
        var total = 0L;
        var start = DirPad.KeyMap['A'];

        foreach (var end in sequence.Select(key => DirPad.KeyMap[key]))
        {
            total += GetSequenceCost(start, end, pad: DirPad, robot);
            start = end;
        }
        
        return total;
    }
}