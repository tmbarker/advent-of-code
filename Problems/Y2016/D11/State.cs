using System.Text;

namespace Problems.Y2016.D11;

public class State : IEquatable<State>
{
    private readonly string _key;

    public int Elevator { get; }
    public IList<Device> Devices { get; }

    public bool IsFinished => Devices.All(device => device.Floor == 4);

    public State(int elevator, IList<Device> devices)
    {
        _key = BuildKey(elevator, devices);
        Elevator = elevator;
        Devices = devices;
    }

    private static bool IsMicrochip(Device device) => device.Type == DeviceType.Microchip;
    private static bool IsGenerator(Device device) => device.Type == DeviceType.Generator;
    
    private static string BuildKey(int elevator, IEnumerable<Device> devices)
    {
        //  We can uniquely key states based on the position of the elevator, and the positions of each pair of devices.
        //  Note that we only care about the set of positions that the pairs of devices occupy, and not which specific
        //  pair it is that occupies any given position.
        //
        var sb = new StringBuilder();
        var devicePairs = devices
            .GroupBy(device => device.Element)
            .Select(grouping => $"<{grouping.Single(IsMicrochip).Floor}-{grouping.Single(IsGenerator).Floor}>")
            .Order();
        
        sb.Append($"[Elevator: {elevator}]");
        sb.Append($"[Devices: {string.Join("", devicePairs)}]");
        
        return sb.ToString();
    }

    public override string ToString()
    {
        return _key;
    }

    public bool Equals(State? other)
    {
        if (ReferenceEquals(null, other)) return false;
        return ReferenceEquals(this, other) || string.Equals(a: _key, b: other._key);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((State)obj);
    }

    public override int GetHashCode()
    {
        return _key.GetHashCode();
    }

    public static bool operator ==(State? left, State? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(State? left, State? right)
    {
        return !Equals(left, right);
    }
}