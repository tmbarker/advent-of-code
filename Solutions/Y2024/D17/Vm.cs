using System.Numerics;

namespace Solutions.Y2024.D17;

public static class Vm
{
    public static Queue<int> Run(int[] program, BigInteger a)
    {
        var b = BigInteger.Zero;
        var c = BigInteger.Zero;

        var ip = 0;
        var @out = new Queue<int>();
        
        while (ip >= 0 && ip < program.Length)
        {
            var opcode  = program[ip++];
            var operand = program[ip++];
            
            switch (opcode)
            {
                case 0: // adv
                    a >>= (int)ComboOperand(operand);
                    break;
                case 1: // bxl
                    b ^= operand;
                    break;
                case 2: // bst
                    b = ComboOperand(operand) & 0b111;
                    break;
                case 3: // jnz
                    if (a != BigInteger.Zero) ip = operand;
                    break;
                case 4: // bxc
                    b ^= c;
                    break;
                case 5: // out
                    @out.Enqueue((int)(ComboOperand(operand) & 0b111));
                    break;
                case 6: // bdv
                    b = a >> (int)ComboOperand(operand);
                    break;
                case 7: // cdv
                    c = a >> (int)ComboOperand(operand); 
                    break;
            }
        }

        return @out;
        
        BigInteger ComboOperand(int operand)
        {
            return operand switch
            {
                0 or 1 or 2 or 3 => operand,
                4 => a,
                5 => b,
                6 => c,
                _ => throw new InvalidOperationException($"Operand {operand} is not supported")
            };
        }
    }
    
    public static Queue<int> Simulate(BigInteger a)
    {
        //  NOTE: This method was written by analyzing the input program. See adjacent asm.text. 
        //
        var @out = new Queue<int>();
        do
        {
            var b = (a & 0b111) ^ 0b001;
            var c = a >>> (int)b;
            
            a >>>= 3;
            b = b ^ 0b100 ^ c;
            
            @out.Enqueue((int)(b & 0b111));
        } while (a != 0);

        return @out;
    }
}