namespace Problems.Y2022.D11;

public readonly struct Monkey
{
    private const int BoredDivisor = 3;
    
    // TODO: Inject this value
    private readonly long _testDivisorProduct = 2 * 17 * 7 * 11 * 19 * 5 * 13 * 3;
    private readonly Queue<long> _items = new ();

    public Operator InspectionOperator { get; init; }
    public int InspectionOperand { get; init; }
    public int TestDivisor { get; init; }
    public int ThrowToOnSuccess { get; init; }
    public int ThrowToOnFailure { get; init; }
    public bool ApplyBoredDivisor { get; init; }

    public bool IsHoldingItem => _items.Count > 0;
    
    public Monkey(IEnumerable<int> startingItems)
    {
        foreach (var item in startingItems)
        {
            _items.Enqueue(item);
        }

        InspectionOperator = default;
        InspectionOperand = default;
        TestDivisor = default;
        ThrowToOnSuccess = default;
        ThrowToOnFailure = default;
        ApplyBoredDivisor = default;
    }

    public (int throwTo, long thrownItem) InspectNextItem()
    {
        var item = EvaluateInspectionOperand(_items.Dequeue());
        
        if (ApplyBoredDivisor)
        {
            item = (int)Math.Floor((float)item / BoredDivisor);   
        }
        else
        {
            item %= _testDivisorProduct;
        }

        return (item % TestDivisor == 0 ? ThrowToOnSuccess : ThrowToOnFailure, item);
    }

    public void CatchItem(long item)
    {
        _items.Enqueue(item);
    }
    
    private long EvaluateInspectionOperand(long arg)
    {
        return InspectionOperator switch
        {
            Operator.Add => arg + InspectionOperand,
            Operator.Multiply => arg * InspectionOperand,
            Operator.Square => arg * arg,
            _ => throw new InvalidOperationException($"{nameof(InspectionOperator)} not recognized [{InspectionOperator}]")
        };
    }
}