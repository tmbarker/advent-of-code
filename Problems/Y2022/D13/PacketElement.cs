namespace Problems.Y2022.D13;

public abstract class PacketElement : IComparable<PacketElement>
{
    public const char ElementDelimiter = ',';
    public const char ListStart = '[';
    public const char ListEnd = ']';
    
    public int CompareTo(PacketElement? other)
    {
        if (other == null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        return (int)PacketComparator.CompareElements(this, other);
    }
}