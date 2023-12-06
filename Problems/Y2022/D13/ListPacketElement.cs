using System.Text;

namespace Problems.Y2022.D13;

public sealed class ListPacketElement : PacketElement
{
    private readonly List<PacketElement> _elements = new ();
    
    public ListPacketElement(IEnumerable<PacketElement>? elements)
    {
        if (elements == null)
        {
            return;
        }
        
        foreach (var element in elements)
        {
            _elements.Add(element);
        }
    }

    public PacketElement this[int i] => _elements[i];
    public int Count => _elements.Count;

    public bool HasElementAtIndex(int i)
    {
        return i >= 0 && i < _elements.Count;
    }
    
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(ListStart);

        for (var i = 0; i < _elements.Count; i++)
        {
            sb.Append(_elements[i]);
            if (i != _elements.Count - 1)
            {
                sb.Append(ElementDelimiter);
            }
        }

        sb.Append(ListEnd);
        return sb.ToString();
    }
}