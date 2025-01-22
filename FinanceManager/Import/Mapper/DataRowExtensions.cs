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
            // "Période de relevé en cours","2024-09-19","VIRGIN PLUS",,"En attente","Débit","104.68"
            //"","2024-08-15","PAIEMENT DESJARDINS","Montreal 00","Inscrites","Crédit","2996.67"
            //"","2024-08-15","AUBAINES YVES CROTEAU","Levis Qc","Inscrites","Débit","295.49"
            Csv item = new Csv();
            item.Filtre = row.ItemArray[0].ToString();
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
            //2024-12-27,"RESTAURANT NORMANDIN LEVIS, QC",59.11,,5223********9154
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
            item.CC = row.ItemArray[4].ToString();
            return item;
        }

        public static Csv MapDesjardinsRowToCsv(this DataRow row)
        {
            //Sample: 
            //"Chaudière ","024270","EOP","2024/11/28",00001,"Paiement facture - AccèsD Internet /Immatriculations - Permis","",35.40,"","","","","",3874.40
            //"Chaudière ","024270","EOP","2024/11/29",00004,"Virement entre folios /de 024373 EOP","","",1800.00,"","","","",5819.83
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
