namespace Problems.Y2022.D05;

public class CranePlan
{
    private const char DataElementDelimiter = ' ';
    
    private const int InstructionNumMovesElementIndex = 1;
    private const int InstructionSourceElementIndex = 3;
    private const int InstructionDestinationElementIndex = 5;
    private const int NumStackAndInstructionSeparatorLines = 1;

    private const int StackItemCharacterOffset = 1;
    private const int StackItemCharacterFrequency = 4;

    private CranePlan(StacksState initialStacksState, IEnumerable<CraneInstruction> instructions)
    {
        InitialStacksState = initialStacksState;
        Instructions = instructions;
    }
    
    public StacksState InitialStacksState { get; }
    public IEnumerable<CraneInstruction> Instructions { get; }

    public static bool TryParse(IEnumerable<string> lines, out CranePlan? cranePlan)
    {
        var enumeratedLines = lines.ToList();
        var numLines = enumeratedLines.Count;

        for (var i = 0; i < numLines; i++)
        {
            // The last line of stack state data will be the first line which contains a decimal/numerical character
            if (!enumeratedLines[i].Any(char.IsDigit))
            {
                continue;
            }

            var stackLines = enumeratedLines.Take(i + 1);
            var instructionLines = enumeratedLines.Skip(i + NumStackAndInstructionSeparatorLines + 1);
            
            var stacksState = ParseStacksState(stackLines);
            var instructions = ParseCraneInstructions(instructionLines);

            cranePlan = new CranePlan(stacksState, instructions);
            return true;
        }

        cranePlan = null;
        return false;
    }

    private static StacksState ParseStacksState(IEnumerable<string> lines)
    {
        // Let's reverse the list before enumerating, this means the first element will contain our stack IDs
        var enumeratedLines = lines
            .Reverse()
            .ToList();
        
        var stacksMap = enumeratedLines
            .First()
            .Split(DataElementDelimiter, StringSplitOptions.RemoveEmptyEntries)
            .ToDictionary(int.Parse, _ => new Stack<char>());

        for (var i = 1; i < enumeratedLines.Count; i++)
        {
            for (var s = 0; s < stacksMap.Count; s++)
            {
                var targetCharIndex = StackItemCharacterOffset + s * StackItemCharacterFrequency;
                if (targetCharIndex >= enumeratedLines[i].Length)
                {
                    break;
                }

                if (char.IsLetter(enumeratedLines[i][targetCharIndex]))
                {
                    // Stack indexing is 1-based
                    stacksMap[s + 1].Push(enumeratedLines[i][targetCharIndex]);   
                }
            }
        }

        return new StacksState(stacksMap);
    }

    private static IEnumerable<CraneInstruction> ParseCraneInstructions(IEnumerable<string> lines)
    {
        return lines.Select(ParseCraneInstruction);
    }

    private static CraneInstruction ParseCraneInstruction(string line)
    {
        var elements = line.Split(DataElementDelimiter);
        return new CraneInstruction
        {
            NumMoves = int.Parse(elements[InstructionNumMovesElementIndex]),
            SourceStack = int.Parse(elements[InstructionSourceElementIndex]),
            DestinationStack = int.Parse(elements[InstructionDestinationElementIndex]),
        };
    }
}