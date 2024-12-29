using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using FinanceManager.Logging;
using System.Diagnostics;

namespace FinanceManager.Views;

public partial class LogConsoleView : UserControl
{
    public LogConsoleView()
    {
        InitializeComponent();
    }

    private void btnClear_Click(object sender, RoutedEventArgs e)
    {
        lcLogs.ClearLogs();
    }
}