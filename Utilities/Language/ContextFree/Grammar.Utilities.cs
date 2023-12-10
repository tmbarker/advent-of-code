using System.Runtime.CompilerServices;

namespace Utilities.Language.ContextFree;

public sealed partial class Grammar
{
    /// <summary>
    /// Does this production yield exactly one terminal?
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUnitTerminal(Production p, IReadOnlySet<string> t)
    {
        return p.Yields.Count == 1 && t.Contains(p.Yields[0]);
    }

    /// <summary>
    /// Does this production yield exactly two non-terminals? 
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBinaryNonTerminal(Production p, IReadOnlySet<string> nt)
    {
        return p.Yields.Count == 2 && nt.Contains(p.Yields[0]) && nt.Contains(p.Yields[1]);
    }
    
    /// <summary>
    /// Does this production yield both terminals and non-terminals? 
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNonSolitary(Production p, IReadOnlySet<string> nt, IReadOnlySet<string> t)
    {
        return p.Yields.Any(nt.Contains) && p.Yields.Any(t.Contains);
    }

    /// <summary>
    /// Does this production yield more than two non-terminals?
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSupraBinaryNonTerminal(Production p, IReadOnlySet<string> nt)
    {
        return p.Yields.Count > 2 && p.Yields.All(nt.Contains);
    }
    
    /// <summary>
    /// Does this production yield exactly one non-terminals?
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUnitNonTerminal(Production p, IReadOnlySet<string> nt)
    {
        return p.Yields.Count == 1 && nt.Contains(p.Yields[0]);
    }
}