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
    List<FinancialTransaction> financialTransactions;
    FinancialTransactionManager financialTransactionManager = new FinancialTransactionManager();

    private MainModel Model => DataContext as MainModel;
    public ImportView()
    {
        InitializeComponent();
        dgImportedData.AddDragDropHandler(Drop);
    }

    private void Drop(object sender, DragEventArgs e)
    {
        lcLogsImport.ClearLogs();
        List<string> files = e.Data.GetFiles()?.Select(x => x.Path.LocalPath).ToList();
        if (files == null && files.Count > 0)//null whenever you didnt drop a file
        {
            return;
        }

        List<string> accountNumberList = new List<string>();

        Model.Import.Load();
        for (int i = 0; i < files.Count; i++)
        {
            Model.Import.financialTransactions = ImportManager.ImporFile(files[i], Model.Import.financialTransactions);
        }

        Model.LoadImportedData();
    }

    private void btnClearTransactions_Click(object sender, RoutedEventArgs e)
    {
        Model.Import.financialTransactions.Clear();
        Model.LoadImportedData();
    }


    private void btnClearLogs_Click(object sender, RoutedEventArgs e)
    {
        lcLogsImport.ClearLogs();
    }

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
        financialTransactionManager.Save(Model.Import.financialTransactions);
        Model.LoadFinancialData();
        Model.LoadAccounts();
        Model.Import.financialTransactions.Clear();
        Model.LoadImportedData();
        lcLogsImport.ClearLogs();
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