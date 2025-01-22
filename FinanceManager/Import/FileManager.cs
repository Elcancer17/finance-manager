using System.IO;
using FinanceManager.Domain;
using static FinanceManager.Domain.FileDefinition;

namespace FinanceManager.Import
{
    public abstract class FileManager

    {
        public FileInfo fileProps { get; }

        public Definition definition { get; }

        public FinancialTransactionManager ftm { get; set; }

        public FileManager(string filename)
        {
            ftm = new FinancialTransactionManager();
            fileProps = new FileInfo(filename);
            definition = new FileDefinition().GetFileDefinition(fileProps.GetFinancialInstitutionType(), fileProps.Extension);
            LoadFile(filename);
            Validate();
        }

        protected abstract void LoadFile(string filename);
        protected abstract bool Validate();
        public abstract void Import();

    }
}
