using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using Avalonia.Styling;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.Utils
{
    public static class DatagridExtentions
    {
        //public static void AddConditionalColoring<T>(this DataGrid datagrid, DataGridColumn column, Func<T, IBrush> colorFunc)
        //{
        //    Style style = new Style(x => x.OfType<DataGridCell>());
        //    ColorConverter<T> colorConverter = new(colorFunc);
        //    Binding binding = new()
        //    {
        //        Converter = colorConverter,
        //        ConverterParameter = column,
        //    };
        //    Setter setter = new(DataGridCell.BackgroundProperty, binding);
        //    style.Setters.Add(setter);
        //    datagrid.Styles.Add(style);
        //}

        //public static Binding GetBinding(this DataGridColumn column)
        //{
        //    if(column is DataGridBoundColumn boundColumn)
        //    {
        //        return (Binding)boundColumn.Binding;
        //    }
        //    else
        //    {
        //        throw new NotSupportedException();
        //    }
        //}

        public static object GetValue(this Binding binding, object element)
        {
            string[] segments = binding.Path.Split('.');
            object current = element;
            for(int i = 0; i < segments.Length; i++)
            {
                string segment = segments[i];
                int? index = null;
                int indexOfIndex = segment.IndexOf("[");
                if (indexOfIndex > -1)
                {
                    string rawIndex = segment.Substring(indexOfIndex, segment.Length - indexOfIndex);
                    index = int.Parse(rawIndex.Trim('[', ']'));
                    segment = segment.Substring(0, indexOfIndex);
                }
                PropertyInfo property = current.GetType().GetProperty(segment);
                current = property.GetValue(current);
                if(index != null && current is IList list)
                {
                    current = list[index.Value];
                }
            }
            return current;
        }
    }
}
