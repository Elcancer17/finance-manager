using Avalonia.Data.Converters;
using Avalonia.Data;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FinanceManager.Utils
{
    public class LBinding<TIn, TOut>
    {
        public LBinding(Expression<Func<TIn, TOut>> expression)
        {
            Expression = expression;
        }

        public Expression<Func<TIn, TOut>> Expression { get; }
        public object Source { get; set; } = AvaloniaProperty.UnsetValue;
        public IValueConverter Converter { get; set; }
        public BindingMode Mode { get; set; }
        public UpdateSourceTrigger UpdateSourceTrigger { get; set; }
        public string StringFormat { get; set; }

        public Binding Compile()
        {
            List<(string name, Type type)> propertiyPath = Expression.GetPropertyPath();
            string bindingPath = string.Join(".", propertiyPath.Skip(1).Select(x => x.name));

            return new Binding(bindingPath)
            {
                Source = Source,
                Converter = Converter,
                Mode = Mode,
                UpdateSourceTrigger = UpdateSourceTrigger,
                StringFormat = StringFormat
            };
        }
    }
}
