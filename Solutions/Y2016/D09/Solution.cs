using Utilities.Extensions;

namespace Solutions.Y2016.D09;

[PuzzleInfo("Explosives in Cyberspace", Topics.StringParsing, Difficulty.Medium, favourite: true)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputText();
        var buffer = input.RemoveWhitespace();
        
        return part switch
        {
            1 => Decompress(buffer, recurse: false),
            2 => Decompress(buffer, recurse: true),
            _ => PuzzleNotSolvedString
        };
    }

    private static long Decompress(string buffer, bool recurse)
    {
        var length = 0L;
        var markerStart = 0;
        var dataScope = true;

        for (var i = 0; i < buffer.Length;)
        {
            switch (buffer[i])
            {
                case '(':
                    markerStart = i;
                    dataScope = false;
                    i++;
                    break;
                case ')':
                    var args = buffer[markerStart..i].ParseInts();
                    var amount = args[0];
                    var repeat = args[1];
                    
                    dataScope = true;
                    length += recurse
                        ? repeat * Decompress(buffer[(i + 1)..(i + amount + 1)], recurse)
                        : repeat * amount;
                    i += amount + 1;
                    break;
                default :
                    if (dataScope)
                    {
                        length++;
                    }
                    i++;
                    break;
            }
        }

        return length;
    }
}