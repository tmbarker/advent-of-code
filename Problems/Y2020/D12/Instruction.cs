namespace Problems.Y2020.D12;

public readonly struct Instruction
{
    public Instruction(char command, int amount)
    {
        Command = command;
        Amount = amount;
    }
    
    public char Command { get; }
    public int Amount { get; }

    public void Deconstruct(out char command, out int amount)
    {
        command = Command;
        amount = Amount;
    }
}