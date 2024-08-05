using System.Collections.Concurrent;
using System.Collections.Generic;
using MiP.ObjectDump.Formatter;
using MiP.ObjectDump.Reflection;
using System.IO;
using System.Linq;
using System.Threading;

namespace MiP.ObjectDump
{
    public static class HtmlDump
    {
        public static string ToHtml(object item, int depth = 5, string title = null)
        {
            var formatter = new HtmlFormatter();
            var reflector = new Reflector();
            var obj = reflector.GetDObject(item, depth);
            formatter.WriteObject(obj);
            return Html.CreateHtml(false, title ?? "Dump输出网页", Html.GetDefaultStyles(), formatter.GetChildren()).ToString();
        }

        private static long defaultIndex = 1;
        private static long minIndex = 0;
        private static ConcurrentBag<(long index, object item, string label)> defaultBag = new();

        public static T DumpToHtml<T>(this T item, string label = null)
        {
            defaultBag.Add((Interlocked.Increment(ref defaultIndex), item,label));
            return item;
        }

        public static void SaveToHtmlFile(string file, int depth = 5, string title = null)
        {
            File.WriteAllText(file, ToHtml(defaultBag.OrderBy(x=>x.index).Select(x=>(x.item,x.label)), depth, title));
        }
        public static string ToHtml(IEnumerable<(object item, string label)> objs, int depth = 5, string title = null)
        {
            var formatter = new HtmlFormatter();
            foreach (var item in objs)
            {
                var reflector = new Reflector();
                var obj = reflector.GetDObject(item, depth);
                formatter.WriteObject(obj,item.label);
            }
            
            return Html.CreateHtml(false, title ?? "Dump输出网页", Html.GetDefaultStyles(), formatter.GetChildren()).ToString();
        }
    }
}
