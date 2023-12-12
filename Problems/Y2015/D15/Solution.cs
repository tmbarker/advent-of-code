using Utilities.Extensions;

namespace Problems.Y2015.D15;

[PuzzleInfo("Science for Hungry People", Topics.Math, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    private readonly record struct Properties(int Cap, int Dur, int Fla, int Tex, int Cal);
    
    public override object Run(int part)
    {
        return part switch
        {
            1 => Optimize(total: 100, calorieReq: null),
            2 => Optimize(total: 100, calorieReq: 500),
            _ => ProblemNotSolvedString
        };
    }

    private long Optimize(int total, int? calorieReq = null)
    {
        var props = ParseInputLines(parseFunc: ParseIngredient).ToArray();
        var max = 0L;
        
        for (var i = 0; i <= total;         i++)
        for (var j = 0; j <= total - i;     j++)
        for (var k = 0; k <= total - i - j; k++)
        {
            var w = total - i - j - k;
            var cap = i * props[0].Cap + j * props[1].Cap + k * props[2].Cap + w * props[3].Cap;
            var dur = i * props[0].Dur + j * props[1].Dur + k * props[2].Dur + w * props[3].Dur;
            var fla = i * props[0].Fla + j * props[1].Fla + k * props[2].Fla + w * props[3].Fla;
            var tex = i * props[0].Tex + j * props[1].Tex + k * props[2].Tex + w * props[3].Tex;
            var cal = i * props[0].Cal + j * props[1].Cal + k * props[2].Cal + w * props[3].Cal;

            if (cap <= 0 || dur <= 0 || fla <= 0 || tex <= 0)
            {
                continue;
            }

            if (calorieReq != null && cal != calorieReq)
            {
                continue;
            }
            
            max = Math.Max(max, (long)cap * dur * fla * tex);
        }

        return max;
    }
    
    private static Properties ParseIngredient(string line)
    {
        var numbers = line.ParseInts();
        return new Properties(
            Cap: numbers[0],
            Dur: numbers[1],
            Fla: numbers[2],
            Tex: numbers[3],
            Cal: numbers[4]);
    }
}