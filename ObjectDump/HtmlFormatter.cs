using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using static System.FormattableString;

namespace MiP.ObjectDump
{
    public class HtmlFormatter
    {
        public void Format(JObject item, TextWriter writer)
        {
            writer.WriteLine("<table>");

            JProperty typeProperty = item.Property("$type");
            if (typeProperty != null)
            {
                writer.WriteLine("<tr>");
                writer.WriteLine("<td colspan='2' class='typeName'>");
                writer.WriteLine(typeProperty.Value);
                writer.WriteLine("</td>");

                writer.WriteLine("</tr>");
            }

            foreach (JProperty property in item.Properties().Where(p => p.Name != "$type"))
            {
                writer.WriteLine("<tr>");

                writer.WriteLine(Invariant($"<td class='propertyName'>{property.Name}</td>"));

                writer.WriteLine("<td class='value'>");

                if (property.Value is JValue value)
                {
                    if (ReferenceEquals(value.Value, null))
                    {
                        writer.WriteLine("<span class='null'>null</span>");
                    }
                    else
                    {
                        writer.WriteLine(value.Value); // TODO: do something to provide custom formatting
                    }
                }

                if (property.Value is JObject obj)
                    Format(obj, writer);

                if (property.Value is JArray array)
                    Format(array, writer);

                writer.WriteLine("</td>");
                writer.WriteLine("</tr>");
            }

            writer.WriteLine("</table>");
        }

        public void Format(JArray array, TextWriter writer)
        {


            // get all column names by searching all objects in array for properties, and give each propertyname a column number.
            // write out column names
            // write out objects
        }
    }
}
