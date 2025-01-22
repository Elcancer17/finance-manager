using FinanceManager.Domain;
using FinanceManager.Import;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinanceManager.ViewModels
{
    public class MainModel : ReactiveUI.ReactiveObject
    {
        public MainModel()
        {
            //CreateDummyData();
            LoadData();
        }
        public SettingModel Settings { get; set; } = new();
        public ObservableCollection<FinancialDisplayLine> FinancialData { get; } = new();

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
        private void CreateDummyData()
        {
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
        private void LoadData()
        {
            FinancialTransactionManager ftm = new FinancialTransactionManager();
            List<FinancialTransaction> listeFT = ftm.Load();
            for (int i = 0; i < listeFT.Count(); i++)
            {
                FinancialData.Add(new FinancialDisplayLine()
                {
                    TimeStamp = listeFT[i].TimeStamp,
                    Accounts = new List<FinancialTransaction>() { listeFT[i] }
                });
            }
        }
    }
}
