using FinanceManager.Domain;
using static FinanceManager.Domain.Quicken;

namespace FinanceManager.Import.Extension
{
    public static class STMTTRNExtensions
    {
        public static FinancialTransaction MapSTMTTRNToFinancialTransaction(this STMTTRN item, string financialInstitution, string accountNumber)
        {
            FinancialTransaction ft = new FinancialTransaction();
            ft.FinancialInstitution = financialInstitution;
            ft.AccountNumber = accountNumber.ToLong();
            //string accountNumberTemp = accountNumber.Substring(0, 4) + accountNumber.Substring(accountNumber.Length-4, 4);
            //ft.AccountNumber = accountNumberTemp.ToLong();
            ft.TimeStamp = item.DTPOSTED.ToDate();
            ft.TransactionType = item.TRNTYPE.AdjustSpecialCaracters().ToUpper();
            ft.Description = item.NAME.AdjustSpecialCaracters();
            ft.Value = item.TRNAMT.ToDecimal();
            ft.QuickenId = item.FITID;
            ft.SetTransactionId();
            return ft;
        }
    }
}
