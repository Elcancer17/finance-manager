using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FinanceManager.Domain
{
    public class FinancialTransaction
    {
        public DateTime TimeStamp { get; set; }
        public string FinancialInstitution { get; set; }
        public decimal Value { get; set; }
        public bool IsValidated { get; set; }
    }
}
