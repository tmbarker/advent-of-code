namespace Solutions.Y2022.D13;

public static class PacketComparator
{
    public static ComparisonResult CompareElements(PacketElement lhs, PacketElement rhs)
    {
        if (lhs is IntegerPacketElement iLhs && rhs is IntegerPacketElement iRhs)
        {
            return CompareIntegers(iLhs, iRhs);
        }

        var lLhs = lhs as ListPacketElement ?? ((IntegerPacketElement)lhs).AsList();
        var lRhs = rhs as ListPacketElement ?? ((IntegerPacketElement)rhs).AsList();

        return CompareLists(lLhs, lRhs);
    }
    
    private static ComparisonResult CompareLists(ListPacketElement lhs, ListPacketElement rhs)
    {
        var lhsLength = lhs.Count;
        var rhsLength = rhs.Count;

        for (var i = 0; i < Math.Max(lhsLength, rhsLength); i++)
        {
            if (!lhs.HasElementAtIndex(i) && rhs.HasElementAtIndex(i))
            {
                return ComparisonResult.Ordered;
            }
            if (!rhs.HasElementAtIndex(i) && lhs.HasElementAtIndex(i))
            {
                return ComparisonResult.Scrambled;
            }

            var elementComparisonResult = CompareElements(lhs[i], rhs[i]);
            if (elementComparisonResult != ComparisonResult.Indeterminate)
            {
                return elementComparisonResult;
            }
        }

        return ComparisonResult.Indeterminate;
    }

    private static ComparisonResult CompareIntegers(IntegerPacketElement lhs, IntegerPacketElement rhs)
    {
        if (lhs.Value == rhs.Value)
        {
            return ComparisonResult.Indeterminate;
        }

        return lhs.Value < rhs.Value ? ComparisonResult.Ordered : ComparisonResult.Scrambled;
    }
}