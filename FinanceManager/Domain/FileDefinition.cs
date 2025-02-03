namespace FinanceManager.Domain
{
    public class FileDefinition
    {
        public string DefinitionType { get; set; }
        public string FileExtention { get; set; }
        public string Compte { get; set; }
        public bool HaveHeader { get; set; }
        public bool HeaderIsEmpty { get; set; }
        public int ColumnsCount { get; set; }
        public string FirstLineFr { get; set; }
        public string FirstLineEn { get; set; }

        public string ColumnIndexForFiltre { get; set; }
        public string ColumnIndexForCompte { get; set; }
        public string ColumnIndexForCompteType { get; set; }
        public string ColumnIndexForNoSeq { get; set; }
        public string ColumnIndexForTransactionDate { get; set; }
        public string ColumnIndexForDescription { get; set; }
        public string ColumnIndexForSubDescription { get; set; }
        public string ColumnIndexForEtat { get; set; }
        public string ColumnIndexForTransactionType { get; set; }
        public string ColumnIndexForAmountCredit { get; set; }
        public string ColumnIndexForAmountDebit { get; set; }
        public string ColumnIndexForTotal { get; set; }
    }
}
