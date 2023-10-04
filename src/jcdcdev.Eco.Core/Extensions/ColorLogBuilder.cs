namespace jcdcdev.Eco.Core.Extensions;

public class ColorLogBuilder
{
    private readonly List<KeyValuePair<string, ConsoleColor>> _parts = new();

    private ColorLogBuilder() { }

    public ColorLogBuilder Append(string text, ConsoleColor color = ConsoleColor.Gray)
    {
        Add(text, color);
        return this;
    }

    public ColorLogBuilder AppendLine(string text, ConsoleColor color = ConsoleColor.Gray)
    {
        Add(text, color);
        Add(Environment.NewLine, color);
        return this;
    }

    private void Add(string text, ConsoleColor color) => _parts.Add(new KeyValuePair<string, ConsoleColor>(text, color));

    public IEnumerable<KeyValuePair<string, ConsoleColor>> Build() => _parts;

    public static ColorLogBuilder Create() => new();
}