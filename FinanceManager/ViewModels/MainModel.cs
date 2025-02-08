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
using Tmds.DBus.Protocol;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinanceManager.ViewModels
{
    public class MainModel : ReactiveUI.ReactiveObject
    {
        public MainModel()
        {
            LoadData();
            if (FinancialData.Count() == 0)
            {
                CreateDummyData();
            }
        }

        public MainModel(List<FinancialTransaction> listeFTM)
        {
            LoadData(listeFTM);
            if (FinancialData.Count() == 0)
            {
                CreateDummyData();
            }
        }
        public SettingModel Settings { get; set; } = new();
        public FinanceModel Finance { get; set; } = new();


        public ObservableCollection<FinancialDisplayLine> FinancialData { get; set; } = new();

        private List<FinancialTransaction> GenerateDummyAccounts(DateTime date, Random random)
        {
            string[] DUMMY_FINANCIAL_INSTITUTIONS = ["Desjardins", "Scocia", "test1", "test2"];

            List<FinancialTransaction> financialTransactions = new();
            for (int i = 0; i < DUMMY_FINANCIAL_INSTITUTIONS.Length; i++)
            {
                decimal value = (decimal)Math.Round(random.NextDouble() * 1000 * random.Next(-1, 2), 2);
                financialTransactions.Add(new FinancialTransaction()
                {
                    TimeStamp = date,
                    FinancialInstitution = DUMMY_FINANCIAL_INSTITUTIONS[i],
                    Value = value
                });
            }

            return financialTransactions;
        }

        private bool HeadsOrTail(Random random)
        {
            if (random.Next(0, 1) == 0)
                return true;
            else
                return false;
        }
        public void CreateDummyData()
        {
            FinancialData.Clear();
            Random random = new Random(0);
            const int NUMBER_OF_DAYS = 50;
            const int MAXIMUM_NUMBER_OF_LINE_PER_DAY = 5;
            DateTime startingDate = DateTime.Now.AddDays(-NUMBER_OF_DAYS).Date;

            for (int i = 0; i < NUMBER_OF_DAYS; i++)
            {
                for (int j = 0; j < MAXIMUM_NUMBER_OF_LINE_PER_DAY; j++)
                {
                    DateTime date = startingDate.AddDays(i);
                    FinancialData.Add(new FinancialDisplayLine()
                    {
                        TimeStamp = date,
                        Accounts = GenerateDummyAccounts(date, random)
                    });
                    if (HeadsOrTail(random))
                    {
                        break;
                    }
                }
            }
        }
        public void LoadData()
        {
            FinancialTransactionManager ftm = new FinancialTransactionManager();
            LoadData(ftm.Load());
        }

        public void LoadData(List<FinancialTransaction> financialTransactions)
        {
            if (financialTransactions.Count() > 0)
            {
                FinancialData.Clear();
                List<IGrouping<long, FinancialTransaction>> listAccount = financialTransactions.GroupBy(p => p.AccountNumber).ToList();
                IGrouping<long, FinancialTransaction> item = listAccount[0];
                for (int g = 0; g < listAccount.Count(); g++)
                {
                    financialTransactions = listAccount[g].ToList();
                    financialTransactions = financialTransactions.OrderByDescending(p => p.TimeStamp).ThenBy(s => s.TimeStamp).ToList();
                    for (int i = 0; i < financialTransactions.Count(); i++)
                    {
                        FinancialData.Add(new FinancialDisplayLine()
                        {
                            TimeStamp = financialTransactions[i].TimeStamp,
                            Accounts = new List<FinancialTransaction>() { financialTransactions[i] }
                        });
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
