using Avalonia.Controls;
using Avalonia.Interactivity;
using FinanceManager.Utils;

namespace FinanceManager.Views;

public partial class LogConsoleView : UserControl
{
    public LogConsoleView()
    {
        InitializeComponent();
        lcLogs.AddDragDropHandler(DragDropExtensions.Drop);
    }

    private void btnClear_Click(object sender, RoutedEventArgs e)
    {
        lcLogs.ClearLogs();
    }

    public void ClearLogs() {
        lcLogs.ClearLogs();
    }
}