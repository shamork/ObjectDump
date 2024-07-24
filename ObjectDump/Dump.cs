using System.Collections.Concurrent;
using System.Collections.Generic;
using MiP.ObjectDump.Formatter;
using MiP.ObjectDump.Reflection;
using System.IO;

namespace MiP.ObjectDump
{
    public static class Dump
    {
        public static string ToHtml(object item, int depth = 5, string title = null)
        {
            using (StringWriter writer = new StringWriter())
            {
                var formatter = new HtmlFormatter(writer);

                Html.WriteBeginHtml(writer, true, title ?? "Dump输出网页", Html.GetDefaultStyles());

                Reflector reflector = new Reflector();
                var dobj = reflector.GetDObject(item, depth);

                formatter.WriteObject(dobj);

                Html.WriteEndHtml(writer);

                return writer.ToString();
            }
        }

        private static ConcurrentBag<(object item, string label)> defaultBag = new();

        public static T DumpToHtml<T>(this T item, string label = null)
        {
            defaultBag.Add((item,label));
            return item;
        }

        public static void SaveToHtmlFile(string file, int depth = 5, string title = null)
        {
            File.WriteAllText(file, ToHtml(defaultBag, depth, title));
        }
        public static string ToHtml(IEnumerable<(object item, string label)> objs, int depth = 5, string title = null)
        {
            using (StringWriter writer = new StringWriter())
            {
                return ToHtml(objs, depth, title, writer);
            }
        }

        public static string ToHtml(IEnumerable<(object item, string label)> objs, int depth, string title, StringWriter writer)
        {
            var formatter = new HtmlFormatter(writer);

            Html.WriteBeginHtml(writer, true, title ?? "Dump输出网页", Html.GetDefaultStyles());

            Reflector reflector = new Reflector();
            foreach (var item in objs)
            {
                var dobj = reflector.GetDObject(item.item, depth);
                formatter.WriteObject(dobj, item.label);
            }

            Html.WriteEndHtml(writer);

            return writer.ToString();
        }
    }
}
