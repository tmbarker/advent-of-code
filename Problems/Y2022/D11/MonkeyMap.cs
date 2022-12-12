namespace Problems.Y2022.D11;

public static class MonkeyMap
{
    // TODO: Build a Regex and actually parse this data
    public static Dictionary<int, Monkey> Parse(IList<string> data, bool applyBoredDivisor)
    {
        return new Dictionary<int, Monkey>()
        {
            {0, new Monkey(new[] {99, 63, 76, 93, 54, 73})
            {
                InspectionOperator = Operator.Multiply,
                InspectionOperand = 11,
                TestDivisor = 2,
                ThrowToOnSuccess = 7,
                ThrowToOnFailure = 1,
                ApplyBoredDivisor = applyBoredDivisor,
            }},
            {1, new Monkey(new[] {91, 60, 97, 54})
            {
                InspectionOperator = Operator.Add,
                InspectionOperand = 1,
                TestDivisor = 17,
                ThrowToOnSuccess = 3,
                ThrowToOnFailure = 2,
                ApplyBoredDivisor = applyBoredDivisor,
            }},
            {2, new Monkey(new[] {65})
            {
                InspectionOperator = Operator.Add,
                InspectionOperand = 7,
                TestDivisor = 7,
                ThrowToOnSuccess = 6,
                ThrowToOnFailure = 5,
                ApplyBoredDivisor = applyBoredDivisor,
            }},
            {3, new Monkey(new[] {84, 55})
            {
                InspectionOperator = Operator.Add,
                InspectionOperand = 3,
                TestDivisor = 11,
                ThrowToOnSuccess = 2,
                ThrowToOnFailure = 6,
                ApplyBoredDivisor = applyBoredDivisor,
            }},
            {4, new Monkey(new[] {86, 63, 79, 54, 83})
            {
                InspectionOperator = Operator.Square,
                TestDivisor = 19,
                ThrowToOnSuccess = 7,
                ThrowToOnFailure = 0,
                ApplyBoredDivisor = applyBoredDivisor,
            }},
            {5, new Monkey(new[] {96, 67, 56, 95, 64, 69, 96})
            {
                InspectionOperator = Operator.Add,
                InspectionOperand = 4,
                TestDivisor = 5,
                ThrowToOnSuccess = 4,
                ThrowToOnFailure = 0,
                ApplyBoredDivisor = applyBoredDivisor,
            }},
            {6, new Monkey(new[] {66, 94, 70, 93, 72, 67, 88, 51})
            {
                InspectionOperator = Operator.Multiply,
                InspectionOperand = 5,
                TestDivisor = 13,
                ThrowToOnSuccess = 4,
                ThrowToOnFailure = 5,
                ApplyBoredDivisor = applyBoredDivisor,
            }},
            {7, new Monkey(new[] {59, 59, 74})
            {
                InspectionOperator = Operator.Add,
                InspectionOperand = 8,
                TestDivisor = 3,
                ThrowToOnSuccess = 1,
                ThrowToOnFailure = 3,
                ApplyBoredDivisor = applyBoredDivisor,
            }},
        };
    }
}