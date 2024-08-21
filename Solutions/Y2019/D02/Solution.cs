using Solutions.Y2019.IntCode;

namespace Solutions.Y2019.D02;

[PuzzleInfo("1202 Program Alarm", Topics.IntCode, Difficulty.Easy)]
public sealed class Solution : IntCodeSolution
{
    private const int Target = 19690720;
    private const int MaxPartOfSpeech = 99;

    public override object Run(int part)
    {
        var program = LoadIntCodeProgram();
        return part switch
        {
            1 => RunProgram(program),
            2 => FindPartsOfSpeech(program),
            _ => PuzzleNotSolvedString
        };
    }

    private static long RunProgram(IList<long> program)
    {
        program[1] = 12;
        program[2] = 2;
        
        var vm = IntCodeVm.Create(program); 
        var ec = vm.Run();

        return ec == IntCodeVm.ExitCode.Halted
            ? vm.Memory[0]
            : throw new NoSolutionException(message: $"Invalid VM exit code [{ec}]");
    }

    private static long FindPartsOfSpeech(IList<long> program)
    {
        for (var noun = 0; noun < MaxPartOfSpeech; noun++)
        for (var verb = 0; verb < MaxPartOfSpeech; verb++)
        {
            var modified = new List<long>(program)
            {
                [1] = noun,
                [2] = verb
            };

            var vm = IntCodeVm.Create(modified);
            var ec = vm.Run();
            
            if (ec == IntCodeVm.ExitCode.Halted && vm.Memory[0] == Target)
            {
                return 100L * noun + verb;
            }
        }

        throw new NoSolutionException();
    }
}