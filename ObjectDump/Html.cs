using System.IO;

namespace MiP.ObjectDump
{
    internal static class Html
    {
        public static string GetDefaultStyles()
        {
            using (StreamReader reader = new StreamReader(
                typeof(Html).Assembly.GetManifestResourceStream(typeof(Html), "styles.css")))
            {
                return reader.ReadToEnd();
            }
        }

        public static void WriteBeginHtml(TextWriter writer, bool docType, string title, string styles)
        {
            if (docType)
                writer.Write("<!DOCTYPE html>");

            writer.Write("<html><head><meta charset='utf-8'><title>");
            writer.Write(title);
            writer.Write("</title><style>");
            writer.Write(styles);
            writer.Write("</style></head><body>");
        }

        public static void WriteEndHtml(TextWriter writer)
        {
            writer.Write("</body></html>");
        }
    }
}
