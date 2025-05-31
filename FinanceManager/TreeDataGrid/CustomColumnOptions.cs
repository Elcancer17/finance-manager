using Avalonia.Controls.Models.TreeDataGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.TreeDataGrid
{
    public class CustomColumnOptions<T> : ColumnOptions<T>
    {
        public bool IsReadOnly { get; set; }
    }
}
