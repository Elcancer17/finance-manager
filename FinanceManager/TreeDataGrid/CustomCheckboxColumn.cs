using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System;
using System.Linq.Expressions;
using Avalonia;
using Avalonia.Layout;
using FinanceManager.Utils;

namespace FinanceManager.TreeDataGrid
{
    public class CustomCheckBoxColumn<TModel> : CustomColumn<TModel, bool> where TModel : class
    {
        public CustomCheckBoxColumn(object header, LBinding<TModel, bool> binding) : base(header, binding)
        {
            _isTextSearchEnabled = true;
            _textSearchValueSelector = (TModel model) => _compiledPropertyGetter(model).ToString();
        }

        public override IDataTemplate GetCellTemplate()
        {
            FuncDataTemplate cellTemplate = new FuncDataTemplate<TModel>((model, namescope) =>
            {
                CheckBox checkbox = new CheckBox()
                {
                    Padding = new Thickness(0),
                    MinHeight = STANDARD_ROW_HEIGHT,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    DataContext = model
                };

                checkbox.Bind(CheckBox.IsCheckedProperty, ValueBinding);

                return checkbox;
            });
            return cellTemplate;
        }

        public override IDataTemplate GetCellEditingTemplate()
        {
            return null;
        }
    }
}
