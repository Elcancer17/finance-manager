using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.PropertyGrid.ViewModels;
using FinanceManager.Domain;
using System;
using System.ComponentModel;

namespace FinanceManager;

public partial class FinancialTransactionWindow : Window
{
    private FinancialTransaction _transaction;
    /// <summary>
    /// You should probably pass a copy in parameter otherwise the cancel won't cancel
    /// </summary>
    public FinancialTransactionWindow(string title, FinancialTransaction transaction)
    {
        _transaction = transaction;
        InitializeComponent();
        this.Title = title;
        Width = 800;
        Height = 400;
        pgDisplay.DataContext = new Display(_transaction);
    }

    private void Window_Closing(object sender, WindowClosingEventArgs e)
    {
        //possible to prevent closing here
        //with e.Cancel = true;
        //but async code will not work easily
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        this.Close(false);
    }

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
        //do validations or formating here
        this.Close(true);
    }

    private class Display
    {
        public enum TransactionType
        {
            Debit,
            Credit
        }
        private FinancialTransaction _transaction;
        public Display(FinancialTransaction transaction)
        {
            _transaction = transaction;
        }

        [ReadOnly(true)]
        public string Institution
        {
            get => _transaction.FinancialInstitution;
        }
        [ReadOnly(true)]
        [DisplayName("Compte")]
        public long Account
        {
            get => _transaction.AccountNumber;
        }
        public TransactionType Type
        {
            get
            {
                return Enum.Parse<TransactionType>(_transaction.TransactionType, true);
            }
            set => _transaction.FinancialInstitutionType = value.ToString().ToUpper();
        }
        [DisplayName("Date Transaction")]
        public DateTime TransactionDate
        {
            get => _transaction.TimeStamp;
            set => _transaction.TimeStamp = value;
        }
        [DisplayName("Montant")]
        public decimal Amount
        {
            get => _transaction.Value;
            set => _transaction.Value = value;
        }
        [DisplayName("Validé")]
        public bool IsValidated
        {
            get => _transaction.IsValidated;
            set => _transaction.IsValidated = value;
        }
    }
}