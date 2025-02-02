using FinanceManager.Domain;
using FinanceManager.Import.Extension;
using FinanceManager.Logging;
using FinanceManager.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;

namespace FinanceManager.Import
{
    internal class XLSXManager : FileManager
    {
        public DataTable fileContent { get; set; }

        public XLSXManager(string filename) : base(filename)
        {
        }

        protected override void LoadFile(string filename)
        {
            string sheetName = "Transactions";
            OleDbCommand cmd = new OleDbCommand();
            OleDbDataAdapter da = new OleDbDataAdapter();
            fileContent = new DataTable();
            string connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"",
                                              fileProps.FullName);
            string query = string.Format("SELECT * FROM [{0}$]", sheetName); // You can use any different queries to get the data from the excel sheet
            OleDbConnection conn = new OleDbConnection(connString);
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                cmd = new OleDbCommand(query, conn);
                da = new OleDbDataAdapter(cmd);
                da.Fill(fileContent);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message, LogLevel.Error.ToString());
            }
            finally
            {
                da.Dispose();
                conn.Close();
            }
        }
        protected override bool Validate()
        {
            //Header validation
            if (definition.HaveHeader)
            {
                if (!definition.HeaderIsEmpty)
                {
                    string headers = fileContent.GetHeaderToString();
                    if (string.IsNullOrEmpty(headers))
                    {
                        throw new Exception(string.Format("Error {0}: The header should not be empty for {1} file",
                                                          FileDefinitionManager.VISA_INFINITE_MOMENTUM_SCOTIA,
                                                          fileProps.GetExtention()));
                    }
                    if (!(headers == definition.FirstLineFr || headers == definition.FirstLineEn))
                    {
                        throw new Exception(string.Format("Error {0}: Header is not the good one for {1} file",
                                                          FileDefinitionManager.VISA_INFINITE_MOMENTUM_SCOTIA,
                                                          fileProps.GetExtention()));
                    }
                }
            }
            //Columns validation
            if (fileContent.GetColumnsCount() != definition.ColumnsCount)
            {
                throw new Exception(string.Format("{0}: Bad columns count {1} for {2} file, expected {3}",
                                                  FileDefinitionManager.VISA_INFINITE_MOMENTUM_SCOTIA,
                                                  fileContent.GetColumnsCount(),
                                                  fileProps.GetExtention(),
                                                  definition.ColumnsCount));
            }
            return true;
        }

        public override void Import()
        {
            List<Csv> result = new List<Csv>();
            try
            {
                Merge(GetData());
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message, LogLevel.Error.ToString());
            }
        }

        public List<Csv> GetData()
        {
            List<Csv> result = new List<Csv>();
            for (int i = 0; i < fileContent.Rows.Count; i++)
            {
                switch (fileProps.GetFinancialInstitutionType())
                {
                    case FileDefinitionManager.VISA_INFINITE_MOMENTUM_SCOTIA:
                        result.Add(fileContent.Rows[i].MapVisaInfiniteMomentumScotiaRowToCsv());
                        break;
                    case FileDefinitionManager.CIBC:
                        result.Add(fileContent.Rows[i].MapCibcRowToCsv());
                        break;
                    case FileDefinitionManager.DESJARDINS:
                        result.Add(fileContent.Rows[i].MapDesjardinsRowToCsv());
                        break;
                    default:
                        break;
                }
            }
            return result;
        }

        public List<FinancialTransaction> Merge(List<Csv> importItems)
        {
            List<FinancialTransaction> result = ftm.Load();
            bool transactionAdded = false;
            int jsonItemCount = result.Count();
            int importingItemCount = importItems.Count();
            int importedItemCount = 0;

            foreach (Csv itemL1 in importItems)
            {
                FinancialTransaction ft = itemL1.MapCsvToFinancialTransaction(fileProps.GetFinancialInstitutionType());

                if (result.FirstOrDefault(s => s.TransactionId == ft.TransactionId) == null)
                {
                    result.Add(ft);
                    importedItemCount += 1;
                    transactionAdded = true;
                }
            }

            Trace.WriteLine(string.Format("Transactions before import: {0} - Importing Items: {1} - Imported Items: {2} - Transactions after import: {3}",
                                          jsonItemCount,
                                          importingItemCount,
                                          importedItemCount,
                                          result.Count()),
                            LogLevel.Information.ToString());

            if (transactionAdded)
            {
                ftm.Save(result);
            }
            return result;
        }
    }
}
