using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.PropertyGrid.Controls;
using FinanceManager.Domain;
using FinanceManager.Utils;
using FinanceManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinanceManager.Views;

public partial class FinanceView : UserControl
{
    private DataGridTextColumn timeStampColumn;
    public FinanceView()
    {
        InitializeComponent();
        dgFinancialData.AddDragDropHandler(Drop);
        timeStampColumn = (DataGridTextColumn)dgFinancialData.Columns.Single();
    }

    private void UserControl_DataContextChanged(object sender, EventArgs e)
    {
        if(DataContext is MainModel model)
        {
            GenerateColumns(model.FinancialData.First().Accounts.Select(x => x.FinancialInstitution).ToList());
        }
    }

    private void GenerateColumns(IReadOnlyList<string> financialInstitutions)
    {
        for(int i = dgFinancialData.Columns.Count-1; i > 0; i--)
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

    private void Drop(object sender, DragEventArgs e)
    {
        List<string> files = e.Data.GetFiles()?.Select(x => x.Path.LocalPath).ToList();
        if(files == null)//null whenever you didnt drop a file
        {
            return;
        }
        //do something here
    }
}