using FinanceManager.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace FinanceManager.Import.Extension
{
    public static class StringExtensions
    {
        public static string ToUTF8(this string text)
        {
            return Encoding.UTF8.GetString(Encoding.Default.GetBytes(text));
        }

        public static string AdjustSpecialCaracters(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }
            //text =  text.RemoveSpecialCharacters();
            
            List<string> chars = new List<string> {
                "&; ",
                "à;a",
                "À;A",
                "é;e",
                "É;E",
                "è;e",
                "È;E",
                "ê;e",
                "Ê;E",
                "ë;e",
                "Ë;E",
                "ç;c",
                "Ç;C",
                "ù;u",
                "Ù;U",
                "ô;o",
                "Ô;O",
                "�;",
                "  ; "
            };
            if (text.Contains("électricité")) {
                string aaa = "";
            }


            for (int i = 0; i < chars.Count; ++i)
            {
                List<string> values = chars[i].Split(";").ToList();
                text = text.Replace(values[0], values[1]);
            }
            
            return text;
        }

        public static string RemoveSpecialCharacters(this string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }
        public static string ReplaceSpecialCharacters(this string str)
        {
            byte[] tempBytes;
            tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(str);
            return System.Text.Encoding.UTF8.GetString(tempBytes);
        }


        public static List<string> GetLines(this string value)
        {
            //return value.Split("\n").ToList();
            value = value.Replace("<", "\n<"); //For CIBC parsing
            List<string> lines = value.Split("\n").ToList();
            return lines;
        }

        public static string GetFirstLine(this string value)
        {
            List<string> lines = value.GetLines();
            if (lines.Count > 0)
            {
                return lines[0].Replace("\r", "").Replace("\n", "");
            }
            return string.Empty;
        }
        public static string GetSecondLine(this string value)
        {
            List<string> lines = value.GetLines();
            if (lines.Count > 1)
            {
                return lines[1].Replace("\r", "").Replace("\n", "");
            }
            return string.Empty;
        }

        public static DateTime ToDate(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace("-", "").Replace("/", "");
                int yyyy = int.Parse(value.Substring(0, 4));
                int mm = int.Parse(value.Substring(4, 2));
                int dd = int.Parse(value.Substring(6, 2));
                return new DateTime(yyyy, mm, dd);
            }
            return new DateTime();
        }

        public static decimal ToDecimal(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace(".", ",");
                if (decimal.TryParse(value, out decimal j))
                {
                    return j;
                }
                else
                {
                    throw new Exception(string.Format("String {0} could not be parsed to Decimal.",
                                                      value));
                }
            }
            return 0;
        }

        public static int ToInt(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (int.TryParse(value, out int j))
                {
                    return j;
                }
                else
                {
                    throw new Exception(string.Format("String {0} could not be parsed to Int.",
                                                      value));
                }
            }
            return 0;
        }

        public static long ToLong(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (long.TryParse(value, out long j))
                {
                    return j;
                }
                else
                {
                    throw new Exception(string.Format("String {0} could not be parsed to Long.",
                                                      value));
                }
            }
            return 0;
        }

        public static string AdjustQuickenXml(this string value)
        {
            value = value.CleanQuickenXmlStartLines();
            value = value.AddXmlFisrtLine();
            value = value.AdjustSpecialCaracters();
            value = value.AdjustQuickenXmlEndTag();
            value = value.ToUTF8();
            return value;
        }

        private static string CleanQuickenXmlStartLines(this string value)
        {
            int pos = value.IndexOf("<OFX>");
            return value.Substring(pos);
        }

        private static string AddXmlFisrtLine(this string value)
        {
            value = "<?xml version=\"1.0\"?>\n" + value;
            return value;
        }

        private static string AdjustQuickenXmlEndTag(this string value)
        {
            List<string> lines = value.GetLines();
            for (int i = lines.Count - 1; i > -1; i--)
            {
                if (string.IsNullOrEmpty(lines[i]))
                {
                    lines.RemoveAt(i);
                    continue;
                }
                lines[i] = lines[i].AddXmlEndTag("DTSERVER");
                lines[i] = lines[i].AddXmlEndTag("USERKEY");
                lines[i] = lines[i].AddXmlEndTag("INTU.BID");
                lines[i] = lines[i].AddXmlEndTag("LANGUAGE");
                lines[i] = lines[i].AddXmlEndTag("CODE");
                lines[i] = lines[i].AddXmlEndTag("SEVERITY");
                lines[i] = lines[i].AddXmlEndTag("MESSAGE");
                lines[i] = lines[i].AddXmlEndTag("TRNUID");
                lines[i] = lines[i].AddXmlEndTag("CURDEF");
                lines[i] = lines[i].AddXmlEndTag("BANKID");
                lines[i] = lines[i].AddXmlEndTag("BRANCHID");
                lines[i] = lines[i].AddXmlEndTag("ACCTID");
                lines[i] = lines[i].AddXmlEndTag("ACCTTYPE");
                lines[i] = lines[i].AddXmlEndTag("DTSTART");
                lines[i] = lines[i].AddXmlEndTag("DTEND");
                lines[i] = lines[i].AddXmlEndTag("TRNTYPE");
                lines[i] = lines[i].AddXmlEndTag("DTPOSTED");
                lines[i] = lines[i].AddXmlEndTag("TRNAMT");
                lines[i] = lines[i].AddXmlEndTag("FITID");
                lines[i] = lines[i].AddXmlEndTag("NAME");
                lines[i] = lines[i].AddXmlEndTag("MEMO");
                lines[i] = lines[i].AddXmlEndTag("BALAMT");
                lines[i] = lines[i].AddXmlEndTag("DTASOF");
                lines[i] = lines[i].AddXmlEndTag("DTPROFUP");
                lines[i] = lines[i].AddXmlEndTag("DTACCTUP");
            }
            value = string.Join("\n", lines);
            return value;
        }

        private static string AddXmlEndTag(this string value, string attribute)
        {
            if (value.IndexOf("<" + attribute + ">") != -1)
            {
                value += "</" + attribute + ">";
            }
            return value;
        }
        public static Csv MapVisaInfiniteMomentumScotiaLineToCsv(this string line)
        {
            FileDefinitionManager fdm = new FileDefinitionManager();
            FileDefinition fd = fdm.GetFileDefinition(FileDefinitionManager.VISA_INFINITE_MOMENTUM_SCOTIA);
            //Sample:
            // Filtre,Date,Description,Sous-description,État,Type d’opération,Montant
            // "Période de relevé en cours","2024-09-19","VIRGIN PLUS",,"En attente","Débit","104.68"
            //"","2024-08-15","PAIEMENT DESJARDINS","Montreal 00","Inscrites","Crédit","2996.67"
            //"","2024-08-15","AUBAINES YVES CROTEAU","Levis Qc","Inscrites","Débit","295.49"
            line = line.Replace("\r", "").Replace("\"", "");
            List<string> cols = line.Split(",").ToList();
            Csv item = new Csv();
            item.Filtre = cols[fd.ColumnIndexForFiltre.ToInt()];
            item.Compte = fd.Compte;
            item.TransactionDate = cols[fd.ColumnIndexForTransactionDate.ToInt()];
            item.Description = cols[fd.ColumnIndexForDescription.ToInt()];
            item.SubDescription = cols[fd.ColumnIndexForSubDescription.ToInt()];  
            item.Etat = cols[fd.ColumnIndexForEtat.ToInt()];
            item.TransactionType = cols[fd.ColumnIndexForTransactionType.ToInt()];
            item.Amount = cols[fd.ColumnIndexForAmountCredit.ToInt()];
            if ((item.TransactionType == "Crédit" || item.TransactionType == "Credit") &&
                !cols[fd.ColumnIndexForAmountCredit.ToInt()].Contains("-"))
            {
                item.Amount = "-" + cols[fd.ColumnIndexForAmountCredit.ToInt()];
            }
            return item;
        }

        public static Csv MapCibcLineToCsv(this string line)
        {
            FileDefinitionManager fdm = new FileDefinitionManager();
            FileDefinition fd = fdm.GetFileDefinition(FileDefinitionManager.CIBC);
            // Sample: 
            //2024-12-27,"RESTAURANT NORMANDIN LEVIS, QC",59.11,,5223********9154
            line = line.Replace("\r", "").Replace("\"", "");
            List<string> cols = line.Split(",").ToList();
            Csv item = new Csv();
            item.TransactionDate = cols[fd.ColumnIndexForTransactionDate.ToInt()];
            item.Description = cols[fd.ColumnIndexForDescription.ToInt()];
            if (cols[fd.ColumnIndexForAmountDebit.ToInt()] != string.Empty)
            {
                item.Amount = "-" + cols[fd.ColumnIndexForAmountDebit.ToInt()];
                item.TransactionType = "DEBIT";
            }
            else if (cols[fd.ColumnIndexForAmountCredit.ToInt()] != string.Empty)
            {
                item.Amount = cols[fd.ColumnIndexForAmountCredit.ToInt()];
                item.TransactionType = "CREDIT";
            }
            item.Compte = cols[fd.ColumnIndexForCompte.ToInt()];
            item.Compte = fd.Compte;
            return item;
        }

        public static Csv MapDesjardinsLineToCsv(this string line)
        {
            FileDefinitionManager fdm = new FileDefinitionManager();
            FileDefinition fd = fdm.GetFileDefinition(FileDefinitionManager.DESJARDINS);
            //Sample: 
            //"Chaudière ","024270","EOP","2024/11/28",00001,"Paiement facture - AccèsD Internet /Immatriculations - Permis","",35.40,"","","","","",3874.40
            //"Chaudière ","024270","EOP","2024/11/29",00004,"Virement entre folios /de 024373 EOP","","",1800.00,"","","","",5819.83
            line = line.Replace("\r", "").Replace("\"", "");
            List<string> cols = line.Split(",").ToList();
            Csv item = new Csv();
            item.Filtre = cols[fd.ColumnIndexForFiltre.ToInt()];
            item.Compte = cols[fd.ColumnIndexForCompte.ToInt()];
            item.CompteType = cols[fd.ColumnIndexForCompteType.ToInt()];
            item.TransactionDate = cols[fd.ColumnIndexForTransactionDate.ToInt()];
            item.NoSeq = cols[fd.ColumnIndexForNoSeq.ToInt()];
            item.Description = cols[fd.ColumnIndexForDescription.ToInt()];
            item.Amount = cols[fd.ColumnIndexForAmountDebit.ToInt()];
            if (cols[fd.ColumnIndexForAmountDebit.ToInt()] != string.Empty)
            {
                item.Amount = "-" + cols[fd.ColumnIndexForAmountDebit.ToInt()];
                item.TransactionType = "DEBIT";
            }
            else if (cols[fd.ColumnIndexForAmountCredit.ToInt()] != string.Empty)
            {
                item.Amount = cols[fd.ColumnIndexForAmountCredit.ToInt()];
                item.TransactionType = "CREDIT";
            }
            item.Total = cols[fd.ColumnIndexForTotal.ToInt()];
            return item;
        }
    }
}
