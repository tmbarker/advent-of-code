using Utilities.Collections;

namespace Utilities.Language.ContextFree;

using Key = ValueTuple<int, int, string>;
using BackRef = ValueTuple<int, string, string>;

/// <summary>
/// A value type representing the table used in an execution of the CYK algorithm
/// </summary>
public readonly struct CykTable
{
    public DefaultDict<Key, bool> P { get; } = new(defaultValue: false);
    public DefaultDict<Key, HashSet<BackRef>> B { get; } = new(defaultValue: new HashSet<BackRef>());
    
    public CykTable()
    {
    }
}