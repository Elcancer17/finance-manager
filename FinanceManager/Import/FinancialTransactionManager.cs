using FinanceManager.Domain;
using FinanceManager.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace FinanceManager.Import
{
    public class FinancialTransactionManager
    {
        public const string FINANCIAL_TRANSACTION_FILENAME = "FiancialTransaction_{0}.json";

        public FileInfo fileProps { get; set; }

        public FinancialTransactionManager()
        {
            fileProps = GetFile(DateTime.Now);
            Trace.WriteLine(string.Format("Processing {0} file: {1}",
                                          fileProps.Extension.ToUpper().Replace(".", ""),
                                          fileProps.FullName),
                            LogLevel.Information.ToString());
        }

        public FinancialTransactionManager(DateTime dt)
        {
            fileProps = GetFile(dt);
            Trace.WriteLine(string.Format("Processing {0} file: {1}",
                                          fileProps.Extension.ToUpper().Replace(".", ""),
                                          fileProps.FullName),
                            LogLevel.Information.ToString());
        }

        public List<FinancialTransaction> Load()
        {
            return JsonManager.LoadJson<List<FinancialTransaction>>(fileProps.FullName);
        }

        public void Save(List<FinancialTransaction> obj)
        {
            JsonManager.SaveToJson(fileProps.FullName, obj);
        }

        private FileInfo GetFile(DateTime dt)
        {
            FileInfo location = new FileInfo(Assembly.GetExecutingAssembly().Location);
            string filename = string.Format(FINANCIAL_TRANSACTION_FILENAME, GetYYYYMM(dt));
            string fullname = Path.Combine(location.DirectoryName, filename);
            return new FileInfo(fullname);
        }

        private string GetYYYYMM(DateTime dt)
        {
            char zero = '0';
            return dt.Year.ToString() + dt.Month.ToString().PadLeft(2, zero);
        }
    }
}
