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
            HtmlFormatter formatter = new HtmlFormatter();

            var serializer = new JsonSerializer()
            {
                Converters = { new StringEnumConverter() },
                TypeNameHandling = TypeNameHandling.All
            };

            JObject jobject = JObject.FromObject(item, serializer);
            using (StringWriter writer = new StringWriter())
            {
                Html.WriteBeginHtml(writer, true, "Title TBD", Html.GetDefaultStyles());

                formatter.Format(jobject, writer);

                Html.WriteEndHtml(writer);

                return writer.ToString();
            }
        }
    }
}
