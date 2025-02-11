using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using FinanceManager.Domain;
using FinanceManager.Utils;
using FinanceManager.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Linq;
using FinanceManager.Logging;
using FinanceManager.Import;
using System.Collections.Specialized;
using System.Reactive.Linq;

namespace FinanceManager.Views;

public partial class ImportView : UserControl
{
    List<FinancialTransaction> fts;
    FinancialTransactionManager ftm;

    private MainModel Model => DataContext as MainModel;
    public ImportView()
    {
        InitializeComponent();
        dgImportedData.AddDragDropHandler(Drop);
        ftm = new FinancialTransactionManager();
        fts = ftm.Load();
    }

    private void Drop(object sender, DragEventArgs e)
    {
        lcLogsImport.ClearLogs();
        List<string> files = e.Data.GetFiles()?.Select(x => x.Path.LocalPath).ToList();
        if (files == null)//null whenever you didnt drop a file
        {
            return;
        }

        for (int i = 0; i < files.Count; i++)
        {
            Trace.WriteLine(files[i], LogLevel.Information.ToString());
            ImportManager im = new ImportManager(files[i]);
            fts = im.ImporFile(fts);
        }
        Model.LoadImportedData(fts);
    }


    private void btnClearTransactions_Click(object sender, RoutedEventArgs e)
    {
        Model.Import.ImportedData.Clear();
        fts.Clear();
    }


    private void btnClearLogs_Click(object sender, RoutedEventArgs e)
    {
        lcLogsImport.ClearLogs();
    }

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
        ftm.Save(fts);
        Model.LoadFinancialData() ;
    }

    private void UserControl_DataContextChanged(object sender, EventArgs e)
    {
        if (DataContext is MainModel model)
        {
            model.Import.ImportedData.CollectionChanged -= ImportedData_CollectionChanged;

            model.Import.ImportedData.CollectionChanged += ImportedData_CollectionChanged;
        }
    }

    private void ImportedData_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        Model.Import.CalculateImportedDataTotals();
    }
}