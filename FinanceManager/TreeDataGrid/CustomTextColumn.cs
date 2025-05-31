using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System;
using System.Linq.Expressions;
using Avalonia;
using Avalonia.Layout;
using FinanceManager.Utils;

namespace FinanceManager.TreeDataGrid
{
    public class CustomTextColumn<TModel, TValue> : CustomColumn<TModel, TValue> where TModel : class
    {
        public CustomTextColumn(object header, LBinding<TModel, TValue> binding) : base(header, binding)
        {
            _isTextSearchEnabled = true;
            _textSearchValueSelector = (TModel model) => _compiledPropertyGetter(model).ToString();
        }

        public override IDataTemplate GetCellTemplate()
        {
            FuncDataTemplate cellTemplate = new FuncDataTemplate<TModel>((model, namescope) =>
            {
                TextBlock textblock = new TextBlock()
                {
                    Padding = new Thickness(9,7),
                    MinHeight = STANDARD_ROW_HEIGHT,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    DataContext = model
                };

                textblock.Bind(TextBlock.TextProperty, ValueBinding);

                return textblock;
            });
            return cellTemplate;
        }

        public override IDataTemplate GetCellEditingTemplate()
        {
            if (IsReadOnly)
            {
                return null;
            }
            FuncDataTemplate cellEditingTemplate = new FuncDataTemplate<TModel>((model, namescope) =>
            {
                TextBox textBox = new TextBox()
                {
                    Padding = new Thickness(4),
                    MinHeight = STANDARD_ROW_HEIGHT,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Stretch
                };
                textBox.Bind(TextBox.TextProperty, ValueBinding);

                return textBox;
            });
            return cellEditingTemplate;
        }
    }
}
