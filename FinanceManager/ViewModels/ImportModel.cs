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
        public ObservableCollection<FinancialTransactionDisplay> ImportedData { get; } = new();
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
