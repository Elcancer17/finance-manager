using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FinanceManager.Logging;

public enum LogLevel
{
    None = 0,
    Information,
    Warning,
    Error,
    Critical,
}

public class LogConsoleTraceListener : TraceListener
{
    private readonly static Dictionary<LogLevel, IImmutableSolidColorBrush> LOG_LEVEL_COLORS = new()
    {
        { LogLevel.None, Brushes.Black },
        { LogLevel.Information, Brushes.Blue },
        { LogLevel.Warning, Brushes.Orange },
        { LogLevel.Error, Brushes.Tomato },
        { LogLevel.Critical, Brushes.Red },
    };
    private readonly static LogLevel[] LOG_LEVELS = Enum.GetValues<LogLevel>();
    private LogConsole logConsole;

    public LogConsoleTraceListener(LogConsole logConsole)
    {
        this.logConsole = logConsole;
    }

    public override void Write(string message)
    {
        WriteLine(message);
    }

    public override void WriteLine(string message)
    {
        LogLevel logLevel = LOG_LEVELS.FirstOrDefault(x => message.StartsWith(x.ToString()));
        logConsole.LogMessage(message, LOG_LEVEL_COLORS[logLevel]);
    }
}