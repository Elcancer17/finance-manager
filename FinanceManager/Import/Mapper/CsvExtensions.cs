using FinanceManager.Domain;

namespace FinanceManager.Import.Extension
{
    public static class CsvExtensions
    {
        public static FinancialTransaction MapCsvToFinancialTransaction(this Csv item, string financialInstitution)
        {
            FinancialTransaction ft = new FinancialTransaction();
            ft.FinancialInstitution = financialInstitution;
            //if (!string.IsNullOrEmpty(item.Compte) && !string.IsNullOrEmpty(item.CompteType)) { ft.AccountNumber = string.Format("{0}-{1}", item.Compte, item.CompteType); }
            //else if (!string.IsNullOrEmpty(item.CC)) { ft.AccountNumber = item.CC; }
            //else if (!string.IsNullOrEmpty(accountNumber)) { ft.AccountNumber = accountNumber; }
            //else { ft.AccountNumber = item.}
            ft.AccountNumber = item.Compte.ToLong();
            //ft.AccountNumber = item.Compte.Replace("*","").Replace("0","").ToLong();
            ft.TimeStamp = item.TransactionDate.ToDate();
            ft.TransactionType = item.TransactionType.AdjustSpecialCaracters().ToUpper();
            ft.Description = string.Format("{0} {1}", item.Description, item.SubDescription).Trim();
            ft.Description = ft.Description.AdjustSpecialCaracters();
            ft.Value = item.Amount.ToDecimal();
            ft.SetTransactionId();
            return ft;
        }
    }
}
