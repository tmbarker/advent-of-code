using Utilities.Collections;
using Utilities.Extensions;

namespace Solutions.Y2023.D15;

[PuzzleInfo("Lens Library", Topics.None, Difficulty.Easy)] 
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputText();
        var instr = input.Split(separator: ',');
        var boxes = new DefaultDict<int, Box>(defaultSelector: n => new Box(n));
        
        return part switch
        {
            1 => instr.Sum(Hash),
            2 => Install(instr, boxes),
            _ => ProblemNotSolvedString
        };
    }
    
    private static int Hash(string s)
    {
        return s.Aggregate(seed: 0, func: (val, c) => (val + c) * 17 % 256);
    }
    
    private static int Install(IEnumerable<string> instructions, IDictionary<int, Box> boxes)
    {
        foreach (var instr in instructions)
        {
            var label = string.Concat(instr.TakeWhile(char.IsLetter));
            var box = boxes[Hash(label)];

            switch (instr[label.Length])
            {
                case '-':
                    box.Remove(label);
                    break;
                case '=':
                    box.Add(label, focalLength: instr.ParseInt());
                    break;
            }
        }

        return boxes.Values.Sum(box => box.GetPower());
    }
}