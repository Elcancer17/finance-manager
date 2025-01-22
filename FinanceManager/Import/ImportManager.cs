﻿using FinanceManager.Logging;
using System;
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

        public void ImporFile()
        {
            try
            {
                switch (FileProps.Extension.ToUpper())
                {
                    case ".QFX":
                        QuickenManager qm = new QuickenManager(FileProps.FullName);
                        qm.Import();
                        break;
                    case ".CSV":
                        CSVManager csvm = new CSVManager(FileProps.FullName);
                        csvm.Import();
                        break;
                    case ".XLSX":
                        XLSXManager xlsx = new XLSXManager(FileProps.FullName);
                        xlsx.Import();
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
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message, LogLevel.Error.ToString());
            }
        }
    }
}
