using Avalonia.Controls.Selection;
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.TreeDataGridAddons
{
    public static class TreeDataGridExtensions
    {
        public static IReadOnlyList<TModel> GetSelection<TModel>(this TreeDataGrid datagrid) where TModel : class
        {
            HashSet<TModel> selectedItems = new HashSet<TModel>();
            switch (datagrid.Source.Selection)
            {
                case TreeDataGridCellSelectionModel<TModel> selectionModel:
                    for (int i = 0; i < selectionModel.SelectedIndexes.Count; i++)
                    {
                        int rowIndex = datagrid.Source.Rows.ModelIndexToRowIndex(selectionModel.SelectedIndexes[i].RowIndex);
                        selectedItems.Add((TModel)datagrid.Source.Rows[rowIndex].Model);
                    }
                    break;
                default:
                    throw new NotSupportedException();
            }
            return selectedItems.ToArray();
        }
    }
}
