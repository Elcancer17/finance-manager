using FinanceManager.Domain;
using FinanceManager.Import.Extension;
using FinanceManager.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static FinanceManager.Domain.Quicken;

namespace FinanceManager.Import
{
    public sealed class QuickenManager : FileManager

    {
        const string ROOT_ELEMENT_NAME = "OFX";

        public string fileContent { get; set; }

        public QuickenManager(string filename) : base(filename)
        {
        }

        protected override void LoadFile(string filename)
        {
            try
            {
                //fileContent = File.ReadAllText(filename);
                using (var sr = new StreamReader(filename, System.Text.Encoding.GetEncoding("iso-8859-1")))
                {
                    fileContent = sr.ReadToEnd();
                }
                fileContent = fileContent.AdjustQuickenXml();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message, LogLevel.Error.ToString());
            }
        }
        protected override bool Validate()
        {
            if (fileContent.IndexOf(string.Format("<{0}>", ROOT_ELEMENT_NAME)) == -1)
            {
                throw new Exception(string.Format("Bad expected format for {0} file", 
                                    fileProps.GetExtention()));
            }
            return true;
        }

        public override void Import()
        {
            List<FinancialTransaction> resultFT = null;
            try
            {
                resultFT = Merge(GetData());
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message, LogLevel.Error.ToString());
            }
        }

        public OFX GetData()
        {
            return XMLManager.DeserializeXML<OFX>(fileContent, ROOT_ELEMENT_NAME);
        }

        public List<FinancialTransaction> Merge(OFX ofx)
        {
            List<FinancialTransaction> result = ftm.Load();
            try
            {
                bool transactionAdded = false;
                int jsonItemCount = result.Count();
                int importingItemCount = 0;
                int importedItemCount = 0;

                //Manage creditcard transaction
                if (ofx.CREDITCARDMSGSRSV1 != null &&
                    ofx.CREDITCARDMSGSRSV1.CCSTMTTRNRS != null &&
                    ofx.CREDITCARDMSGSRSV1.CCSTMTTRNRS.CCSTMTRS != null &&
                    ofx.CREDITCARDMSGSRSV1.CCSTMTTRNRS.CCSTMTRS.BANKTRANLIST != null)
                {
                    importingItemCount += ofx.CREDITCARDMSGSRSV1.CCSTMTTRNRS.CCSTMTRS.BANKTRANLIST.STMTTRN.Count();
                    foreach (STMTTRN itemL1 in ofx.CREDITCARDMSGSRSV1.CCSTMTTRNRS.CCSTMTRS.BANKTRANLIST.STMTTRN)
                    {
                        FinancialTransaction ft = itemL1.MapSTMTTRNToFinancialTransaction(fileProps.GetFinancialInstitutionType(), 
                                                                                          ofx.CREDITCARDMSGSRSV1.CCSTMTTRNRS.CCSTMTRS.CCACCTFROM.ACCTID);

                        if (result.FirstOrDefault(s => s.TransactionId == ft.TransactionId) == null)
                        {
                            result.Add(ft);
                            importedItemCount += 1;
                            transactionAdded = true;
                        }
                    }
                }

                //Manage Bank account transaction
                if (ofx.BANKMSGSRSV1 != null && ofx.BANKMSGSRSV1.STMTTRNRS != null)
                {
                    foreach (STMTTRNRS iteml1 in ofx.BANKMSGSRSV1.STMTTRNRS)
                    {
                        if (iteml1.STMTRS != null)
                        {
                            foreach (STMTRS iteml2 in iteml1.STMTRS)
                            {
                                if (iteml2.BANKTRANLIST != null)
                                {
                                    importingItemCount += iteml2.BANKTRANLIST.STMTTRN.Count();
                                    foreach (STMTTRN iteml3 in iteml2.BANKTRANLIST.STMTTRN)
                                    {
                                        FinancialTransaction ft = iteml3.MapSTMTTRNToFinancialTransaction(fileProps.GetFinancialInstitutionType(), 
                                                                                                          iteml2.BANKACCTFROM.BRANCHID);

                                        if (result.FirstOrDefault(s => s.TransactionId == ft.TransactionId) == null)
                                        {
                                            result.Add(ft);
                                            importedItemCount += 1;
                                            transactionAdded = true;
                                        }
                                    }
                                }
                            }
                        }
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
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message, LogLevel.Error.ToString());
            }
            return result;
        }
    }
}
