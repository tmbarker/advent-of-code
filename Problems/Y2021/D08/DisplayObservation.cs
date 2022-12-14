namespace Problems.Y2021.D08;

public readonly struct DisplayObservation
{
    public DisplayObservation(IEnumerable<string> uniqueSignalPatterns, IEnumerable<string> outputDigits)
    {
        UniqueSegmentPatterns.AddRange(uniqueSignalPatterns);
        OutputDigitSegments.AddRange(outputDigits);
    }

    public List<string> UniqueSegmentPatterns { get; } = new();
    public List<string> OutputDigitSegments { get; } = new();
}