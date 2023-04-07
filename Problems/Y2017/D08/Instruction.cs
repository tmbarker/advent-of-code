namespace Problems.Y2017.D08;

public readonly record struct Instruction(string DestReg, string DestOp, int DestArg, string SrcReg, string CheckOp, int CheckArg);