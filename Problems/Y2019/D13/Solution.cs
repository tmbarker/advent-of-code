using Problems.Y2019.Common;
using Problems.Y2019.IntCode;

namespace Problems.Y2019.D13;

/// <summary>
/// Care Package: https://adventofcode.com/2019/day/13
/// </summary>
public class Solution : SolutionBase2019
{
    public override int Day => 13;
    
    public override object Run(int part)
    {
        return part switch
        {
            0 => CountGameObjects(GameObject.Block),
            1 => GetWinningScore(),
            _ => ProblemNotSolvedString
        };
    }

    private int CountGameObjects(GameObject type)
    {
        var vm = IntCodeVm.Create(LoadIntCodeProgram());
        vm.Run();

        return new Screen(vm.OutputBuffer).GetCount(type);
    }

    private long GetWinningScore()
    {
        var program = LoadFreeToPlayProgram();
        var arcadeMachine = IntCodeVm.Create(program);
        
        arcadeMachine.InputBuffer.Enqueue(Joystick.Neutral);
        arcadeMachine.Run();

        var screen = new Screen(arcadeMachine.OutputBuffer);
        while (screen.GetCount(GameObject.Block) > 0)
        {
            if (screen.Ball.X == screen.Paddle.X)
            {
                arcadeMachine.InputBuffer.Enqueue(Joystick.Neutral);
            }
            else if (screen.Ball.X > screen.Paddle.X)
            {
                arcadeMachine.InputBuffer.Enqueue(Joystick.Right);
            }
            else if (screen.Ball.X < screen.Paddle.X)
            {
                arcadeMachine.InputBuffer.Enqueue(Joystick.Left);
            }

            arcadeMachine.Run();
            screen.UpdatePixels(arcadeMachine.OutputBuffer);
        }

        return screen.Score;
    }

    private IList<long> LoadFreeToPlayProgram()
    {
        var program = LoadIntCodeProgram();
        program[0] = 2;
        return program;
    }
}