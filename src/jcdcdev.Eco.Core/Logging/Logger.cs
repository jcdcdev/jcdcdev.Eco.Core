using Eco.Shared.Localization;
using jcdcdev.Eco.Core.Extensions;

namespace jcdcdev.Eco.Core.Logging;

public static class Logger
{
    public static readonly object Lock = new();

    public static void WriteLine(string message, ConsoleColor color = ConsoleColor.White)
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

    private static void Write(string message, ConsoleColor color = ConsoleColor.White)
    {
        if (Console.BackgroundColor != color)
        {
            Console.ForegroundColor = color;
        }

        Console.Write(Localizer.DoStr(message));

        if (Console.BackgroundColor != ConsoleColor.White)
        {
            Console.ResetColor();
        }
    }

    public static void Log(this ColorLogBuilder builder, string? prefix = null, ConsoleColor color = ConsoleColor.DarkCyan)
    {
        var bvalu = builder.Build();
        var signleLineOnly = bvalu.All(x => !x.NewLine);
        lock (Lock)
        {
            if (signleLineOnly)
            {
                WriteTimeStamp();
                if (prefix.IsNotNullOrWhitespace())
                {
                    Write($"[{prefix}] ", color);
                }
            }

            foreach (var token in bvalu)
            {
                if (token.NewLine)
                {
                    WriteTimeStamp();
                    if (prefix.IsNotNullOrWhitespace())
                    {
                        Write($"[{prefix}] ", color);
                    }
                }

                Write($"{token.Text}", token.Color);
                if (token.NewLine)
                {
                    WriteNewLine();
                }
            }

            if (signleLineOnly)
            {
                WriteNewLine();
            }

            builder.Clear();
        }
    }
}