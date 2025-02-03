using FinanceManager.Domain;
using System.Data;

namespace FinanceManager.Import.Extension
{
    public static class DataRowExtensions
    {
        public static Csv MapVisaInfiniteMomentumScotiaRowToCsv(this DataRow row)
        {
            //Sample:
            // Filtre,Date,Description,Sous-description,État,Type d’opération,Montant
            // "Période de relevé en cours","2025-01-01","ACHAT 1",,"En attente","Débit","100.00"
            //"","2025-01-02","ACHAT 2","Montreal 00","Inscrites","Crédit","200.00"
            //"","2025-01-03","ÀCHAT 3","Levis Qc","Inscrites","Débit","300.00"
            Csv item = new Csv();
            item.Filtre = row.ItemArray[0].ToString();
            item.Compte = "4538261968031105"; // "4538261968031014";
            item.TransactionDate = row.ItemArray[1].ToString();
            item.Description = row.ItemArray[2].ToString();
            item.SubDescription = row.ItemArray[3].ToString();
            item.Etat = row.ItemArray[4].ToString();
            item.TransactionType = row.ItemArray[5].ToString();
            item.Amount = row.ItemArray[6].ToString();
            if ((item.TransactionType == "Crédit" || item.TransactionType == "Credit") && 
                 !row.ItemArray[6].ToString().Contains("-"))
            {
                item.Amount = "-" + row.ItemArray[6];
            }
            return item;
        }

        public static Csv MapCibcRowToCsv(this DataRow row)
        {
            // Sample: 
            //2025-01-01,"ACHAT 1, QC",100.00,,1234********2222
            Csv item = new Csv();
            item.TransactionDate = row.ItemArray[0].ToString();
            item.Description = row.ItemArray[1].ToString();
            if (row.ItemArray[2].ToString() != string.Empty)
            {
                item.Amount = row.ItemArray[2].ToString();
                item.TransactionType = "DEBIT";
            }
            else if (row.ItemArray[3].ToString() != string.Empty)
            {
                item.Amount = "-" + row.ItemArray[3].ToString();
                item.TransactionType = "CREDIT";
            }
            item.Compte = row.ItemArray[4].ToString();
            item.Compte = "5223030005809154";
            return item;
        }

        public static Csv MapDesjardinsRowToCsv(this DataRow row)
        {
            //Sample: 
            //"Chaudière ","123456","EOP","2025-01-01",00001,"ACHAT 1","",100.00,"","","","","",5000.00
            //"Chaudière ","123456","EOP","2025-01-02",00004,"ACHAT 2","","",1000.00,"","","","",5000.00
            Csv item = new Csv();
            item.Filtre = row.ItemArray[0].ToString();
            item.Compte = row.ItemArray[1].ToString();
            item.CompteType = row.ItemArray[2].ToString();
            item.TransactionDate = row.ItemArray[3].ToString();
            item.NoSeq = row.ItemArray[4].ToString();
            item.Description = row.ItemArray[5].ToString();
            //item.Amount = row.ItemArray[7].ToString();
            if (row.ItemArray[7].ToString() != string.Empty)
            {
                item.Amount = "-" + row.ItemArray[7].ToString();
                item.TransactionType = "DEBIT";
            }
            else if (row.ItemArray[8].ToString() != string.Empty)
            {
                item.Amount = row.ItemArray[8].ToString();
                item.TransactionType = "CREDIT";
            }
            item.Total = row.ItemArray[13].ToString();
            return item;
        }
    }
}
