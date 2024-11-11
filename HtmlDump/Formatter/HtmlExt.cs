using HtmlAgilityPack;

namespace MiP.ObjectDump.Formatter;

internal static class HtmlExt
{
    public static HtmlNode onclick(this HtmlNode node, string value)
    {
        node.Attributes.Add("onclick", value);
        return node;
    }

    public static HtmlNode AddCls(this HtmlNode node, string value)
    {
        if (value != null)
        {
            node.AddClass(value);
        }

        return node;
    }

    public static HtmlNode AddStyle(this HtmlNode node, string value)
    {
        if (value != null)
        {
            node.Attributes.Add("style", value);
        }

        return node;
    }

    public static HtmlNode AddAttr(this HtmlNode node, string name, string value)
    {
        node.Attributes.Add(name, value);
        return node;
    }

    public static HtmlNode AddText(this HtmlNode node, string value)
    {
        if (value != null)
        {
            node.ChildNodes.Add(node.OwnerDocument.CreateText(value));
        }

        return node;
    }

    public static HtmlNode AddChildren(this HtmlNode node, params HtmlNode[] children)
    {
        if (children != null && children.Length != 0)
        {
            foreach (var c in children)
            {
                node.ChildNodes.Add(c);
            }
        }

        return node;
    }

    public static HtmlNode CreateCDataElement(this HtmlDocument doc, string name, string type = null, string value = null)
    {
        var e = doc.CreateElement(name);
        if (type != null)
        {
            e.AddAttr("type", type);
        }

        if (value != null)
            e.InnerHtml = value;
        return e;
    }

    public static HtmlNode CreateStyle(this HtmlDocument doc, string value = null)
    {
        return doc.CreateCDataElement("style", "text/css", value);
    }

    public static HtmlNode CreateMeta(this HtmlDocument doc, string httpEquiv = null, string content = null)
    {
        var e = doc.CreateElement("meta");
        if (httpEquiv != null)
            e.AddAttr("http-equiv", httpEquiv);
        if (content != null)
            e.AddAttr("content", content);
        return e;
    }
    public static HtmlNode CreateTitle(this HtmlDocument doc, string title)
    {
        var e = doc.CreateElement("title");
        if (title != null)
            e.AddText(title);
        return e;
    }

    public static HtmlNode CreateDiv(this HtmlDocument doc)
    {
        return doc.CreateElement("div");
    }

    public static HtmlNode CreateText(this HtmlDocument doc, string text)
    {
        return doc.CreateTextNode(HtmlDocument.HtmlEncode(text).Replace(" ","&ensp;"));
    }

    public static HtmlNode CreateTextNoEscape(this HtmlDocument doc, string text)
    {
        return doc.CreateTextNode(text);
    }

    public static HtmlNode CreateSpan(this HtmlDocument doc)
    {
        return doc.CreateElement("span");
    }
    public static HtmlNode CreateSpan(this HtmlDocument doc,string clsName,string text)
    {
        return doc.CreateElement("span").AddCls(clsName).AddText(text);
    }
    public static HtmlNode CreateScript(this HtmlDocument doc, string value, string language = "JavaScript", string type = "text/javascript")
    {
        var e = doc.CreateCDataElement("script",type, value);
        if (language != null)
            e.AddAttr("language", language);
        if (type != null)
            e.AddAttr("type", type);
        return e;
    }
}