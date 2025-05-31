using Avalonia.Data;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace FinanceManager.Utils
{
    public static class BindingExtentions
    {
        private class Dummy : StyledElement
        {
            public static readonly AvaloniaProperty ValueProperty = AvaloniaProperty.Register<Dummy, object>("Value");
        }

        private static readonly Dummy _dummy = new Dummy();

        public static object GetValue(object element, string propertyPath)
        {
            Binding binding = new Binding(propertyPath)
            {
                Mode = BindingMode.OneTime,
                Source = element,
            };
            lock (_dummy)
            {
                _dummy.DataContext = element;
                using BindingExpressionBase _ = _dummy.Bind(Dummy.ValueProperty, binding);
                return _dummy.GetValue(Dummy.ValueProperty);
            }
        }

        public static object GetValue(this IBinding binding, object element)
        {
            lock (_dummy)
            {
                _dummy.DataContext = element;
                using BindingExpressionBase _ = _dummy.Bind(Dummy.ValueProperty, binding);
                return _dummy.GetValue(Dummy.ValueProperty);
            }
        }

        public static IDisposable SetValue(this IBinding binding, object element, object value)
        {
            lock (_dummy)
            {
                _dummy.DataContext = element;
                using BindingExpressionBase _ = _dummy.Bind(Dummy.ValueProperty, binding);
                return _dummy.SetValue(Dummy.ValueProperty, value);
            }
        }

        public static string GetPath(this IBinding binding)
        {
            if (binding is Binding regularBinding)
            {
                return regularBinding.Path;
            }
            else if (binding is CompiledBindingExtension compiledBinding)
            {
                return compiledBinding.Path.ToString();
            }
            throw new NotSupportedException();
        }

        public static List<(string name, Type type)> GetPropertyPath<TIn, TOut>(this Expression<Func<TIn, TOut>> expression)
        {
            List<(string name, Type type)> result = new List<(string, Type)>();
            Expression currentExpression = expression.Body;
            while (currentExpression != null)
            {
                switch (currentExpression)
                {
                    case MemberExpression memberExpression:
                        if (memberExpression.Member is PropertyInfo propertyInfo)
                        {
                            result.Add((propertyInfo.Name, propertyInfo.PropertyType));
                        }
                        else if (memberExpression.Member is FieldInfo fieldInfo)
                        {
                            result.Add((fieldInfo.Name, fieldInfo.FieldType));
                        }
                        currentExpression = memberExpression.Expression;
                        break;
                    case MethodCallExpression methodExpression:
                        if (methodExpression.Object == null)
                        {
                            throw new NotSupportedException("Static methods not supported.");
                        }
                        currentExpression = methodExpression.Object;
                        result.Add((methodExpression.Method.Name, methodExpression.Method.ReturnType));
                        break;
                    case ParameterExpression parameterExpression:
                        currentExpression = null;
                        result.Add((parameterExpression.Name, parameterExpression.Type));
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            result.Reverse();
            return result;
        }

    }
}
