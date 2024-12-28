using FinanceManager.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.ViewModels
{
    public class FinancialDisplayLine
    {
        public DateTime TimeStamp { get; set; }
        public FinancialTransaction Desjardins { get; set; }
        public FinancialTransaction Scocia { get; set; }

    }
}
