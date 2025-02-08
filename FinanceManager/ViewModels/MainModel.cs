using DynamicData.Binding;
using FinanceManager.Domain;
using FinanceManager.Import;
using FinanceManager.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.ViewModels
{
    public class MainModel : ReactiveUI.ReactiveObject
    {
        public MainModel()
        {
            LoadData();
        }

        public SettingModel Settings { get; set; } = new();
        public FinanceModel Finance { get; set; } = new();
        public ImportModel Import { get; set; } = new();

        public void LoadData()
        {
            FinancialTransactionManager ftm = new FinancialTransactionManager();
            LoadData(ftm.Load());
        }

        public void LoadData(List<FinancialTransaction> financialTransactions)
        {
            if (financialTransactions.Count() > 0)
            {
                Finance.FinancialData.Clear();
                List<IGrouping<long, FinancialTransaction>> listAccount = financialTransactions.GroupBy(p => p.AccountNumber).ToList();
                for (int g = 0; g < listAccount.Count(); g++)
                {
                    financialTransactions = listAccount[g].ToList();
                    financialTransactions = financialTransactions.OrderByDescending(p => p.TimeStamp).ThenBy(s => s.TimeStamp).ToList();
                    for (int i = 0; i < financialTransactions.Count(); i++)
                    {
                        Finance.FinancialData.Add(new FinancialTransactionDisplay(financialTransactions[i]));
                    }
                }
            }
        }

        public static void LogTransactions()
        {
            FinancialTransactionManager ftm = new FinancialTransactionManager();
            List<FinancialTransaction> listeFTM = ftm.Load();

            List<IGrouping<long, FinancialTransaction>> listGroup = listeFTM.GroupBy(p => p.AccountNumber).ToList();
            //IGrouping<long, FinancialTransaction> item = listGroup[6];
            //listeFTM = item.ToList();
            //listeFTM = listeFTM.OrderByDescending(p => p.TimeStamp).ThenByDescending(s => s.TimeStamp).ToList();
            //listeFTM = listeFTM.OrderByDescending(p => p.TimeStamp).ThenBy(s => s.TimeStamp).ToList();

            for (int i = 0; i < listGroup.Count(); i++)
            {
                IGrouping<long, FinancialTransaction> item = listGroup[i];
                List<FinancialTransaction> liste = item.ToList();
                liste = liste.OrderByDescending(p => p.TimeStamp).ThenByDescending(s => s.TimeStamp).ToList();

                for (int j = 0; j < liste.Count(); j++)
                {
                    Trace.WriteLine(string.Format("{0} | {1} | {2} | {3} | {4}",
                        liste[j].AccountNumber,
                        liste[j].TimeStamp,
                        liste[j].Value,
                        liste[j].TransactionType,
                        liste[j].Description));
                }
            }
        }

    }
}
