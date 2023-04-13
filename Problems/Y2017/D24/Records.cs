namespace Problems.Y2017.D24;

public readonly record struct Adapter(string Key, int Port1, int Port2);
public readonly record struct Bridge(int Strength, int Length);
public readonly record struct Compatibility(int ResultingPort, string ViaAdapter);