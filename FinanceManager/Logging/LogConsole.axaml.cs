using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using DynamicData;
using System;
using System.Diagnostics;

namespace FinanceManager.Logging;

public partial class LogConsole : UserControl
{
    private const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss.fff";
    private const int MAX_LINE_COUNT = 500;

    private DateTime initialTime = DateTime.Now;
    private Stopwatch stopwatch = Stopwatch.StartNew();
    private LogConsoleTraceListener traceListener;
    public LogConsole()
    {
        InitializeComponent();
        stbLogs.Inlines = new InlineCollection();
        traceListener = new LogConsoleTraceListener(this);
    }

    private void UserControl_DataContextChanged(object sender, EventArgs e)
    {
        if(DataContext == null && Trace.Listeners.Contains(traceListener))
        {
            Trace.Listeners.Remove(traceListener);
        }
        else if (!Trace.Listeners.Contains(traceListener))
        {
            Trace.Listeners.Add(traceListener);
        }
    }

    private DateTime CalculateCurrentTime()
    {
        return initialTime + stopwatch.Elapsed;
    }

    private Inline CreateText(string text, IBrush color)
    {
        return new Run(text)
        {
            Foreground = color,
        };
    }

    private Inline CreateBoldText(string text, IBrush color)
    {
        return new Bold()
        {
            Inlines = [CreateText(text, color)]
        };
    }

    public void LogMessage(string message, IImmutableSolidColorBrush color)
    {
        DateTime currentTime = CalculateCurrentTime();
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            if (stbLogs.Inlines.Count > MAX_LINE_COUNT)
            {
                stbLogs.Inlines.Clear();
            }
            stbLogs.Inlines.AddRange([
                CreateBoldText($"{currentTime.ToString(DATE_TIME_FORMAT)} - ", Brushes.Black),
                CreateText(message, color),
                new LineBreak()
            ]);
        }, DispatcherPriority.Normal);
        svLogs.ScrollToEnd();
    }

    public void ClearLogs()
    {
        Dispatcher.UIThread.InvokeAsync(stbLogs.Inlines.Clear, DispatcherPriority.Normal);
    }
}
