using System.Collections.Generic;
using System.Linq;

namespace FinanceManager.Domain
{
    public class FileDefinition
    {
        const string VISA_INFINITE_MOMENTUM_SCOTIA_FIRST_LINE_FR = "Filtre,Date,Description,Sous-description,État,Type d’opération,Montant";
        const string VISA_INFINITE_MOMENTUM_SCOTIA_FIRST_LINE_EN = "Filter,Date,Description,Sub-description,Status,Type of Transaction,Amount";

        public const string VISA_INFINITE_MOMENTUM_SCOTIA = "VisaInfiniteMomentumScotia";
        public const string CIBC = "Cibc";
        public const string DESJARDINS = "Desjardins";

        public const string QFX_EXTENTION = ".QFX";
        public const string CSV_EXTENTION = ".CSV";
        public const string XLSX_EXTENTION = ".XLSX";


        public class Definition
        {
            public Definition(string definitionType,
                              string fileExtention, 
                              int columnsCount,
                              bool haveHeader, 
                              bool headerIsEmpty)
            {
                DefinitionType = definitionType;
                FileExtention = fileExtention;
                ColumnsCount = columnsCount;
                HaveHeader = haveHeader;
                HeaderIsEmpty = headerIsEmpty;
            }

            public Definition(string definitionType, 
                              string fileExtention, 
                              int columnsCount,
                              bool haveHeader, 
                              bool headerIsEmpty, 
                              string firstLineFR, 
                              string firstLineEN)
            {
                DefinitionType = definitionType;
                FileExtention = fileExtention;
                ColumnsCount = columnsCount;
                HaveHeader = haveHeader;
                HeaderIsEmpty = headerIsEmpty;
                FirstLineFr = firstLineFR;
                FirstLineEn = firstLineEN;
            }

            public string DefinitionType { get; }
            public string FileExtention { get; }
            public bool HaveHeader { get; }
            public bool HeaderIsEmpty { get; }
            public int ColumnsCount { get; }
            public string FirstLineFr { get; }
            public string FirstLineEn { get; }
        }

        public List<Definition> DefinitionList { get; }

        public FileDefinition()
        {
            DefinitionList = new List<Definition>();
            DefinitionList.Add(new Definition(DESJARDINS, CSV_EXTENTION, 14, true, true));
            DefinitionList.Add(new Definition(VISA_INFINITE_MOMENTUM_SCOTIA, CSV_EXTENTION, 7, true, false, VISA_INFINITE_MOMENTUM_SCOTIA_FIRST_LINE_FR, VISA_INFINITE_MOMENTUM_SCOTIA_FIRST_LINE_EN));
            DefinitionList.Add(new Definition(VISA_INFINITE_MOMENTUM_SCOTIA, XLSX_EXTENTION, 7, true, false, VISA_INFINITE_MOMENTUM_SCOTIA_FIRST_LINE_FR, VISA_INFINITE_MOMENTUM_SCOTIA_FIRST_LINE_EN));
            DefinitionList.Add(new Definition(CIBC, CSV_EXTENTION, 6, false, true));
        }

        public Definition GetFileDefinition(string definitionType, string fileExtention)
        {
            Definition result = DefinitionList.FirstOrDefault(s => s.DefinitionType == definitionType && s.FileExtention == fileExtention.ToUpper());
            return result;
        }
    }
}
