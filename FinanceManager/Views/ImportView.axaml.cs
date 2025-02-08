using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using FinanceManager.Domain;
using FinanceManager.Utils;
using FinanceManager.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Linq;
using FinanceManager.Logging;
using FinanceManager.Import;
using static System.Net.WebRequestMethods;
using DynamicData;

namespace FinanceManager.Views;

public partial class ImportView : UserControl
{
    private DataGridTextColumn timeStampColumn;
    List<FinancialTransaction> fts;
    FinancialTransactionManager ftm;

    private MainModel Model => DataContext as MainModel;
    public ImportView()
    {
        InitializeComponent();
        dgFinancialData.AddDragDropHandler(Drop);
        //lcLogs.AddDragDropHandler(DragDropExtensions.Drop);
        timeStampColumn = (DataGridTextColumn)dgFinancialData.Columns.Single();
        ftm = new FinancialTransactionManager();
        fts = ftm.Load();
    }

    private void Drop(object sender, DragEventArgs e)
    {
        //lcLogs.ClearLogs();
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
        //Model.CreateDummyData();
        Model.LoadData(fts);
    }

    //Private void MainUserControl_GotFocus(ByVal sender as Object, ByVal e as EventArgs) Handles Me.GotFocus
    //MessageBox.Show("got focus")
    //End Sub

    //private void Focus(object sender, RoutedEventArgs e)
    //{
    //    string aaa = "";
    //}

    private void btnClear_Click(object sender, RoutedEventArgs e)
    {
        //lcLogs.ClearLogs();
    }
    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
        ftm.Save(fts);
    }

    private void UserControl_DataContextChanged(object sender, EventArgs e)
    {
        if (DataContext is MainModel model)
        {
            GenerateColumns(model.FinancialData.First().Accounts.Select(x => x.FinancialInstitution).ToList());
        }
    }

    private void GenerateColumns(IReadOnlyList<string> financialInstitutions)
    {
        for (int i = dgFinancialData.Columns.Count - 1; i > 0; i--)
        {
            if (dgFinancialData.Columns[i] != timeStampColumn)
            {
                dgFinancialData.Columns.RemoveAt(i);
            }
        }
        for (int i = 0; i < financialInstitutions.Count; i++)
        {
            DataGridColoredTextColumn<decimal> valueColumn = new()
            {
                Header = financialInstitutions[i],
                Width = DataGridLength.Auto,
                Binding = new Binding($"{nameof(FinancialDisplayLine.Accounts)}[{i}].{nameof(FinancialTransaction.Value)}"),
                ColorFunc = SelectColor,
                IsReadOnly = true,
            };
            dgFinancialData.Columns.Add(valueColumn);
            DataGridCheckBoxColumn isValidatedColumn = new()
            {
                Header = "Is Validated",
                Binding = new Binding($"{nameof(FinancialDisplayLine.Accounts)}[{i}].{nameof(FinancialTransaction.IsValidated)}")
            };
            dgFinancialData.Columns.Add(isValidatedColumn);
        }
    }

    private IBrush SelectColor(decimal value)
    {
        if (value >= 0)
        {
            return Brushes.LightGreen;
        }
        else
        {
            return Brushes.OrangeRed;
        }
    }
}