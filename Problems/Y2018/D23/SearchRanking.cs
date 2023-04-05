namespace Problems.Y2018.D23;

public readonly struct SearchRanking : IComparable<SearchRanking>
{
    private readonly long _inRange;
    private readonly long _distance;
    private readonly long _volume;

    public SearchRanking(long inRange, long distance, long volume)
    {
        _inRange = inRange;
        _distance = distance;
        _volume = volume;
    }

    public int CompareTo(SearchRanking other)
    {
        // If one region intersects the range of more bots than the other, rank it higher
        //
        var inRangeComparison = _inRange.CompareTo(other._inRange);
        if (inRangeComparison != 0)
        {
            return -1 * inRangeComparison;
        }
        
        // Otherwise, if one region is close to the origin, rank it higher
        //
        var distanceComparison = _distance.CompareTo(other._distance);
        if (distanceComparison != 0)
        {
            return distanceComparison;
        }
        
        // Finally, rank the smaller region (by volume) higher
        //
        return _volume.CompareTo(other._volume);
    }

    public override string ToString()
    {
        return $"[Bots in range: {_inRange}] [Distance: {_distance}] [Volume: {_volume}]";
    }
}