using System.Data;

namespace FinanceManager.Utils
{
    public static class DataTableExtensions
    {
        public static DataRowCollection GetRows(this DataTable value)
        {
            return value.Rows;
        }

        public static DataRow GetRow(this DataTable value)
        {
            return value.Rows[0];
        }

        public static string GetHeaderToString(this DataTable value)
        {
            string headers = "";
            foreach (DataColumn c in value.Columns)
            {
                if (string.IsNullOrEmpty(headers)) { headers = c.ColumnName; }
                else { headers += "," + c.ColumnName; }
            }
            return headers;
        }

        public static int GetColumnsCount(this DataTable value)
        {
            return value.Columns.Count;
        }
    }
}
