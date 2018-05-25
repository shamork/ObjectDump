using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using static System.FormattableString;

namespace MiP.ObjectDump
{
    public class ReflectionFormatter
    {
        private readonly TextWriter _writer;

        private IValueFormatter[] _formatters;

        public ReflectionFormatter(TextWriter writer)
        {
            _writer = writer;

            _formatters = new IValueFormatter[]
            {
                new NullFormatter(_writer),
                new StringEmptyFormatter(_writer),
                new SimpleTypeFormatter(_writer),
                new EnumFormatter(_writer),
                new ArrayFormatter(_writer, this),
                new ObjectFormatter(_writer, this)
            };
        }

        public void Format(object item)
        {
            var itemType = item?.GetType();
            foreach (var formatter in _formatters)
            {
                if (formatter.Format(item, itemType))
                    return;
            }

            _writer.WriteLine($"<span class='null'>{item}</span>");
        }
    }

    public interface IValueFormatter
    {
        bool Format(object item, Type itemType);
    }

    public abstract class ValueFormatter : IValueFormatter
    {
        private readonly TextWriter _writer;

        public ValueFormatter(TextWriter writer)
        {
            _writer = writer;
        }

        public abstract bool Format(object item, Type itemType);

        protected void Write(string rawHtml)
        {
            _writer.WriteLine(rawHtml);
        }

        private Type[] _simpleTypes =
        {
            typeof(object), typeof(bool), typeof(string),
            typeof(byte), typeof(sbyte),
            typeof(short), typeof (ushort),
            typeof(int), typeof(uint),
            typeof(long), typeof(ulong),
            typeof(decimal), typeof(double), typeof(float),
            typeof(DateTime), typeof(TimeSpan),
        };

        protected bool IsSimpleType(Type type)
        {
            return _simpleTypes.Contains(type);
        }

        protected IDictionary<string, object> GetMembers(object item, Type itemType)
        {
            var fields = itemType.GetFields(BindingFlags.Instance | BindingFlags.Public);
            var properties = itemType.GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);

            Dictionary<string, object> result = new Dictionary<string, object>();

            foreach (var field in fields)
            {
                string name = field.Name;
                object value = field.GetValue(item);
                result.Add(name, value);
            }

            foreach (var property in properties)
            {
                string name = property.Name;

                // TODO: maximum depth for catching stack overflows, or we cannot dump a type.
                // TODO: try to catch exception from property and display accordingly (use red color)
                object value = property.GetValue(item);

                result.Add(name, value);
            }

            return result;
        }
    }

    public class NullFormatter : ValueFormatter
    {
        public NullFormatter(TextWriter writer) : base(writer)
        {
        }

        public override bool Format(object item, Type itemType)
        {
            if (!ReferenceEquals(item, null))
                return false;

            Write("<span class='null'>null</span>");

            return true;
        }
    }

    public class StringEmptyFormatter : ValueFormatter
    {
        public StringEmptyFormatter(TextWriter writer) : base(writer)
        {
        }

        public override bool Format(object item, Type itemType)
        {
            if (!string.Empty.Equals(item))
                return false;

            Write("<span class='stringEmptyClass'>string<span class='stringEmptyProperty'>.Empty</span></span>");

            return true;
        }
    }

    public class SimpleTypeFormatter : ValueFormatter
    {
        public SimpleTypeFormatter(TextWriter writer) : base(writer)
        {
        }

        public override bool Format(object item, Type itemType)
        {
            if (!IsSimpleType(itemType))
                return false;

            Write(item.ToString());

            return true;
        }
    }

    public class EnumFormatter : ValueFormatter
    {
        public EnumFormatter(TextWriter writer) : base(writer)
        {
        }

        public override bool Format(object item, Type itemType)
        {
            if (!itemType.IsEnum)
                return false;

            Write(item.ToString());

            return true;
        }
    }



    public class ArrayFormatter : ValueFormatter
    {
        private readonly ReflectionFormatter _reflectionFormatter;

        public ArrayFormatter(TextWriter writer, ReflectionFormatter reflectionFormatter) : base(writer)
        {
            _reflectionFormatter = reflectionFormatter;
        }

        public override bool Format(object item, Type itemType)
        {
            List<object> list;

            if (!(item is IEnumerable enumerable))
                return false;

            list = enumerable.Cast<object>().ToList();

            var columns = GetArrayColumns(list, enumerable.GetType());

            // write out table with column headers
            Write("<table>");
            WriteArrayHeader(columns.Count, list, itemType);
            WriteArrayColumnHeader(columns);

            // write out in order.    
            foreach (object arrayItem in list)
            {
                Write("<tr>");
                WriteArrayItem(arrayItem, columns);
                Write("</tr>");
            }

            Write("</table>");

            return true;
        }

        private void WriteArrayHeader(int columnCount, List<object> list, Type listType)
        {
            string colspan = columnCount > 1 ? $" colspan='{columnCount}'" : string.Empty;
            Write("<tr>");
            Write($"<td{colspan} class='type'>");
            Write($"{listType.Name} ({list.Count} items)"); // TODO: better type naming
            Write("</td>");
            Write("</tr>");
        }

        private void WriteArrayItem(object item, IList<(int index, string columnName)> columns)
        {
            Type itemType = item.GetType();

            if (ReferenceEquals(item, null)
                || string.Empty.Equals(item)
                || IsSimpleType(itemType)
                || item is IEnumerable
                )
            {
                // if simple type or collection, use column span, and write via Format(JToken)

                string colspan = columns.Count > 1 ? $" colspan='{columns.Count}'" : string.Empty;
                Write(Invariant($"<td{colspan}>"));

                _reflectionFormatter.Format(item);
                Write("</td>");
            }
            else
            {
                // if object, use columns

                var members = GetMembers(item, itemType);

                foreach (var (index, columnName) in columns.OrderBy(c => c.index))
                {
                    if (members.TryGetValue(columnName, out object propertyValue))
                    {
                        Write("<td>");
                        _reflectionFormatter.Format(propertyValue);
                        Write("</td>");
                    }
                    else
                    {
                        Write("<td></td>"); // add empty column
                    }
                }
            }
        }

        private void WriteArrayColumnHeader(IList<(int index, string columnName)> columns)
        {
            Write("<tr>");

            foreach ((int index, string columnName) in columns.OrderBy(p => p.index))
            {
                Write("<td class='arrayColumn'>");
                Write(columnName);
                Write("</td>");
            }

            Write("</tr>");
        }

        private IList<(int index, string columnName)> GetArrayColumns(IList list, Type listType)
        {
            List<(int index, string columnName)> columns = new List<(int index, string columnName)>();
            HashSet<string> knownColumns = new HashSet<string>();
            HashSet<Type> knownTypes = new HashSet<Type>();
            int index = 0;

            // get all column names by searching all objects in array for properties, 
            foreach (object item in list)
            {
                Type itemType = item?.GetType();
                if (ReferenceEquals(item, null)
                    || string.Empty.Equals(item)
                    || IsSimpleType(itemType)
                    || itemType.IsEnum
                    || item is IEnumerable
                    )
                    continue;

                if (knownTypes.Contains(itemType)) // do each type only once // TODO: type is still done x times if it is used in x collections.
                    continue;

                var properties = GetPropertyNames(itemType);

                foreach (var property in properties)
                {
                    // give each propertyname a column number.
                    if (!knownColumns.Contains(property))
                    {
                        knownColumns.Add(property);
                        columns.Add((index++, property));
                    }
                }
            }

            return columns;
        }

        private IList<string> GetPropertyNames(Type itemType)
        {
            var fields = itemType.GetFields(BindingFlags.Instance | BindingFlags.Public);
            var properties = itemType.GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);

            List<string> result = new List<string>();

            foreach (var field in fields)
            {
                string name = field.Name;
                result.Add(name);
            }

            foreach (var property in properties)
            {
                string name = property.Name;
                result.Add(name);
            }

            return result;
        }
    }



    public class ObjectFormatter : ValueFormatter
    {
        private readonly ReflectionFormatter _formatter;

        public ObjectFormatter(TextWriter writer, ReflectionFormatter formatter) : base(writer)
        {
            _formatter = formatter;
        }

        public override bool Format(object item, Type itemType)
        {
            // check if its a collection
            // TODO: collection formatter before this one.
            if (item is IEnumerable)
                return false;
            if (itemType.GetInterfaces()
                .Any(i => i.IsGenericType 
                        && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
            { return false; }

            Write("<table>");

            Write("<tr>");
            Write("<td colspan='2' class='type'>");
            Write(itemType.ToString());
            Write("</td>");
            Write("</tr>");

            IDictionary<string, object> members = GetMembers(item, itemType);
            foreach (var pair in members)
            {
                Write("<tr>");

                Write(Invariant($"<td class='property'>{pair.Key}</td>"));

                Write("<td class='value'>");

                _formatter.Format(pair.Value);

                Write("</td>");
                Write("</tr>");
            }

            Write("</table>");

            return true;
        }
    }
}
