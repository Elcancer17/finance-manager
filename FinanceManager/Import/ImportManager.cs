using FinanceManager.Domain;
using FinanceManager.Logging;
using FinanceManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace FinanceManager.Import
{
    public static class ImportManager
    {
        public static List<FinancialTransaction> ImporFile(string filename, List<FinancialTransaction> financialTransactions)
        {
            try
            {
                FileInfo fileProps = new FileInfo(filename);
                Trace.WriteLine(string.Format("Processing {0} file: {1}",
                                              fileProps.Extension.ToUpper().Replace(".", ""),
                                              filename),
                                LogLevel.Information.ToString());
                
                if (!fileProps.Exists) return financialTransactions;

                //FileDefinitionManager fdm = new FileDefinitionManager();
                //fdm.ImportFile(FileProps.FullName);

                switch (fileProps.Extension.ToUpper())
                {
                    case ".QFX":
                        QuickenManager qm = new QuickenManager(fileProps.FullName);
                        financialTransactions = qm.Import(financialTransactions);
                        break;
                    case ".CSV":
                        CSVManager csvm = new CSVManager(fileProps.FullName);
                        financialTransactions = csvm.Import(financialTransactions);
                        break;
                    case ".XLSX":
                        XLSXManager xlsx = new XLSXManager(fileProps.FullName);
                        financialTransactions = xlsx.Import(financialTransactions);
                        break;
                    case ".QIF":
                    //break;
                    case ".OFX":
                    //break;
                    case ".ASO":
                    //break;
                    case ".QBO":
                    //break;
                    default:
                        Trace.WriteLine(string.Format("Unsupported format: {0}",
                                        fileProps.Extension.ToUpper()),
                                        LogLevel.Error.ToString());
                        break;
                }
                MainModel.LogTransactions();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message, LogLevel.Error.ToString());
            }
            return financialTransactions;
        }
    }
}
