﻿using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Utilities.Extensions;

public static class StringExtensions
{
    private const RegexOptions Options = RegexOptions.Compiled;
    private static readonly Regex NumberRegex = new(pattern: @"(-?\d+)", Options);
    private static readonly Regex WhitespaceRegex = new(pattern: @"\s+", Options);

    /// <summary>
    ///     Parse the provided string, returning the first found number.
    /// </summary>
    /// <param name="s">The string to parse</param>
    /// <typeparam name="T">The type of number to parse</typeparam>
    /// <returns>The first parsed number</returns>
    /// <exception cref="FormatException">When at least one number cannot be parsed</exception>
    public static T ParseNumber<T>(this string s) where T : INumber<T>
    {
        var numbers = ParseNumbers<T>(s);
        return numbers.Length != 0
            ? numbers[0]
            : throw new FormatException($"Invalid number format [{s}]");
    }

    public static T[] ParseNumbers<T>(this string s) where T : INumber<T>
    {
        return NumberRegex.Matches(s)
            .Select(m => T.Parse(s: m.Value.AsSpan(), provider: null))
            .ToArray();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ParseInt(this string s) => ParseNumber<int>(s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long ParseLong(this string s) => ParseNumber<long>(s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int[] ParseInts(this string s) => ParseNumbers<int>(s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IList<long> ParseLongs(this string s) => ParseNumbers<long>(s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AsDigit(this char c)
    {
        return c - '0';
    }

    public static string RemoveWhitespace(this string str)
    {
        return WhitespaceRegex.Replace(input: str, replacement: string.Empty);
    }
}