namespace Problems.Y2021.D18;

using SfNumber = List<Element>;

public static class SnailfishParser
{
    private const char Open = '[';
    private const char Close = ']';
    private const char Delim = ',';
    
    private static readonly HashSet<char> SyntaxSet =
    [
        Open,
        Close,
        Delim
    ];
    
    public static SfNumber Parse(string number)
    {
        var elements = new SfNumber();
        var valueBuffer = new Queue<char>();

        foreach (var c in number)
        {
            if (SyntaxSet.Contains(c) && valueBuffer.Count > 0)
            {
                var valueString = string.Concat(valueBuffer);
                var valueElement = new Element(int.Parse(valueString));
                
                valueBuffer.Clear();
                elements.Add(valueElement);
            }
            
            switch (c)
            {
                case Open:
                    elements.Add(Element.Open);
                    continue;
                case Close:
                    elements.Add(Element.Close);
                    continue;
                case Delim:
                    elements.Add(Element.Delim);
                    continue;
            }

            valueBuffer.Enqueue(c);
        }

        return elements;
    }
}