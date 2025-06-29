using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Experimental.Data;
using System;
using System.Linq.Expressions;
using Avalonia;
using Avalonia.Data.Converters;
using FinanceManager.Utils;

namespace FinanceManager.TreeDataGridAddons
{

    public abstract class CustomColumn<TModel, TValue> where TModel : class
    {
        protected const int STANDARD_ROW_HEIGHT = 24;

        protected LBinding<TModel, TValue> _binding;
        protected Func<TModel, TValue> _compiledPropertyGetter;

        protected bool _isTextSearchEnabled;
        protected Func<TModel, string> _textSearchValueSelector;

        public object Header { get; protected set; }
        public Binding ValueBinding { get; protected set; }
        public GridLength Width { get; set; } = GridLength.Auto;
        public bool IsReadOnly { get; set; }

        public CustomColumn(object header, LBinding<TModel, TValue> binding)
        {
            Header = header;
            _binding = binding;
            _compiledPropertyGetter = binding.Expression.Compile();
            ValueBinding = binding.Compile();
            if(binding.Mode != BindingMode.TwoWay)
            {
                IsReadOnly = true;
            }
        }

        protected virtual CustomColumnOptions<TModel> GetDefaultOptions()
        {
            return new CustomColumnOptions<TModel>()
            {
                CanUserResizeColumn = true,
                CanUserSortColumn = true,
                CompareAscending = CompareAscending,
                CompareDescending = CompareDescending,
            };
        }

        protected virtual int CompareAscending(TModel first, TModel second)
        {
            TValue firstValue = _compiledPropertyGetter(first);
            TValue secondValue = _compiledPropertyGetter(second);

            if (firstValue is IComparable<TValue> firstComparable)
            {
                return firstComparable.CompareTo(secondValue);
            }
            else
            {
                string firstString = firstValue?.ToString() ?? string.Empty;
                string secondString = secondValue?.ToString() ?? string.Empty;
                return firstString.CompareTo(secondString);
            }
        }

        protected virtual int CompareDescending(TModel first, TModel second)
        {
            return CompareAscending(second, first);
        }

        public abstract IDataTemplate GetCellTemplate();

        public virtual IDataTemplate GetCellEditingTemplate()
        {
            return null;
        }

        public TemplateColumn<TModel> Compile(ColumnOptions<TModel> options = null)
        {
            options = options ?? GetDefaultOptions();
            return new TemplateColumn<TModel>(Header, GetCellTemplate(), GetCellEditingTemplate(), Width, new TemplateColumnOptions<TModel>()
            {
                IsTextSearchEnabled = _isTextSearchEnabled,
                TextSearchValueSelector = _textSearchValueSelector,
                CanUserResizeColumn = options?.CanUserResizeColumn ?? true,
                CanUserSortColumn = options?.CanUserSortColumn ?? true,
                MinWidth = options.MinWidth,
                MaxWidth = options.MaxWidth,
                CompareAscending = options?.CompareAscending ?? CompareAscending,
                CompareDescending = options?.CompareDescending ?? CompareDescending,
                BeginEditGestures = options?.BeginEditGestures ?? BeginEditGestures.Default
            });
        }
    }
}
