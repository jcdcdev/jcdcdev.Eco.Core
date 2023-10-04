using System.Diagnostics.CodeAnalysis;

namespace jcdcdev.Eco.Core.Extensions;

public static class StringExtensions
{
    public static string EnsureEndsWith(this string str, string value) => !str.EndsWith(value) ? $"{str}{value}" : str;

    public static bool IsNotNullOrWhitespace([NotNullWhen(true)] this string? str) => !string.IsNullOrWhiteSpace(str);
}