using Utilities.Extensions;

namespace Solutions.Y2019.D16;

[PuzzleInfo("Flawed Frequency Transmission", Topics.Math, Difficulty.Hard)]
public sealed class Solution : SolutionBase
{
    private const int Phases = 100;
    private const int MessageLength = 8;
    
    private static readonly int[] BasePattern = [0, 1, 0, -1];

    public override object Run(int part)
    {
        var input = GetInputText();
        var initialData = ParseNumbers(input);
        
        return part switch
        {
            1 => Execute(data: initialData),
            2 => ExecuteFast(input: initialData, inputRepetitions: 10000, messageOffset: 7), 
            _ => PuzzleNotSolvedString
        };
    }

    private static string Execute(int[] data)
    {
        for (var i = 0; i < Phases; i++)
        {
            data = Phase(data);
        }

        return FormOutputString(data.Take(MessageLength));
    }
    
    private static string ExecuteFast(int[] input, int inputRepetitions, int messageOffset)
    {
        var inputLength = input.Length * inputRepetitions;
        var digitsToSkip = Enumerable
            .Range(0, messageOffset)
            .Sum(n => (int)Math.Pow(10, n) * input[messageOffset - n - 1]);
        
        // The below method only works if our output message is wholly in the second half of the data stream
        //
        if (digitsToSkip < inputLength / 2)
        {
            throw new NoSolutionException(message: "Solution not valid for provided input");
        }

        var outputLength = inputLength - digitsToSkip;
        var data = new int[outputLength];
        var output = new int[outputLength];

        for (var i = 0; i < outputLength; i++)
        {
            data[i] = input[(i + digitsToSkip) % input.Length];
        }
        
        // In the second half of the datastream, each pattern digit is always '1', this means that each output digit
        // is equal to the sum of all digits after it, mod 10. We can compute the digits from the back to the front,
        // preserving the trailing sum from the previously computed digit
        //
        for (var i = 0; i < Phases; i++)
        {
            var trailingSum = data[^1];
            output[^1] = trailingSum;
            
            for (var j = 1; j < outputLength; j++)
            {
                trailingSum = (trailingSum + data[outputLength - j - 1]) % 10;
                output[outputLength - j - 1] = trailingSum;
            }

            data = output;
        }
        
        return FormOutputString(data.Take(MessageLength));
    }
    
    private static int[] Phase(int[] data)
    {
        var length = data.Length;
        var output = new int[length];
        
        for (var i = 0; i < length; i++)
        {
            for (var j = 0; j < length; j++)
            {
                var posInPattern = (j + 1) % ((i + 1) * BasePattern.Length);
                var patternElement = BasePattern[posInPattern / (i + 1)];
                
                output[i] += data[j] * patternElement;
            }

            output[i] = Math.Abs(output[i]) % 10;
        }

        return output;
    }
    
    private static string FormOutputString(IEnumerable<int> data)
    {
        return string.Join(string.Empty, data);
    }
    
    private static int[] ParseNumbers(string input)
    {
        return input.Select(StringExtensions.AsDigit).ToArray();
    }
}