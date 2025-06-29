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
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Controls.Selection;
using FinanceManager.TreeDataGridAddons;
using System.Reactive.Linq;
using Avalonia.Data;

namespace FinanceManager.Views;

public partial class ImportView : UserControl
{
    private FinancialTransactionManager financialTransactionManager = new FinancialTransactionManager();
    private MainModel _model;
    public ImportView()
    {
        InitializeComponent();
        dgImportedData.AddDragDropHandler(Drop);
    }

    private void UserControl_DataContextChanged(object sender, EventArgs e)
    {
        if (DataContext is MainModel model)
        {
            model.Import.ImportedData.CollectionChanged -= ImportedData_CollectionChanged;
            _model = model;
            model.Import.ImportedData.CollectionChanged += ImportedData_CollectionChanged;

            FlatTreeDataGridSource<FinancialTransactionDisplay> source = new FlatTreeDataGridSource<FinancialTransactionDisplay>(_model.Import.ImportedData)
            {
                Columns =
                {
                    new CustomTextColumn<FinancialTransactionDisplay, string>(
                        "Institution",
                        new LBinding<FinancialTransactionDisplay, string>(x => x.Transaction.FinancialInstitution)
                        {
                            Mode = BindingMode.OneWay,
                        }
                    ).Compile(),
                    new CustomTextColumn<FinancialTransactionDisplay, long>(
                        "Compte",
                        new LBinding<FinancialTransactionDisplay, long>(x => x.Transaction.AccountNumber)
                        {
                            Mode = BindingMode.OneWay,
                        }
                    ).Compile(),
                    new CustomTextColumn<FinancialTransactionDisplay, DateTime>(
                        "Date",
                        new LBinding<FinancialTransactionDisplay, DateTime>(x => x.Transaction.TimeStamp)
                        {
                            Mode = BindingMode.OneWay,
                            StringFormat = "yyyy-MM-dd",
                        }
                    ).Compile(),
                    new CustomTextColumn<FinancialTransactionDisplay, decimal>(
                        "Montant",
                        new LBinding<FinancialTransactionDisplay, decimal>(x => x.Transaction.Value)
                        {
                            Mode = BindingMode.OneWay,
                            StringFormat = "c",
                        }
                    ).Compile(),
                    new CustomTextColumn<FinancialTransactionDisplay, string>(
                        "Description",
                        new LBinding<FinancialTransactionDisplay, string>(x => x.Transaction.Description)
                        {
                            Mode = BindingMode.OneWay,
                        }
                    ).Compile(),
                    new CustomTextColumn<FinancialTransactionDisplay, string>(
                        "Message",
                        new LBinding<FinancialTransactionDisplay, string>(x => x.Transaction.Message)
                        {
                            Mode = BindingMode.OneWay,
                        }
                    ).Compile(),
                },
            };
            dgImportedData.Source = source;
            dgImportedData.Source.Selection = new TreeDataGridCellSelectionModel<FinancialTransactionDisplay>(source)
            {
                SingleSelect = false
            };
        }
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

        _model.Import.Load();
        for (int i = 0; i < files.Count; i++)
        {
            _model.Import.financialTransactions = ImportManager.ImporFile(files[i], _model.Import.financialTransactions);
        }

        _model.LoadImportedData();
    }

    private void btnClearTransactions_Click(object sender, RoutedEventArgs e)
    {
        _model.Import.financialTransactions.Clear();
        _model.LoadImportedData();
    }

    private void btnClearLogs_Click(object sender, RoutedEventArgs e)
    {
        lcLogsImport.ClearLogs();
    }

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
        financialTransactionManager.Save(_model.Import.financialTransactions);
        _model.LoadFinancialData();
        _model.LoadAccounts();
        _model.Import.financialTransactions.Clear();
        _model.LoadImportedData();
        lcLogsImport.ClearLogs();
    }

    private void ImportedData_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        _model.Import.CalculateImportedDataTotals();
    }
}