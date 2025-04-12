using Avalonia.Controls;
using FinanceManager.Domain;
using FinanceManager.Import;
using FinanceManager.Import.Extension;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FinanceManager.ViewModels
{
    public class MainModel : ReactiveUI.ReactiveObject
    {
        List<FinancialTransaction> financialTransactions;
        FinancialTransactionManager financialTransactionManager = new FinancialTransactionManager();

        public MainModel()
        {
            Load();
        }

        public SettingModel Settings { get; set; } = new();
        public FinanceModel Finance { get; set; } = new();
        public ImportModel Import { get; set; } = new();

        public void Load()
        {
            LoadFinancialData();
            LoadAccounts();
        }

        public void LoadFinancialData()
        {
            financialTransactions = financialTransactionManager.Load();
            LoadFinancialData(financialTransactionManager.Load());
        }

        public void LoadFinancialData(long accountNumber)
        {
            financialTransactions = financialTransactionManager.Load();
            financialTransactions = financialTransactions.Where(p => p.AccountNumber == accountNumber).ToList();
            Finance.FinancialData.Clear();
            if (financialTransactions.Count() > 0)
            {
                financialTransactions = financialTransactions.OrderBy(p => p.FinancialInstitution).ThenBy(s => s.AccountNumber).ThenBy(s => s.TimeStamp).ToList();
                for (int i = 0; i < financialTransactions.Count(); i++)
                {
                    Finance.FinancialData.Add(new FinancialTransactionDisplay(financialTransactions[i]));
                }
            }
        }

        public void LoadFinancialData(List<FinancialTransaction> financialTransactions)
        {
            Finance.FinancialData.Clear();
            if (financialTransactions.Count() > 0)
            {
                financialTransactions = financialTransactions.OrderBy(p => p.FinancialInstitution).ThenBy(s => s.AccountNumber).ThenBy(s => s.TimeStamp).ToList();
                for (int i = 0; i < financialTransactions.Count(); i++)
                {
                    Finance.FinancialData.Add(new FinancialTransactionDisplay(financialTransactions[i]));
                }
            }
        }

        public void LoadAccounts()
        {
            Finance.Accounts.Clear();
            foreach (var item in financialTransactions.GroupBy(p => new { p.FinancialInstitution, p.FinancialInstitutionType, p.AccountNumber })
                                                       .Select(p => new { p.Key.FinancialInstitution, p.Key.FinancialInstitutionType, p.Key.AccountNumber }))
            {
                Finance.Accounts.Add(new AccountDisplay() { FinancialInstitution = item.FinancialInstitution,
                                                            FinancialInstitutionType = item.FinancialInstitutionType,
                                                            AccountNumber = item.AccountNumber });
            };
        }

        public void LoadAccounts(List<FileDefinition> definitionList)
        {
            Finance.Accounts.Clear();
            if (definitionList.Count() > 0)
            {
                definitionList = definitionList.OrderBy(p => p.Order).ThenBy(s => s.DefinitionType).ThenBy(s => s.Compte).ToList();
                for (int i = 0; i < definitionList.Count(); i++)
                {
                    Finance.Accounts.Add(new AccountDisplay() { FinancialInstitution = definitionList[i].DefinitionType, AccountNumber = definitionList[i].Compte.ToLong() });
                }
            }
        }
               
        public void LoadImportedData()
        {
            Import.ImportedData.Clear();
            if (Import.financialTransactions.Count() > 0)
            {
                Import.financialTransactions = Import.financialTransactions.OrderBy(p => p.FinancialInstitution).ThenBy(s => s.AccountNumber).ThenBy(s => s.TimeStamp).ToList();
                for (int i = 0; i < Import.financialTransactions.Count(); i++)
                {
                    Import.ImportedData.Add(new FinancialTransactionDisplay(Import.financialTransactions[i]));
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
