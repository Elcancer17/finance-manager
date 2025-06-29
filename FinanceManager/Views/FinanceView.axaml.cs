using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Selection;
using Avalonia.Data;
using Avalonia.Interactivity;
using FinanceManager.Domain;
using FinanceManager.Import;
using FinanceManager.TreeDataGridAddons;
using FinanceManager.Utils;
using FinanceManager.ViewModels;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace FinanceManager.Views;

public partial class FinanceView : UserControl
{
    //private ThePopup thePopup;
    //private Popup popup;
    private MainModel _model;
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

            _model = model;
            model.Finance.SelectedYearChanged += OnSelectedYearChanged;
            model.Finance.SelectedMonthChanged += OnSelectedMonthChanged;
            model.Finance.SelectedAccountChanged += OnSelectedAccountChanged;
            model.Finance.FinancialData.CollectionChanged += FinancialData_CollectionChanged;

            /*
            <DataGridTextColumn Header="Compte" Binding="{Binding Path=Transaction.FinancialInstitution}" IsReadOnly="False"/>
			<DataGridTextColumn Header="Compte" Binding="{Binding Path=Transaction.AccountNumber}" IsReadOnly="False"/>
			<DataGridTextColumn Header="Date" Binding="{Binding Path=Transaction.TimeStamp, StringFormat='yyyy-MM-dd'}" IsReadOnly="True"/>
			<DataGridTextColumn Header="Montant" Binding="{Binding Path=Transaction.Value, StringFormat='c'}" IsReadOnly="True"/>
			<DataGridTextColumn Header="Description" Binding="{Binding Path=Transaction.Description}" IsReadOnly="True"/>
			<DataGridCheckBoxColumn Header="Validé" Binding="{Binding Path=Transaction.IsValidated}" IsReadOnly="False"/>
			<DataGridTextColumn Header="Total" Binding="{Binding Path=Total}" IsReadOnly="True"/>
			<DataGridTemplateColumn>
				<DataGridTemplateColumn.CellTemplate>
					<DataTemplate>
						<Button Content="Edit" Tag="{Binding}" Click="btnEdit_Click"/>
					</DataTemplate>
				</DataGridTemplateColumn.CellTemplate>
			</DataGridTemplateColumn>
            */
            FlatTreeDataGridSource<FinancialTransactionDisplay> source = new FlatTreeDataGridSource<FinancialTransactionDisplay>(_model.Finance.FinancialData)
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
                    new CustomCheckBoxColumn<FinancialTransactionDisplay>(
                        "Validé",
                        new LBinding<FinancialTransactionDisplay, bool>(x => x.Transaction.IsValidated)
                        {
                            Mode = BindingMode.TwoWay,
                        }
                    ).Compile(),
                    new CustomTextColumn<FinancialTransactionDisplay, decimal>(
                        "Total",
                        new LBinding<FinancialTransactionDisplay, decimal>(x => x.Total)
                        {
                            Mode = BindingMode.OneWay,
                        }
                    ).Compile(),

                },
            };
            dgFinancialData.Source = source;
            dgFinancialData.Source.Selection = new TreeDataGridCellSelectionModel<FinancialTransactionDisplay>(source)
            {
                SingleSelect = false
            };
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
            _model.LoadFinancialData(account.AccountNumber);
        }
    }

    private void FinancialData_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        _model.Finance.CalculateFinancialDataTotals();
    }


    private async void btnAdd_Click(object sender, RoutedEventArgs e)
    {
        FinancialTransaction newTransaction = new();
        Window ownerWindow = (Window)TopLevel.GetTopLevel(this);

        FinancialTransactionWindow window = new("Ajouter Transaction", newTransaction);
        bool savedNewTransaction = await window.ShowDialog<bool>(ownerWindow);
        if (savedNewTransaction)
        {
            //do add;
        }
    }

    private async void btnEdit_Click(object sender, RoutedEventArgs e)
    {
        IReadOnlyList<FinancialTransactionDisplay> selection = dgFinancialData.GetSelection<FinancialTransactionDisplay>();
        Window ownerWindow = (Window)TopLevel.GetTopLevel(this);

        for (int i = 0; i < selection.Count; i++)
        {
            FinancialTransaction transactionCopy = new();
            selection[i].Transaction.CopyTo(transactionCopy);

            FinancialTransactionWindow window = new("Modifier Transaction", transactionCopy);
            bool savedEdit = await window.ShowDialog<bool>(ownerWindow);
            if(savedEdit)
            {
                //copy edited transaction
                //example transactionCopy.CopyTo(selection[i].Transaction);
            }
        }
    }

    private async void btnDelete_Click(object sender, RoutedEventArgs e)
    {
        IReadOnlyList<FinancialTransactionDisplay> selection = dgFinancialData.GetSelection<FinancialTransactionDisplay>();
        var messageBox = MessageBoxManager.GetMessageBoxStandard("Confirmation",
            $"Es-tu sure de vouloir supprimer {selection.Count} transactions?",
            ButtonEnum.YesNo,
            Icon.Warning);
        Window ownerWindow = (Window)TopLevel.GetTopLevel(this);
        if(await messageBox.ShowWindowDialogAsync(ownerWindow) == ButtonResult.Yes)
        {
            //do delete
        }
    }

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {

    }
}