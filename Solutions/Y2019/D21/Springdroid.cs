using System.Text;
using Solutions.Y2019.IntCode;

namespace Solutions.Y2019.D21;

public static class Springdroid
{
    private const int AsciiRange = 255;
    
    public static bool Run(IList<long> firmware, IEnumerable<string> program, out string output)
    {
        var input = Compile(program);
        var vm = IntCodeVm.Create(firmware, input);
        
        vm.Run();

        if (vm.OutputBuffer.Last() > AsciiRange)
        {
            output = vm.OutputBuffer.Last().ToString();
            return true;
        }

        output = ReadAsciiOutput(vm);
        return false;
    }

    private static IEnumerable<long> Compile(IEnumerable<string> script)
    {
        foreach (var instr in script)
        {
            foreach (var c in instr)
            {
                yield return c;
            }

            yield return '\n';
        }
    }
    
    private static string ReadAsciiOutput(IntCodeVm vm)
    {
        var sb = new StringBuilder();
        while (vm.OutputBuffer.Any())
        {
            sb.Append((char)vm.OutputBuffer.Dequeue());
        }

        return sb.ToString();
    }
}