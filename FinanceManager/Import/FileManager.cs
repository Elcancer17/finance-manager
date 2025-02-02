using FinanceManager.Domain;
using System.IO;

namespace FinanceManager.Import
{
    public abstract class FileManager

    {
        public FileInfo fileProps { get; }

        public FileDefinition definition { get; }

        public FinancialTransactionManager ftm { get; set; }

        public FileManager(string filename)
        {
            ftm = new FinancialTransactionManager();
            fileProps = new FileInfo(filename);
            definition = new FileDefinitionManager().GetFileDefinition(fileProps.GetFinancialInstitutionType());
            LoadFile(filename);
            Validate();
        }

        protected abstract void LoadFile(string filename);
        protected abstract bool Validate();
        public abstract void Import();

    }
}
