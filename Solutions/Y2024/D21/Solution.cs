using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2024.D21;

[PuzzleInfo("Keypad Conundrum", Topics.Recursion, Difficulty.Hard, favourite: true)]
public sealed class Solution : SolutionBase
{
    private static readonly Pad Numpad = Pad.Parse(flat: "*0A123456789", cols: 3, skip: '*');
    private static readonly Pad Dirpad = Pad.Parse(flat: "<v>*^A",       cols: 3, skip: '*');
    private static readonly Dictionary<(Vec2D, Vec2D, int), long> Memo = new();
    
    private readonly record struct State(Vec2D Pos, string Sequence)
    {
        public State Up()    => new(Pos + Vec2D.Up,    $"{Sequence}^");
        public State Down()  => new(Pos + Vec2D.Down,  $"{Sequence}v");
        public State Left()  => new(Pos + Vec2D.Left,  $"{Sequence}<");
        public State Right() => new(Pos + Vec2D.Right, $"{Sequence}>");
    }
    
    public override object Run(int part)
    {
        return ParseInputLines(code => GetComplexity(code, numDirPads: part == 1 ? 2 : 25)).Sum();
    }
    
    private static long GetComplexity(string sequence, int numDirPads)
    {
        var total = 0L;
        var start = Numpad.KeyMap['A'];
        
        foreach (var end in sequence.Select(key => Numpad.KeyMap[key]))
        {
            total += GetSequenceCost(start, end, pad: Numpad, robot: numDirPads);
            start = end;
        }

        return total * sequence.ParseLong();
    }
    
    private static long GetSequenceCost(Vec2D start, Vec2D end, Pad pad, int robot)
    {
        if (pad == Dirpad && Memo.TryGetValue((start, end, robot), out var cached))
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
                var remote = pad == Dirpad;
                var target = remote ? robot - 1 : robot;
                
                result = long.Min(result, CalculateRobotCost($"{state.Sequence}A", robot: target));
                continue;
            }

            if (state.Pos.Y < end.Y) queue.Enqueue(state.Up());
            if (state.Pos.Y > end.Y) queue.Enqueue(state.Down());
            if (state.Pos.X < end.X) queue.Enqueue(state.Right());
            if (state.Pos.X > end.X) queue.Enqueue(state.Left());
        }

        if (pad == Dirpad) Memo[(start, end, robot)] = result;
        return result;
    }
    
    private static long CalculateRobotCost(string sequence, int robot)
    {
        if (robot == 0)
        {
            return sequence.Length;
        }
        
        var total = 0L;
        var start = Dirpad.KeyMap['A'];

        foreach (var end in sequence.Select(key => Dirpad.KeyMap[key]))
        {
            total += GetSequenceCost(start, end, pad: Dirpad, robot);
            start = end;
        }
        
        return total;
    }
}