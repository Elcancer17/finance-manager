using System.IO;

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
                return FileDefinitionManager.VISA_INFINITE_MOMENTUM_SCOTIA;
            }
            else if (value.Name.ToLower().Contains("cibc") || value.Name.ToLower().Contains("cc"))
            {
                return FileDefinitionManager.CIBC;
            }
            else if (value.Name.ToLower().Contains("releve") || value.Name.ToLower().Contains("desjardins") || value.Name.ToLower().Contains("banque"))
            {
                return FileDefinitionManager.DESJARDINS;
            }
            return string.Empty;
        }
    }
}
