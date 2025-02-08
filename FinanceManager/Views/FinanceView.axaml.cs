using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.PropertyGrid.Controls;
using FinanceManager.Domain;
using FinanceManager.Logging;
using FinanceManager.Utils;
using FinanceManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;

namespace FinanceManager.Views;

public partial class FinanceView : UserControl
{
    public MainModel Model => DataContext as MainModel;
    public FinanceView()
    {
        InitializeComponent();
    }

    private void UserControl_DataContextChanged(object sender, EventArgs e)
    {
        if(DataContext is MainModel model)
        {
            model.Finance.SelectedYearChanged -= OnSelectedYearChanged;
            model.Finance.SelectedMonthChanged -= OnSelectedMonthChanged;
            model.Finance.SelectedAccountChanged -= OnSelectedAccountChanged;
            model.Finance.FinancialData.CollectionChanged -= FinancialData_CollectionChanged;

            model.Finance.SelectedYearChanged += OnSelectedYearChanged;
            model.Finance.SelectedMonthChanged += OnSelectedMonthChanged;
            model.Finance.SelectedAccountChanged += OnSelectedAccountChanged;
            model.Finance.FinancialData.CollectionChanged += FinancialData_CollectionChanged;
        }
    }

    private void OnSelectedYearChanged(object sender, int year)
    {

    }

    private void OnSelectedMonthChanged(object sender, int month)
    {

    }

    private void OnSelectedAccountChanged(object sender, AccountDisplay account)
    {

    }

    private void FinancialData_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        Model.Finance.CalculateFinancialDataTotals();
    }


    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
    
    }

    private void btnDelete_Click(object sender, RoutedEventArgs e)
    {

    }

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {

    }

    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
        if(sender is Button button && button.Tag is FinancialTransactionDisplay editedElement)
        {

        }
    }
}