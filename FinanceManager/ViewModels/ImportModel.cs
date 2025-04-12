using FinanceManager.Domain;
using FinanceManager.Import;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.ViewModels
{
    public class ImportModel : ReactiveUI.ReactiveObject
    {
        public List<FinancialTransaction> financialTransactions { get; set; }
        FinancialTransactionManager financialTransactionManager = new FinancialTransactionManager();

        public ObservableCollection<FinancialTransactionDisplay> ImportedData { get; } = new();

        public void Load()
        {
            financialTransactions = financialTransactionManager.Load();
        }

        public void CalculateImportedDataTotals()
        {
            decimal currentTotal = 0;
            for (int i = 0; i < ImportedData.Count; i++)
            {
                currentTotal += ImportedData[i].Transaction.Value;
                ImportedData[i].Total = currentTotal;
            }
        }
    }
}
