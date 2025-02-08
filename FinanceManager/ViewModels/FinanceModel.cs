using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.ViewModels
{
    public class FinanceModel : ReactiveUI.ReactiveObject
    {
        public EventHandler<int> SelectedYearChanged;
        public EventHandler<int> SelectedMonthChanged;
        public EventHandler<AccountDisplay> SelectedAccountChanged;


        private int selectedYear = DateTime.Now.Year;
        public int SelectedYear 
        {
            get => selectedYear;
            set
            {
                selectedYear = value;
                SelectedYearChanged?.Invoke(this, value);
            }
        }
        public ObservableCollection<int> AvailableYears { get; } = [DateTime.Now.Year - 1, DateTime.Now.Year, DateTime.Now.Year + 1];

        private int selectedMonth;
        public int SelectedMonth
        {
            get => selectedMonth;
            set
            {
                selectedMonth = value;
                SelectedMonthChanged?.Invoke(this, value);
            }
        }
        public ObservableCollection<int> AvailableMonths { get; } = [1, 2, 3, 4, 5, 6];


        private AccountDisplay selectedAccount;
        public AccountDisplay SelectedAccount
        {
            get => selectedAccount;
            set
            {
                selectedAccount = value;
                SelectedAccountChanged?.Invoke(this, value);
            }
        }
        public ObservableCollection<AccountDisplay> Accounts { get; } = [
             new AccountDisplay(){
                 AccountName = "Compte #1",
                 FinancialInstitution = "Desjardins"
             },
            new AccountDisplay(){
                 AccountName = "Compte #2",
                 FinancialInstitution = "Desjardins"
             }
            ];


        public ObservableCollection<FinancialTransactionDisplay> FinancialData { get; } = new();
        public void CalculateFinancialDataTotals()
        {
            decimal currentTotal = 0;
            for(int i = 0; i < FinancialData.Count; i++)
            {
                currentTotal += FinancialData[i].Transaction.Value;
                FinancialData[i].Total = currentTotal;
            }
        }
    }
}
