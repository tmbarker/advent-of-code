namespace Problems.Y2019.IntCode;

/// <summary>
/// Opcodes for an <see cref="IntCode"/> virtual machine (<see cref="Vm"/>)
/// </summary>
internal enum OpCode
{
    /// <summary>
    /// Add
    /// </summary>
    Add = 1,
    /// <summary>
    /// Multiply
    /// </summary>
    Mul = 2,
    /// <summary>
    /// Input
    /// </summary>
    Inp = 3,
    /// <summary>
    /// Output
    /// </summary>
    Out = 4,
    /// <summary>
    /// Jump if true
    /// </summary>
    Jit = 5,
    /// <summary>
    /// Jump if false
    /// </summary>
    Jif = 6,
    /// <summary>
    /// Less than
    /// </summary>
    Lst = 7,
    /// <summary>
    /// Equals
    /// </summary>
    Eql = 8,
}