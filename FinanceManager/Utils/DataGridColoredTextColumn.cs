using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.Utils
{
    public class DataGridColoredTextColumn<T> : DataGridTemplateColumn
    {
        public DataGridColoredTextColumn()
        {
            CanUserSort = true;
        }
        private Binding binding;
        public Binding Binding
        {
            get => binding;
            set
            {
                binding = value;
                SortMemberPath = value.Path;
            }
        }
        public Func<T, IBrush> ColorFunc { get; set; }

        protected override Control GenerateElement(DataGridCell cell, object dataItem)
        {
            Grid grid = new()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            TextBlock textBlock = new TextBlock()
            { 
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center
            };
            textBlock.Bind(TextBlock.TextProperty, Binding);
            ColorConverter<T> colorConverter = new(ColorFunc);
            Binding colorBinding = new()
            {
                Converter = colorConverter,
                ConverterParameter = this,
            };
            grid.Children.Add(textBlock);

            //Style style = new Style(x => x.OfType<DataGridCell>()
            //                            .Not(x => x.Class(":selected"))
            //                            .Not(x => x.Class(":focus")));
            //Setter setter = new(Grid.BackgroundProperty, colorBinding);
            //style.Add(setter);
            //cell.Styles.Add(style);

            grid.Bind(TextBlock.BackgroundProperty, colorBinding);
            return grid;
        }
    }
}
