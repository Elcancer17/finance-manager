using FinanceManager.Domain;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FinanceManager.Import
{
    public class FileDefinitionManager
    {
        const string VISA_INFINITE_MOMENTUM_SCOTIA_FIRST_LINE_FR = "Filtre,Date,Description,Sous-description,État,Type d’opération,Montant";
        const string VISA_INFINITE_MOMENTUM_SCOTIA_FIRST_LINE_EN = "Filter,Date,Description,Sub-description,Status,Type of Transaction,Amount";

        public const string VISA_INFINITE_MOMENTUM_SCOTIA = "VisaInfiniteMomentumScotia";
        public const string CIBC = "Cibc";
        public const string DESJARDINS = "Desjardins";
        public const string BANQUE = "BANQUE";
        public const string CC = "CC";

        public const string QFX_EXTENTION = ".QFX";
        public const string CSV_EXTENTION = ".CSV";
        public const string XLSX_EXTENTION = ".XLSX";

        const string FILENAME = "FileDefinitions.json";

        public FileInfo fileProps { get; }
        public List<FileDefinition> DefinitionList { get; }

        public FileDefinitionManager()
        {
            fileProps = GetFile();
            //DefinitionList = JsonManager.LoadJson<List<FileDefinition>>(fileProps.FullName);
            DefinitionList = GetFileDefinition();
            Validate();
        }
        protected bool Validate()
        {
            //TODO
            return true;
        }

        public void AddFromDragAndDrop(string filename)
        {
            //TODO
            FileInfo fileProps = new FileInfo(filename);
            if (DefinitionList.FirstOrDefault(s => s.DefinitionType == fileProps.GetFinancialInstitutionType()) == null)
            {
                DefinitionList.Add(new FileDefinition() { 
                    DefinitionType = fileProps.GetFinancialInstitutionType(),
                    FileExtention = fileProps.Extension.ToUpper(),
                    ColumnsCount = 0,
                    HaveHeader = false,
                    HeaderIsEmpty = false
                });
            }
            Save();
        }
        private List<FileDefinition> GetFileDefinition()
        {
            return new List<FileDefinition>() {
                new FileDefinition() { 
                    DefinitionType = DESJARDINS,
                    FileExtention = CSV_EXTENTION,
                    ColumnsCount = 14,
                    HaveHeader = true,
                    HeaderIsEmpty = true,
                    ColumnIndexForFiltre = "0",
                    ColumnIndexForCompte = "1",
                    ColumnIndexForCompteType = "2",
                    ColumnIndexForNoSeq = "4",
                    ColumnIndexForTransactionDate = "3",
                    ColumnIndexForDescription = "5",
                    ColumnIndexForSubDescription = "",
                    ColumnIndexForEtat = "",
                    ColumnIndexForTransactionType = "",
                    ColumnIndexForAmountDebit = "7",
                    ColumnIndexForAmountCredit = "8",
                    ColumnIndexForTotal = "13"
                },
                new FileDefinition() { 
                    DefinitionType = VISA_INFINITE_MOMENTUM_SCOTIA,
                    FileExtention = CSV_EXTENTION,
                    ColumnsCount = 7,
                    HaveHeader = true,
                    HeaderIsEmpty = false,
                    FirstLineFr = VISA_INFINITE_MOMENTUM_SCOTIA_FIRST_LINE_FR,
                    FirstLineEn = VISA_INFINITE_MOMENTUM_SCOTIA_FIRST_LINE_EN,
                    ColumnIndexForFiltre = "0",
                    ColumnIndexForCompte = "",
                    ColumnIndexForCompteType = "5",
                    ColumnIndexForNoSeq = "",
                    ColumnIndexForTransactionDate = "1",
                    ColumnIndexForDescription = "2",
                    ColumnIndexForSubDescription = "3",
                    ColumnIndexForEtat = "4",
                    ColumnIndexForTransactionType = "",
                    ColumnIndexForAmountCredit = "6",
                    ColumnIndexForAmountDebit = "6",
                    ColumnIndexForTotal = ""
                },
                new FileDefinition() { 
                    DefinitionType = CIBC,
                    FileExtention = CSV_EXTENTION,
                    ColumnsCount = 6,
                    HaveHeader = false,
                    HeaderIsEmpty = true,
                    ColumnIndexForFiltre = "",
                    ColumnIndexForCompte = "4",
                    ColumnIndexForCompteType = "",
                    ColumnIndexForNoSeq = "",
                    ColumnIndexForTransactionDate = "0",
                    ColumnIndexForDescription = "1",
                    ColumnIndexForSubDescription = "",
                    ColumnIndexForEtat = "",
                    ColumnIndexForTransactionType = "",
                    ColumnIndexForAmountDebit = "2",
                    ColumnIndexForAmountCredit = "3",
                    ColumnIndexForTotal = ""
                }
            };
        }

        private FileInfo GetFile()
        {
            FileInfo location = new FileInfo(Assembly.GetExecutingAssembly().Location);
            string fullname = Path.Combine(location.DirectoryName, FILENAME);
            return new FileInfo(fullname);
        }

        public FileDefinition GetFileDefinition(string definitionType)
        {
            //FileDefinition result = DefinitionList.FirstOrDefault(s => s.DefinitionType == definitionType && s.FileExtention == fileExtention.ToUpper());
            FileDefinition result = DefinitionList.FirstOrDefault(s => s.DefinitionType == definitionType);
            return result;
        }

        public void Save() {
            JsonManager.SaveToJson(fileProps.FullName, DefinitionList);
        }
    }
}
