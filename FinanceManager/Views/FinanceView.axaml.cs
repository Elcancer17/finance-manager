using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using FinanceManager.Domain;
using FinanceManager.Import;
using FinanceManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace FinanceManager.Views;

public partial class FinanceView : UserControl
{
    //private ThePopup thePopup;
    //private Popup popup;
    public MainModel Model => DataContext as MainModel;
    public FinanceView()
    {
        InitializeComponent();
        //popup = this.FindControl<Popup>("Popup");
        //thePopup = this.FindControl<Popup>("ThePopup");
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
        if (account != null && account.AccountNumber > 0)
        {
            Model.LoadFinancialData(account.AccountNumber);
        }
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
    private void btnPopup_Click(object sender, RoutedEventArgs e)
    {
        //if (sender is Button button && button.Tag is FinancialTransactionDisplay editedElement)
        //{

        //}
    }
    private void OnPopupButton_Click(object sender, RoutedEventArgs e)
    {
        //if (popup.IsOpen)
        //{
        //    popup.IsOpen = false;
        //}
        //else
        //{
        //    popup.IsOpen = true;
        //}
    }
}