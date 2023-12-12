namespace Solutions.Y2022.D13;

public static class PacketParser
{
    public static IEnumerable<PacketPair> ParsePairs(IEnumerable<string> input)
    {
        var pairs = new List<PacketPair>();
        var trimmedLines = input
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .ToList();

        for (var i = 0; i < trimmedLines.Count; i += 2)
        {
            var first = ParseElement(trimmedLines[i]);
            var second = ParseElement(trimmedLines[i + 1]);

            pairs.Add(new PacketPair((i + 2) / 2, first, second));
        }

        return pairs;
    }

    public static IEnumerable<PacketElement> ParsePackets(IEnumerable<string> input)
    {
        return input
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(ParseElement);
    }

    public static PacketElement ParseElement(string elementString)
    {
        if (int.TryParse(elementString, out var value))
        {
            return new IntegerPacketElement(value);
        }

        var listCount = 0;
        var lastDelimiter = 0;
        var list = new List<PacketElement>();
        
        for (var i = 0; i < elementString.Length; i++)
        {
            switch (elementString[i])
            {
                case PacketElement.ListStart:
                    listCount++;
                    break;
                case PacketElement.ListEnd:
                    listCount--;
                    if (listCount == 0 && lastDelimiter + 1 < i)
                    {
                        //  We have hit the end of a List, parse from (just after) the previous list delimiter to here
                        //
                        list.Add(ParseElement(elementString[(lastDelimiter + 1)..i]));
                    }
                    break;
                case PacketElement.ElementDelimiter:
                    if (listCount == 1 && lastDelimiter + 1 < i)
                    {
                        //  We are in the first level of a list, and have hit a list delimiter, parse from just after
                        //  the previous list delimiter to here
                        //
                        list.Add(ParseElement(elementString[(lastDelimiter + 1)..i]));
                        lastDelimiter = i;
                    }
                    break;
            }
        }

        return new ListPacketElement(list);
    }
}