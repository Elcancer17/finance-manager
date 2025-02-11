using FinanceManager.Domain;
using FinanceManager.Logging;
using FinanceManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace FinanceManager.Import
{
    public class ImportManager
    {
        private FileInfo FileProps { get; set; }

        public ImportManager(string filename)
        {
            FileProps = new FileInfo(filename);
            Trace.WriteLine(string.Format("Processing {0} file: {1}",
                                          FileProps.Extension.ToUpper().Replace(".", ""),
                                          filename),
                            LogLevel.Information.ToString());
        }

        public List<FinancialTransaction> ImporFile(List<FinancialTransaction> financialTransactions)
        {
            try
            {
                FileDefinitionManager fdm = new FileDefinitionManager();
                fdm.AddFromDragAndDrop(FileProps.FullName);

                switch (FileProps.Extension.ToUpper())
                {
                    case ".QFX":
                        QuickenManager qm = new QuickenManager(FileProps.FullName);
                        financialTransactions = qm.Import(financialTransactions);
                        break;
                    case ".CSV":
                        CSVManager csvm = new CSVManager(FileProps.FullName);
                        financialTransactions = csvm.Import(financialTransactions);
                        break;
                    case ".XLSX":
                        XLSXManager xlsx = new XLSXManager(FileProps.FullName);
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
                                        FileProps.Extension.ToUpper()),
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
