using MiP.ObjectDump.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.FormattableString;

namespace MiP.ObjectDump.Formatter
{
    public class HtmlFormatter
    {
        private TextWriter _writer;

        public HtmlFormatter(TextWriter writer)
        {
            _writer = writer;
        }

        public void WriteObject(DObject item)
        {
            Format((dynamic)item);
        }

        private void Format(DObject nullValue)
        {
            throw new InvalidOperationException($"Value of {nullValue.GetType().Name} is not supported, forgot (dynamic) dispatch?");
        }

        private void Format(DNull nullValue)
        {
            Write("<span class='null'>null</span>");
        }

        private void Format(DValue value)
        {
            Write(value.Value.Replace("\r\n","<br>").Replace("\n","<br>"));
        }

        private void Format(DError error)
        {
            Write($"<span class='error'>{error.Error}</span>");
        }

        private void Format(CyclicReference reference)
        {
            Write("<table>");

            Write("<tr>");
            Write("<td class='type'>");
            WriteString($"Cyclic reference {reference.TypeHeader}");
            Write("</td>");
            Write("</tr>");

            Write("<tr>");
            Write("<td class='cyclic'>");
            WriteString($"To: {reference.Reference}");
            Write("</td>");
            Write("</tr>");

            Write("</table>");
        }

        private void Format(DComplex complex)
        {
            Write("<table>");

            Write("<tr>");
            Write("<td colspan='2' class='type'>");
            WriteString(complex.TypeHeader);
            Write("</td>");
            Write("</tr>");

            foreach (var property in complex.Properties)
            {
                Write("<tr>");

                Write(Invariant($"<td class='property'>{property.Name}</td>"));

                Write("<td class='value'>");

                WriteObject(property.Value);

                Write("</td>");
                Write("</tr>");
            }

            Write("</table>");
        }

        private void Format(DArray array)
        {
            int columnCount = array.Columns.Count;
            string colspan = columnCount > 1 ? $" colspan='{columnCount}'" : string.Empty;

            Write("<table>");

            Write("<tr>");
            Write($"<td{colspan} class='type'>");
            WriteString(array.TypeHeader);
            Write("</td>");
            Write("</tr>");

            WriteColumnHeaders(array.Columns);

            foreach (var item in array.Items)
            {
                Write("<tr>");

                WriteArrayItem((dynamic)item, array.Columns, colspan);

                Write("</tr>");
            }

            Write("</table>");
        }

        private void WriteColumnHeaders(IReadOnlyDictionary<string, int> columns)
        {
            if (columns.Count <= 1)
                return;

            Write("<tr>");

            foreach (var column in columns.OrderBy(c => c.Value))
            {
                Write("<td class='arrayColumn'>");
                WriteString(column.Key);
                Write("</td>");
            }

            Write("</tr>");
        }

        private void WriteArrayItem(DObject item, IReadOnlyDictionary<string, int> columns, string colspan)
        {
            Write($"<td{colspan}>");
            Format((dynamic)item);
            Write("</td>");
        }

        private void WriteArrayItem(DComplex complex, IReadOnlyDictionary<string, int> columns, string colspan)
        {
            foreach (var column in columns.OrderBy(c => c.Value))
            {
                var property = complex.Properties.FirstOrDefault(p => p.Name == column.Key);
                if (property != null)
                {
                    Write("<td>");
                    Format((dynamic)property.Value);
                    Write("</td>");
                }
                else
                {
                    Write("<td></td>"); // add empty column
                }
            }
        }

        private void Write(string rawHtml)
        {
            _writer.WriteLine(rawHtml);
        }

        private void WriteString(string str)
        {
            Write(System.Web.HttpUtility.HtmlEncode(str).Replace("\r\n","\n").Replace("\n","<br>"));
        }
    }
}
