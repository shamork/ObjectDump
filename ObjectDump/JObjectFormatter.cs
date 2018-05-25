using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.FormattableString;

namespace MiP.ObjectDump
{
    public class JObjectFormatter
    {
        private readonly TextWriter _writer;

        public JObjectFormatter(TextWriter writer)
        {
            _writer = writer;
        }

        public void Format(JToken token)
        {
            if (token is JObject obj)
                Format(obj);
            if (token is JArray arr)
                Format(arr);
            if (token is JValue val)
            {
                if (val.Type == JTokenType.Null)
                {
                    Write("<span class='null'>null</span>");
                }
                else if (val.Type == JTokenType.String && string.Empty.Equals(val.Value))
                {
                    Write("<span class='stringEmptyClass'>string<span class='stringEmptyProperty'>.Empty</span></span>");
                }
                else
                {
                    Write(val); // TODO: do something to provide custom formatting
                }
            }
        }

        private void Format(JObject item)
        {
            Write("<table>");

            JProperty typeProperty = item.Property("$type");
            if (typeProperty != null)
            {
                Write("<tr>");
                Write("<td colspan='2' class='type'>");
                Write(typeProperty.Value);
                Write("</td>");

                Write("</tr>");
            }

            foreach (JProperty property in item.Properties().Where(p => p.Name != "$type"))
            {
                Write("<tr>");

                if (property.Name != "$values")
                    Write(Invariant($"<td class='property'>{property.Name}</td>"));

                Write("<td class='value'>");

                Format(property.Value);

                Write("</td>");
                Write("</tr>");
            }

            Write("</table>");
        }

        private void Format(JArray array)
        {
            Dictionary<string, int> columns = GetArrayColumns(array);

            // write out table with column headers
            Write("<table>");
            WriteArrayColumnHeader(columns);

            // write out in order.
            // - if object, use columns
            // - if not, use column span, and write via Format(JToken)
            foreach (JToken token in array)
            {
                Write("<tr>");
                // we use a dynamic dispatch here to find the correct method base on the runtime-type of token.
                WriteArrayItem((dynamic)token, columns);
                Write("</tr>");
            }

            Write("</table>");
        }

        private static Dictionary<string, int> GetArrayColumns(JArray array)
        {
            Dictionary<string, int> columns = new Dictionary<string, int>();
            int index = 0;

            // get all column names by searching all JObjects in array for properties, 
            foreach (JObject jObject in array.OfType<JObject>())
            {
                foreach (var property in jObject.Properties())
                {
                    // give each propertyname a column number.
                    string name = property.Name;
                    if (name == "$type" || name == "$values")
                        continue;

                    if (!columns.ContainsKey(name))
                    {
                        columns[name] = index++;
                    }
                }
            }

            if (index == 0) // thats when no JObjects were in the array, just tokens or other arrays.
            {
                columns[$"[] ({array.Count} items)"] = index++; // create a fake column;
            }

            return columns;
        }

        private void WriteArrayColumnHeader(Dictionary<string, int> columns)
        {
            Write("<tr>");

            foreach (var column in columns.OrderBy(p => p.Value))
            {
                Write("<td class='arrayColumn'>");
                Write(column.Key);
                Write("</td>");
            }

            Write("</tr>");
        }

        private void WriteArrayItem(JValue value, IDictionary<string, int> columns)
        {
            string colspan = columns.Count > 1 ? $" colspan='{columns.Count}'" : string.Empty;
            Write(Invariant($"<td{colspan}>"));

            Write(value);

            Write("</td>");
        }

        private void WriteArrayItem(JObject jObject, IDictionary<string, int> columns)
        {
            JProperty values = jObject.Property("$values");
            if (values != null)
            {
                WriteArrayItem((dynamic)values.Value, columns);
                return;
            }

            foreach (var column in columns.OrderBy(c => c.Value))
            {
                if (jObject.TryGetValue(column.Key, out JToken token))
                {
                    Write("<td>");
                    Format(token);
                    Write("</td>");
                }
                else
                {
                    Write("<td></td>"); // empty column
                }
            }
        }

        private void WriteArrayItem(JArray array, IDictionary<string, int> columns)
        {
            string colspan = columns.Count > 1 ? $" colspan='{columns.Count}'" : string.Empty;
            Write(Invariant($"<td{colspan}>"));

            Format(array);

            Write("</td>");
        }

        private void Write(JValue val)
        {
            Write(val.Value.ToString());
        }

        private void Write(JToken value)
        {
            Write(value.ToString());
        }

        private void Write(string rawHtml)
        {
            _writer.WriteLine(rawHtml);
        }
    }
}
