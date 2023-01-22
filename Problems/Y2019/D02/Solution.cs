using Problems.Common;
using Problems.Y2019.Common;
using Problems.Y2019.IntCode;

namespace Problems.Y2019.D02;

/// <summary>
/// 1202 Program Alarm: https://adventofcode.com/2019/day/2
/// </summary>
public class Solution : SolutionBase2019
{
    private const int Target = 19690720;
    private const int MaxPartOfSpeech = 99;

    public override int Day => 2;
    
    public override object Run(int part)
    {
        var program = LoadIntCodeProgram();
        return part switch
        {
            0 => RunProgram(program),
            1 => FindPartsOfSpeech(program),
            _ => ProblemNotSolvedString,
        };
    }

    private static int RunProgram(IList<int> program)
    {
        program[1] = 12;
        program[2] = 2;
        
        IntCodeVm.Create(program).Run();
        return program[0];
    }

    private static int FindPartsOfSpeech(IList<int> program)
    {
        for (var noun = 0; noun < MaxPartOfSpeech; noun++)
        for (var verb = 0; verb < MaxPartOfSpeech; verb++)
        {
            var modified = new List<int>(program)
            {
                [1] = noun,
                [2] = verb
            };
            
            IntCodeVm.Create(modified).Run();
            if (modified[0] == Target)
            {
                return 100 * noun + verb;
            }
        }

        throw new NoSolutionException();
    }
}