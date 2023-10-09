using System.Collections;
using System.Runtime.InteropServices;

namespace jcdcdev.Eco.Core.Extensions;

public class ColorLogBuilder
{
    private readonly List<ColorLogToken> _parts = new();
    private readonly KeyValuePair<string, ConsoleColor> _prefix;
    private readonly bool _hasPrefix;


    private ColorLogBuilder()
    {
        // Add(Environment.NewLine, ConsoleColor.White);
    }

    public ColorLogBuilder Append(string text, ConsoleColor color = ConsoleColor.White)
    {
        Add(text, color, false);
        return this;
    }

    public ColorLogBuilder AppendLine(string text, ConsoleColor color = ConsoleColor.White)
    {
        Add($"{text}", color);
        return this;
    }

    private void Add(string text, ConsoleColor color, bool newLine = true)
    {
        var token = new ColorLogToken
        {
            Text = text,
            Color = color,
            NewLine = newLine
        };

        _parts.Add(token);
    }

    public static ColorLogBuilder Create() => new();

    public IEnumerable<ColorLogToken> Build() => _parts;
    public void Clear() => _parts.Clear();
}

public class ColorLogToken
{
    public string Text { get; set; }
    public ConsoleColor Color { get; set; }
    public bool NewLine { get; set; }
}