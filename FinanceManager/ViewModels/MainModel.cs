using FinanceManager.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.ViewModels
{
    public class MainModel : ReactiveUI.ReactiveObject
    {
        public MainModel()
        {
            CreateDummyData();
        }
        public SettingModel Settings { get; set; } = new();
        public ObservableCollection<FinancialDisplayLine> FinancialData { get; } = new();

        private FinancialTransaction GenerateDummyTransaction(DateTime date, Random random)
        {
            return new FinancialTransaction()
            {
                TimeStamp = date,
                Value = (decimal)Math.Round(random.NextDouble() * 1000, 2)
            };
        }
        private bool HeadsOrTail(Random random)
        {
            if (random.Next(0,1) == 0) 
                return true;
            else
                return true;
        }
        private void CreateDummyData()
        {
            Random random = new Random(0);
            const int NUMBER_OF_DAYS = 50;
            const int MAXIMUM_NUMBER_OF_LINE_PER_DAY = 5;
            DateTime startingDate = DateTime.Now.AddDays(-NUMBER_OF_DAYS).Date;

            for(int i = 0; i < NUMBER_OF_DAYS; i++)
            {
                for (int j = 0; j < MAXIMUM_NUMBER_OF_LINE_PER_DAY; j++)
                {
                    DateTime date = startingDate.AddDays(i);
                    FinancialData.Add(new FinancialDisplayLine()
                    {
                        TimeStamp = date,
                        Desjardins = GenerateDummyTransaction(date, random),
                        Scocia = GenerateDummyTransaction(date, random),
                    });
                    if (HeadsOrTail(random))
                    {
                        break;
                    }
                }
            }
        }
    }
}
