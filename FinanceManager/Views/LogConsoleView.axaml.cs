using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using FinanceManager.Import;
using FinanceManager.Utils;
using static System.Net.WebRequestMethods;
using System.Collections.Generic;
using System.Diagnostics;
using FinanceManager.Domain;
using System.Linq;
using FinanceManager.Logging;

namespace FinanceManager.Views;

public partial class LogConsoleView : UserControl
{
    List<FinancialTransaction> fts;
    FinancialTransactionManager ftm;

    public LogConsoleView()
    {
        InitializeComponent();
        lcLogs.AddDragDropHandler(Drop);
        ftm = new FinancialTransactionManager();
        fts = ftm.Load();
    }

    private void btnClear_Click(object sender, RoutedEventArgs e)
    {
        lcLogs.ClearLogs();
    }
    private void Drop(object sender, DragEventArgs e)
    {
        lcLogs.ClearLogs();
        List<string> files = e.Data.GetFiles()?.Select(x => x.Path.LocalPath).ToList();
        if (files == null)//null whenever you didnt drop a file
        {
            return;
        }
        //do something here
        for (int i = 0; i < files.Count; i++)
        {
            Trace.WriteLine(files[i], LogLevel.Information.ToString());
            ImportManager im = new ImportManager(files[i]);
            fts = im.ImporFile(fts);
        }
        //ftm.Save(fts);
    }
}