using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System;
using System.Linq.Expressions;
using Avalonia;
using Avalonia.Layout;
using Avalonia.Data;
using System.Collections;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml.MarkupExtensions;
using System.Collections.Generic;
using System.Reflection;
using FinanceManager.Utils.Converters;
using FinanceManager.Utils;

namespace FinanceManager.TreeDataGridAddons
{
    public class ComboBoxColumn<TModel, TValue> : CustomColumn<TModel, TValue> where TModel : class
    {
        public event EventHandler<SelectionChangedEventArgs> OnSelectionChanged;
        public event EventHandler<ItemsSourceRequiredEventArgs> OnItemsSourceRequired;

        private static readonly Func<TValue, string> DEFAULT_DISPLAY_MEMBER_GETTER = (value) => value?.ToString();

        private Func<TValue, string> _displayMemberGetter = DEFAULT_DISPLAY_MEMBER_GETTER;
        private GenericConverter<TValue, string> _displayMemberConverter = new GenericConverter<TValue, string>(DEFAULT_DISPLAY_MEMBER_GETTER);
        public Func<TValue, string> DisplayMemberGetter
        {
            get => _displayMemberGetter;
            set
            {
                _displayMemberGetter = value;
                _displayMemberConverter = new GenericConverter<TValue, string>(value);
            }
        }
        public IEnumerable ItemsSource { get; set; }

        public ComboBoxColumn(object header, LBinding<TModel, TValue> binding) : base(header, binding)
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
                    Padding = new Thickness(5, 0, 0, 0),
                    MinHeight = STANDARD_ROW_HEIGHT,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Stretch
                };

                textblock.Bind(TextBlock.DataContextProperty, ValueBinding);
                textblock.Bind(TextBlock.TextProperty, new Binding("")
                {
                    Converter = _displayMemberConverter
                });

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
                ComboBox comboBox = new()
                {
                    MinHeight = STANDARD_ROW_HEIGHT,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    BorderThickness = new Thickness(0),
                    DataContext = model,
                };

                if (DisplayMemberGetter != DEFAULT_DISPLAY_MEMBER_GETTER)
                {
                    comboBox.DisplayMemberBinding = new Binding()
                    {
                        Converter = _displayMemberConverter
                    };
                }

                if (ItemsSource != null)
                {
                    comboBox.ItemsSource = ItemsSource;
                }
                else
                {
                    ItemsSourceRequiredEventArgs args = new()
                    {
                        ItemOnRow = model
                    };
                    OnItemsSourceRequired?.Invoke(this, args);
                    if (args.ItemsSource != null)
                    {
                        comboBox.ItemsSource = args.ItemsSource;
                    }
                    else
                    {
                        Type selectedItemType = _binding.Expression.ReturnType;
                        if (selectedItemType?.IsEnum ?? false)
                        {
                            comboBox.ItemsSource = Enum.GetValues(selectedItemType);
                        }
                    }
                }

                comboBox.Bind(ComboBox.SelectedItemProperty, ValueBinding);
                comboBox.SelectionChanged += (object sender, SelectionChangedEventArgs e) =>
                {
                    //if(comboBox.SelectedItem is TValue newValue)
                    //{
                    //    _setter(model, newValue);
                    //}
                    //else
                    //{
                    //    _setter(model, default);
                    //}
                    OnSelectionChanged?.Invoke(sender, e);
                };

                return comboBox;
            });
            return cellEditingTemplate;
        }

        protected override int CompareAscending(TModel first, TModel second)
        {
            TValue firstValue = _compiledPropertyGetter(first);
            TValue secondValue = _compiledPropertyGetter(second);

            if (firstValue is IComparable<TValue> firstComparable)
            {
                return firstComparable.CompareTo(secondValue);
            }
            else
            {
                return DisplayMemberGetter(firstValue).CompareTo(DisplayMemberGetter(secondValue));
            }
        }

        public class ItemsSourceRequiredEventArgs
        {
            public object ItemOnRow { get; set; }
            public IEnumerable ItemsSource { get; set; }
        }
    }
}
