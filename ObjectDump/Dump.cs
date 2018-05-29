using MiP.ObjectDump.Formatter;
using MiP.ObjectDump.Reflection;
using System.IO;

namespace MiP.ObjectDump
{
    public static class Dump
    {
        public static string ToHtml(object item, int depth = 5)
        {
            using (StringWriter writer = new StringWriter())
            {
                var formatter = new HtmlFormatter(writer);

                Html.WriteBeginHtml(writer, true, "Title TBD", Html.GetDefaultStyles());

                Reflector reflector = new Reflector();
                var dobj = reflector.GetDObject(item, depth);

                formatter.WriteObject(dobj);

                Html.WriteEndHtml(writer);

                return writer.ToString();
            }
        }
    }
}
