using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.Utils.Converters
{
    public class GenericConverter<Tin, Tout> : IValueConverter
    {
        private Func<Tin, Tout> _func;
        public GenericConverter(Func<Tin, Tout> func)
        {
            _func = func;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Tin input)
            {
                return _func(input);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
