using Problems.Common;

namespace Problems.Y2021.D24;

/// <summary>
/// Arithmetic Logic Unit: https://adventofcode.com/2021/day/24
/// </summary>
public class Solution : SolutionBase
{
    /*
    The MONAD program repeats the following block for each of the 14 input digits. The only differences between
    blocks are parameterized in brackets below:
     
        inp w
        mul x 0
        add x z
        mod x 26
        div z <DIV>
        add x <CHECK>
        eql x w
        eql x 0
        mul y 0
        add y 25
        mul y x
        add y 1
        mul z y
        mul y 0
        add y w
        add y <ADD>
        mul y x
        add z y
        
    The values for my input are summarized in the table below, where DIGIT is the 0 based digit index, from MSB to LSB

        DIGIT  |  DIV  |  CHECK  |   ADD
          0    |   1   |    14   |   12
          1    |   1   |    15   |    7
          2    |   1   |    12   |    1
          3    |   1   |    11   |    2
          4    |  26   |   - 5   |    4
          5    |   1   |    14   |   15
          6    |   1   |    15   |   11
          7    |  26   |   -13   |    5
          8    |  26   |   -16   |    3
          9    |  26   |   - 8   |    9
         10    |   1   |    15   |    2
         11    |  26   |   - 8   |    3
         12    |  26   |     0   |    3
         13    |  26   |   - 4   |   11
        
    Note that when CHECK is strictly greater than 0 DIV is always 1, and when CHECK is less than or equal to 0 DIV is 
    always 26. Also note that on each cycle of the above instruction block, only the value of z persists:
       - w is set by the 'inp' instruction
       - x is cleared by the 'mul x 0' instruction
       - y is cleared by the 'mul y 0' instruction 
       
    Because of this each block of the above instructions can be seen as equivalent to executing the following logic:
        1. Clear all registers except for z
        2. Read the next input 'digit' [1..9]
        3. Depending on the value of DIV:
            3a) DIV == 1:
                1. The condition (z % 26 + CHECK) =/= digit is evaluated
                2. In this context CHECK is always >= 10, and because digit is [1..9] this condition is always true
                3. Therefore, z => (z * 26) + digit + ADD
            3b) DIV == 26: 
                1. The condition (z % 26 + CHECK) =/= digit is evaluated
                2. In this context CHECK is always less than or equal to 0, it is possible for this condition to be false
                3. Therefore, z => (z / 26) * (25 * condition + 1) + condition * (d + ADD)
                    When condition is true:  z => z + d + ADD
                    When condition is false: z => z / 26
                   
    Therefore, by constraining our input such that the condition is false for cases when DIV == 26, it is possible to
    change z after each block as follows:
        a) When DIV == 1:  z => z * 26 + d + ADD  [Note that d + ADD will always be less than 26]
        b) When DIV == 26: z => z / 26
         
    Therefore, we can think of each block as either pushing a base-26 number to a stack, or popping the stack. Case a)
    would correspond to pushing to the stack, while case b) would correspond to popping the stack. Having a 0 in the z
    register at the end of the MONAD program execution is isomorphic to having an empty stack. This means we must
    constrain our input digits so each DIV == 26 block becomes a pop. 
    
    Mapping the above table to stack actions looks as follows:

        PUSH    =>   digit[0] + 12
        PUSH    =>   digit[1] +  7
        PUSH    =>   digit[2] +  1
        PUSH    =>   digit[3] +  2
        POP     =>   digit[4] == (digit[3] + 2) - 5
        PUSH    =>   digit[5] + 15
        PUSH    =>   digit[6] + 11
        POP     =>   digit[7] == (digit[6] + 11) - 13
        POP     =>   digit[8] == (digit[5] + 15) - 16
        POP     =>   digit[9] == (digit[2] + 1) - 8
        PUSH    =>  digit[10] + 2
        POP     =>  digit[11] == (digit[10] + 2) - 8
        POP     =>  digit[12] == (digit[1] + 7) - 0
        POP     =>  digit[13] == (digit[0] + 12) - 4
 
    The POPs give us 7 equalities, which we can solve to find our minimum and maximum model numbers:
    
         digit[4] = digit[3] - 3
         digit[7] = digit[6] - 2
         digit[8] = digit[5] - 1
         digit[9] = digit[2] - 7
        digit[11] = digit[10] - 6
        digit[12] = digit[1] + 7
        digit[13] = digit[0] + 8
        
    Therefore our extrema are:
    Max: 12996997829399
    Min: 11841231117189
    */
    
    private const string LargestModelNumber  = "12996997829399";
    private const string SmallestModelNumber = "11841231117189";

    public override object Run(int part)
    {
        return part switch
        {
            1 => TryModelNumber(GetInputLines(), LargestModelNumber),
            2 => TryModelNumber(GetInputLines(), SmallestModelNumber),
            _ => ProblemNotSolvedString
        };
    }

    private static string TryModelNumber(IEnumerable<string> monadProgram, string modelNumber)
    {
        var digitBuffer = new Queue<int>(modelNumber.Select(c => c - '0'));
        var result = Alu.Run(monadProgram, digitBuffer);

        return result == 0
            ? modelNumber
            : ProblemNotSolvedString;
    }
}