using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.IO;

namespace MiP.ObjectDump
{
    public static class Dump
    {
        public static string ToHtml<T>(T item)
        {
            var serializer = new JsonSerializer()
            {
                Converters = { new StringEnumConverter() },
                TypeNameHandling = TypeNameHandling.All
            };

            JToken token = JToken.FromObject(item, serializer);
            using (StringWriter writer = new StringWriter())
            {
                HtmlFormatter formatter = new HtmlFormatter(writer);

                Html.WriteBeginHtml(writer, true, "Title TBD", Html.GetDefaultStyles());

                formatter.Format(token);

                Html.WriteEndHtml(writer);

                return writer.ToString();
            }
        }
    }
}
