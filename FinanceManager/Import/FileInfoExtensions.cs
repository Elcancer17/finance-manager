using System.IO;
using FinanceManager.Domain;

namespace FinanceManager.Import
{
    public static class FileInfoExtensions
    {
        public static string GetExtention(this FileInfo value)
        {
            return value.Extension.Replace(".", "").ToUpper();
        }

        public static string GetFinancialInstitutionType(this FileInfo value)
        {
            if (value.Name.ToLower().Contains("visa") && value.Name.ToLower().Contains("scotia"))
            {
                return FileDefinition.VISA_INFINITE_MOMENTUM_SCOTIA;
            }
            else if (value.Name.ToLower().Contains("cibc"))
            {
                return FileDefinition.CIBC;
            }
            else if (value.Name.ToLower().Contains("releve"))
            {
                return FileDefinition.DESJARDINS;
            }
            return string.Empty;
        }
    }
}
