using Eco.Shared.Localization;

namespace jcdcdev.Eco.Core.Extensions;

public static class Logger
{
    private static readonly object Lock = new();

    public static void Write(ColorLogBuilder builder) => WriteLine(builder.Build());

    public static void WriteLine(string message, ConsoleColor color = ConsoleColor.Gray)
    {
        lock (Lock)
        {
            WriteTimeStamp();
            Write($"{message}", color);
            WriteNewLine();
        }
    }

    public static void WriteLine(IEnumerable<KeyValuePair<string, ConsoleColor>> parts)
    {
        lock (Lock)
        {
            WriteTimeStamp();
            foreach (var kvp in parts) Write(kvp.Key, kvp.Value);

            WriteNewLine();
        }
    }

    public static void WriteLines(IEnumerable<KeyValuePair<string, ConsoleColor>> messages)
    {
        lock (Lock)
        {
            foreach (var kvp in messages)
            {
                WriteTimeStamp();
                Write($"{kvp.Key}", kvp.Value);
                WriteNewLine();
            }
        }
    }

    private static void WriteNewLine() => Write(Environment.NewLine);

    private static void WriteTimeStamp()
    {
        var date = DateTime.Now.ToString("hh:mm:ss");
        Write($"[{date}] ", ConsoleColor.DarkGreen);
    }

    private static void Write(string message, ConsoleColor color = ConsoleColor.Gray)
    {
        if (Console.BackgroundColor != color)
        {
            Console.ForegroundColor = color;
        }

        Console.Write(Localizer.DoStr(message));

        if (Console.BackgroundColor != ConsoleColor.Gray)
        {
            Console.ResetColor();
        }
    }
}