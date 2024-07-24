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

        public void WriteObject(DObject item, string label = null)
        {
            if (label != null)
            {
                Write(
                    $"""
                     <div class="headingpresenter"><h1 class="headingpresenter">{label}</h1><div>
                     """
                );
                WriteString(label);
                Write("</h1><div>");
            }

            Format((dynamic)item);
            if (label != null)
                Write("</div></div>");
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
            var tid = tableId++;
            WriteTableTag(tid,$" colspan='2'",$"Cyclic reference {reference.TypeHeader}");

            Write(
                $"""
                 <th title="string">
                 """);
            WriteString($"To: {reference.Reference}");
            Write(
                $"""
                 <span class="meta"></span></th>
                 """);

            Write("</table>");
        }

        private void Format(DComplex complex)
        {
            var tid = tableId++;
            WriteTableTag(tid,$" colspan='2'",complex.TypeHeader);

            foreach (var property in complex.Properties)
            {
                Write("<tr>");
                Write(
                    $"""
                     <th class='member' title="{property.Value.GetType()}">
                     """);
                WriteString(property.Name);
                Write(
                    $"""
                     <span class="meta"></span></th>
                     """);

                Write("<td class='value'>");

                WriteObject(property.Value);

                Write("</td>");
                Write("</tr>");
            }

            Write("</table>");
        }

        private int tableId = 1;
        private void Format(DArray array)
        {
            int columnCount = array.Columns.Count;
            string colspan = columnCount > 1 ? $" colspan='{columnCount}'" : string.Empty;
            var tid = tableId++;
            WriteTableTag(tid,colspan, array.TypeHeader);
            

            WriteColumnHeaders(array.Columns);

            foreach (var item in array.Items)
            {
                Write("<tr>");

                WriteArrayItem((dynamic)item, array.Columns, colspan);

                Write("</tr>");
            }

            Write("</table>");
        }

        private void WriteTableTag(int tid,string colspan,string typeHeader)
        {
            Write($"<table id=\"t{tid}\" style=\"border-bottom: 2px solid;\">");
            Write(
                $"""
                 <thead>
                     <tr>
                         <td class="typeheader" {colspan}><a class="typeheader" onclick="return toggle('t{tid}');"><span
                                     class="arrow-up" id="t{tid}ud"></span>{typeHeader}</a><span
                                 class="meta"></span></td>
                     </tr>
                 </thead>
                 """
            );
        }

        private void WriteColumnHeaders(IReadOnlyDictionary<string, int> columns)
        {
            if (columns.Count <= 1)
                return;

            Write("<tr>");

            foreach (var column in columns.OrderBy(c => c.Value))
            {
                Write(
                    $"""
                    <th title="string">
                    """);
                WriteString(column.Key);
                Write(
                    $"""
                    <span class="meta"></span></th>
                    """);
            }

            Write("</tr>");
        }

        private void WriteArrayItem(DObject item, IReadOnlyDictionary<string, int> columns, string colspan)
        {
            var isNumber = item is DValue dv && dv.Value != null && dv.Value.All(c => (c >= '0' && c <= '9') || c == '.');
            Write(isNumber?$"<td{colspan} class=\"n\">":$"<td{colspan}>");
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
