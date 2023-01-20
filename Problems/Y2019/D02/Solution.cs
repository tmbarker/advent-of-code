using Problems.Common;
using Problems.Y2019.Common;

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
        var memory = ParseMemory(GetInputText());
        return part switch
        {
            0 => RunProgram(memory),
            1 => FindPartsOfSpeech(memory),
            _ => ProblemNotSolvedString,
        };
    }

    private static int RunProgram(IList<int> memory)
    {
        memory[1] = 12;
        memory[2] = 2;

        return Vm.Run(memory)[0];
    }

    private static int FindPartsOfSpeech(IList<int> memory)
    {
        for (var noun = 0; noun < MaxPartOfSpeech; noun++)
        for (var verb = 0; verb < MaxPartOfSpeech; verb++)
        {
            var sandbox = new List<int>(memory)
            {
                [1] = noun,
                [2] = verb
            };

            if (Vm.Run(sandbox)[0] == Target)
            {
                return 100 * noun + verb;
            }
        }

        throw new NoSolutionException();
    }

    private static IList<int> ParseMemory(string input)
    {
        return new List<int>(input.Split(',').Select(int.Parse));
    }
}