namespace Solutions.Y2021.D08;

public readonly struct DisplayObservation(IEnumerable<string> uniqueSignalPatterns, IEnumerable<string> outputDigits)
{
    public List<string> UniqueSegmentPatterns { get; } = [..uniqueSignalPatterns];
    public List<string> OutputDigitSegments { get; } = [..outputDigits];
}