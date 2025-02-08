﻿using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace FinanceManager.Domain
{
    public class FinancialTransaction
    {
        public string QuickenId { get; set; }

        public string FinancialInstitution { get; set; }

        public long AccountNumber { get; set; }

        public string TransactionType { get; set; }

        public string Description { get; set; }

        public DateTime TimeStamp { get; set; }

        public decimal Value { get; set; }

        public bool IsValidated { get; set; }

        public string TransactionId { get; set; }

        [JsonIgnore]
        public string Message { get; set; }

        public void SetTransactionId()
        {
            TransactionId = string.Format("{0}{1}{2}{3}{4}",
                                          FinancialInstitution,
                                          AccountNumber,
                                          TransactionType.Replace("0",""),
                                          TimeStamp.Date.ToString().Substring(0,10),
                                          Value.ToString());
            TransactionId = SHA256ToString(TransactionId);
        }
        public static string SHA256ToString(string s)
        {
            using (var alg = SHA256.Create())
                return alg.ComputeHash(Encoding.UTF8.GetBytes(s)).Aggregate(new StringBuilder(), (sb, x) => sb.Append(x.ToString("x2"))).ToString();
        }
    }
}
