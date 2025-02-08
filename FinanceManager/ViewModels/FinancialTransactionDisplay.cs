using FinanceManager.Domain;

namespace FinanceManager.ViewModels
{
    public class FinancialTransactionDisplay
    {
        public FinancialTransactionDisplay(FinancialTransaction transaction)
        {
            Transaction = transaction;
        }

        public FinancialTransaction Transaction { get; }

        public decimal Total { get; set; }
    }
}
