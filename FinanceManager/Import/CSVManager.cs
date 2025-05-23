﻿using FinanceManager.Domain;
using FinanceManager.Import.Extension;
using FinanceManager.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FinanceManager.Import
{
    internal class CSVManager : FileManager
    {
        public string fileContent { get; set; }

        public CSVManager(string filename) : base(filename)
        {
        }

        protected override void LoadFile(string filename)
        {
            try
            {
                //fileContent = File.ReadAllText(filename, System.Text.Encoding.GetEncoding("iso-8859-1"));
                using (var sr = new StreamReader(filename, System.Text.Encoding.GetEncoding("iso-8859-1")))
                {
                    fileContent = sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message, LogLevel.Error.ToString());
            }
        }
        protected override bool Validate()
        {
            string line = fileContent.GetFirstLine();
            //Header validation
            if (definition.HaveHeader)
            {
                if (!definition.HeaderIsEmpty)
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        throw new Exception(string.Format("Error {0}: The header should not be empty",
                                                          FileDefinitionManager.VISA_INFINITE_MOMENTUM_SCOTIA));
                    }
                    if (!(line == definition.FirstLineFr || line == definition.FirstLineEn))
                    {
                        throw new Exception(string.Format("Error {0}: Header is not the good one for CSV file",
                                                          FileDefinitionManager.VISA_INFINITE_MOMENTUM_SCOTIA));
                    }
                }
            }
            if (string.IsNullOrEmpty(line)) {
                line = fileContent.GetSecondLine();
            }

            //Columns validation
            if (line.Split(",").Count() != definition.ColumnsCount)
            {
                throw new Exception(string.Format("{0}: Bad columns count {1} for CSV file, expected {2}",
                                                  FileDefinitionManager.VISA_INFINITE_MOMENTUM_SCOTIA,
                                                  0,
                                                  definition.ColumnsCount));
            }
            return true;
        }

        public override List<FinancialTransaction> Import(List<FinancialTransaction> financialTransactions)
        {
            try
            {
                financialTransactions = Merge(financialTransactions, GetData());
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message, LogLevel.Error.ToString());
            }
            return financialTransactions;
        }

        public string GetHeader(List<string> lines)
        {
            return lines[0].Replace("\r", "");
        }

        public List<Csv> GetData()
        {
            List<Csv> result = new List<Csv>();
            List<string> lines = fileContent.GetLines();
            for (int i = 0; i < lines.Count; i++)
            {
                lines[i] = lines[i].Replace("\r", "").Replace("\"", "").Replace(", ", " ");
                switch (fileProps.GetFinancialInstitutionType())
                {
                    case FileDefinitionManager.VISA_INFINITE_MOMENTUM_SCOTIA:
                        if (lines[i] != string.Empty && i > 0)
                        {
                            result.Add(lines[i].MapVisaInfiniteMomentumScotiaLineToCsv());
                        }
                        break;
                    case FileDefinitionManager.CIBC: case FileDefinitionManager.CC:
                        if (lines[i] != string.Empty)
                        {
                            result.Add(lines[i].MapCibcLineToCsv());
                        }
                        break;
                    case FileDefinitionManager.DESJARDINS: case FileDefinitionManager.BANQUE:
                        if (lines[i] != string.Empty)
                        {
                            result.Add(lines[i].MapDesjardinsLineToCsv());
                        }
                        break;
                    default:
                        break;
                }
            }
            return result;
        }

        public List<FinancialTransaction> Merge(List<FinancialTransaction> financialTransactions, List<Csv> importItems)
        {
            //List<FinancialTransaction> result = ftm.Load();
            bool transactionAdded = false;
            int jsonItemCount = financialTransactions.Count();
            int importingItemCount = importItems.Count();
            int importedItemCount = 0;

            foreach (Csv itemL1 in importItems)
            {
                FinancialTransaction ft = itemL1.MapCsvToFinancialTransaction(fileProps.GetFinancialInstitutionType());

                if (financialTransactions.FirstOrDefault(s => s.TransactionId == ft.TransactionId) == null)
                {
                    ft.Message = "NOUVEAU!";
                    ft.IsNew = true;
                    financialTransactions.Add(ft);
                    importedItemCount += 1;
                    transactionAdded = true;
                }
            }

            Trace.WriteLine(string.Format("Transactions before import: {0} - Importing Items: {1} - Imported Items: {2} - Transactions after import: {3}",
                                          jsonItemCount,
                                          importingItemCount,
                                          importedItemCount,
                                          financialTransactions.Count()),
                            LogLevel.Information.ToString());

            //if (transactionAdded)
            //{
            //    ftm.Save(result);
            //}
            return financialTransactions;
        }
    }
}
