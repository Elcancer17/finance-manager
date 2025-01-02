using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.Utils
{
    public class ColorConverter<T> : IValueConverter
    {
        private Func<T, IBrush> colorFunc;
        public ColorConverter(Func<T, IBrush> colorFunc)
        {
            this.colorFunc = colorFunc;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null && parameter is DataGridColoredTextColumn<decimal> column)
            {
                object actualValue = column.Binding.GetValue(value);
                if (actualValue is T castedValue)
                {
                    return colorFunc.Invoke(castedValue);
                }
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
