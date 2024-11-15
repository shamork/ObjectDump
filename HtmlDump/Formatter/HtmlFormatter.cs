﻿using MiP.ObjectDump.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;

namespace MiP.ObjectDump.Formatter
{
    public class HtmlFormatter
    {
        #region Constants

        public const string DefaultTailScript = "hideToLevel(1);";
        public const string DefaultHeadStyle=
                """
                html,body,div,span,iframe,p,pre,a,abbr,acronym,code,del,em,img,ins,q,strong,var,i,fieldset,form,label,legend,table,caption,tbody,tfoot,thead,tr,th,td,article,aside,canvas,details,figure,figcaption,footer,header,hgroup,nav,output,section,summary,time,mark,audio,video{margin:0;padding:0;border:0;vertical-align:baseline;font:inherit;font-size:100%}h1,h2,h3,h4,h5,h6{margin:.2em 0 .05em 0;padding:0;border:0;vertical-align:baseline}i,em{font-style:italic}body{margin:0.5em;font-family:Segoe UI,Verdana,sans-serif;font-size:82%;background:white}pre,code,.fixedfont{font-family:Consolas,monospace;font-size:10pt}a,a:visited{text-decoration:none;font-family:Segoe UI Semibold,sans-serif;font-weight:bold;cursor:pointer}a:hover,a:visited:hover{text-decoration:underline}span.hex{color:rgb(200,30,250);font-family:Consolas,monospace;margin-top:1px}span.hex::before{content:" 0x";color:rgb(200,200,200)}table{border-collapse:collapse;border-spacing:0;border:2px solid #4C74B2;margin:0.3em 0.1em 0.2em 0.1em}table.limit{border-bottom-color:#B56172}table.expandable{border-bottom-style:dashed}td,th{vertical-align:top;border:1px solid #bbb;margin:0}th{position:-webkit-sticky;position:sticky;top:0;z-index:2}th[scope=row]{position:-webkit-sticky;position:sticky;left:0;z-index:2}th{padding:0.05em 0.3em 0.15em 0.3em;text-align:left;background-color:#ddd;border:1px solid #777;font-size:.95em;font-family:Segoe UI Semibold,sans-serif;font-weight:bold}th.private{font-family:Segoe UI;font-weight:normal;font-style:italic}td.private{background:#f4f4ee}td.private table{background:white}td,th.member{padding:0.1em 0.3em 0.2em 0.3em;position:initial}tr.repeat>th{font-size:90%;font-family:Segoe UI Semibold,sans-serif;border:none;background-color:#eee;color:#999;padding:0.0em 0.2em 0.15em 0.3em}td.typeheader{font-size:.95em;background-color:#4C74B2;color:white;padding:0 0.3em 0.25em 0.2em}td.n{text-align:right}a.typeheader,a:link.typeheader,a:visited.typeheader,a:link.extenser,a:visited.extenser{font-family:Segoe UI Semibold,sans-serif;font-size:.95em;font-weight:bold;text-decoration:none;color:white;margin-bottom:-0.1em;float:left}a.difheader,a:link.difheader,a:visited.difheader{color:#ff8}a.extenser,a:link.extenser,a:visited.extenser{margin:0 0 0 0.3em;padding-left:0.5em;padding-right:0.3em}a:hover.extenser{text-decoration:none}span.extenser{font-size:1.1em;line-height:0.8}span.cyclic{padding:0 0.2em 0 0;margin:0;font-family:Arial,sans-serif;font-weight:bold;margin:2px;font-size:1.5em;line-height:0;vertical-align:middle}.arrow-up,.arrow-down{display:inline-block;margin:0 0.3em 0.15em 0.1em;width:0;height:0;cursor:pointer}.arrow-up{border-left:0.35em solid transparent;border-right:0.35em solid transparent;border-bottom:0.35em solid white}.arrow-down{border-left:0.35em solid transparent;border-right:0.35em solid transparent;border-top:0.35em solid white}table.group{border:none;margin:0}td.group{border:none;padding:0 0.1em}div.spacer{margin:0.6em 0}div.headingpresenter{border:none;border-left:0.17em dashed #1a5;margin:.8em 0 1em 0.1em;padding-left:.5em}div.headingcontinuation{border:none;border-left:0.2em dotted #1a5;margin:-0.4em 0 1em 0.1em;padding-left:.5em}h1.headingpresenter{border:none;padding:0 0 0.3em 0;margin:0;font-family:Segoe UI Semibold,Arial;font-weight:bold;background-color:white;color:#209020;font-size:1.1em;line-height:1}td.summary{background-color:#DAEAFA;color:black;font-size:.95em;padding:0.05em 0.3em 0.2em 0.3em}tr.columntotal>td{background-color:#eee;font-family:Segoe UI Semibold;font-weight:bold;font-size:.95em;color:#4C74B2;text-align:right}.error > table{border-color:#B56172}.error > table > thead > tr > td.summary{background-color:#F4DEE3;color:black}.error > table > thead > tr > td.typeheader{background-color:#B56172}span.graphbar{background:#DAEAFA;color:#DAEAFA;padding-bottom:1px;margin-left:-0.2em;margin-right:0.2em}a.graphcolumn,a:link.graphcolumn,a:visited.graphcolumn{color:#4C74B2;text-decoration:none;font-weight:bold;font-family:Arial;font-size:1em;line-height:1;letter-spacing:-0.2em;margin-left:0.15em;margin-right:0.2em;cursor:pointer}a.collection,a:link.collection,a:visited.collection{color:#209020}a.reference,a:link.reference,a:visited.reference{color:#0080D1}span.meta,span.null{color:#209020}span.warning{color:red}span.false{color:#888}span.true{font-weight:bold}.highlight{background:#ff8}code.xml b{color:blue;font-weight:normal}code.xml i{color:brown;font-weight:normal;font-style:normal}code.xml em{color:red;font-weight:normal;font-style:normal}span.cc{background:#666;color:white;margin:0 1.5px;padding:0 1px;font-family:Consolas,monospace;border-radius:3px}ol,ul{margin:0.7em 0.3em;padding-left:2.5em}li{margin:0.3em 0}.difadd{background:#a3f3a3;border:1px solid #88d888}.difremove{background:#ffc8c8;border:1px solid #e8b3b3}.rendering{font-style:italic;color:brown}p.scriptLog{color:#a77;background:#f8f6f6;font-family:Consolas,monospace;font-size:9pt;padding:.1em .3em}::-ms-clear{display:none}input,textarea,button,select{font-family:Segoe UI;font-size:1em;padding:.2em}button{padding:.2em .4em}input,textarea,select{margin:.15em 0}input[type="checkbox"],input[type="radio"]{margin:0 0.4em 0 0;height:0.9em;width:0.9em}input[type="radio"]:focus,input[type="checkbox"]:focus{outline:thin dotted red}.checkbox-label{vertical-align:middle;position:relative;bottom:.07em;margin-right:.5em}fieldset{margin:0 .2em .4em .1em;border:1pt solid #aaa;padding:.1em .6em .4em .6em}legend{padding:.2em .1em}

                #floating-buttons {
                    position: fixed;
                    top: 20px;
                    right: 20px;
                    display: flex;
                    flex-direction: column;
                    gap: 10px;
                    z-index: 999;
                }

                #floating-buttons button {
                    background-color: aquamarine;
                    border: none;
                    padding: 5px 10px;
                    cursor: pointer;
                }

                #floating-buttons button:hover {
                    background-color: #ddd;
                }

                """;
        

        public const string DefaultHeadScript = """
                                     function toggle(id)
                                     {
                                     	var table = document.getElementById(id);
                                     	if (table == null) return false;
                                     	var updown = document.getElementById(id + 'ud');
                                     	if (updown == null) return false;
                                     	var expand = updown.className == 'arrow-down';
                                     	updown.className = expand ? 'arrow-up' : 'arrow-down';
                                     	table.style.borderBottomStyle = expand ? 'solid' : 'dashed';
                                     	if (table.rows.length < 2 || table.tBodies.length == 0) return false;
                                     	table.tBodies[0].style.display = expand ? '' : 'none';
                                     	if (table.tHead.rows.length == 2 && !table.tHead.rows[1].id.startsWith ('sum'))
                                     		table.tHead.rows[1].style.display = expand ? '' : 'none';
                                     	if (table.tFoot != null)
                                     		table.tFoot.style.display = expand ? '' : 'none';
                                     	if (expand)
                                     		table.scrollIntoView ({behavior:'smooth', block:'nearest'});
                                     	return false;
                                     }
                                     function hideToLevel(level) {
                                     	//1000 show all
                                     	level = level || 1
                                     
                                     	try {
                                     		var tables = document.getElementsByTagName('table');
                                     		for (let table of tables)
                                     			if (table.id.startsWith('t') && Number(table.id.substring(1)) > -1) {
                                     				var depth = 1;
                                     				if (level > 0) {
                                     					var depthTest = table.parentElement;
                                     					while (depthTest != null) {
                                     						if (depthTest.tagName == 'TABLE' && depthTest.id.startsWith('t') && Number(depthTest.id.substring(1)) > -1)
                                     							depth++;
                                     						depthTest = depthTest.parentElement;
                                     					}
                                     				}
                                     				var updown = document.getElementById(table.id + 'ud');
                                     				if (updown == null) continue;
                                     				var expand = updown.className == 'arrow-down';//显示了展开箭头，说明是可展开的，当前处于隐藏状态
                                     				if (expand != (depth < level)) continue;//(depth < level)说明按需求是可显示的
                                     				updown.className = expand ? 'arrow-up' : 'arrow-down';
                                     				table.style.borderBottom = expand ? '2px solid' : 'dashed 2px';
                                     				if (table.rows.length < 2 || table.tBodies.length == 0) continue;
                                     				table.tBodies[0].style.display = expand ? '' : 'none';
                                     				if (table.tHead.rows.length == 2 && !table.tHead.rows[1].id.startsWith('sum'))
                                     					table.tHead.rows[1].style.display = expand ? '' : 'none';
                                     				if (table.tFoot != null)
                                     					table.tFoot.style.display = expand ? '' : 'none';
                                     			}
                                     		return false;
                                     	}
                                     	catch (err) {
                                     		return '!^§Error:' + err.toString();
                                     	}
                                     }
                                     """;
        #endregion
        private readonly HtmlDocument doc;
        private int tableId = 1;
        private readonly HtmlNodeCollection Children;
        private readonly List<HtmlNode> tail = new List<HtmlNode>();
        private HtmlNodeCollection Head;
        public HtmlNode[] GetChildren() => Children.ToArray();

        public HtmlFormatter(string title, string headStyle=DefaultHeadStyle, string headScript=DefaultHeadScript, string tailScript = DefaultTailScript)
        {
            doc = new HtmlDocument();
            var html = doc.CreateElement("html");
            var head = doc.CreateElement("head");
            var body = doc.CreateElement("body");
            doc.DocumentNode.ChildNodes.Add(html);
            html.ChildNodes.Add(head);
            html.ChildNodes.Add(body);
            Head = head.ChildNodes;
            Children = body.ChildNodes;
            // <meta http-equiv="Content-Type" content="text/html;charset=utf-8"/>
            // <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
            // <meta name="Generator" content="github.com/hyperlinq/hyperlinq"/>


            Head.Add(doc.CreateMeta("Content-Type", "text/html;charset=utf-8"));
            Head.Add(doc.CreateMeta("X-UA-Compatible", "IE=edge"));
            Head.Add(doc.CreateTitle(title));
            Head.Add(doc.CreateStyle(headStyle));
            Head.Add(doc.CreateScript(headScript));
            var arr =
                new[]
                {
                    doc.CreateDiv()
                        .AddAttr("id", "floating-buttons")
                        .AddChildren(
                            doc.CreateElement("button").AddAttr("type", "button").onclick("hideToLevel(1)").AddText("全部折叠"),
                            doc.CreateElement("button").AddAttr("type", "button").onclick("hideToLevel(2)").AddText("展开到1层"),
                            doc.CreateElement("button").AddAttr("type", "button").onclick("hideToLevel(3)").AddText("展开到2层"),
                            doc.CreateElement("button").AddAttr("type", "button").onclick("hideToLevel(4)").AddText("展开到3层"),
                            doc.CreateElement("button").AddAttr("type", "button").onclick("hideToLevel(1000)").AddText("全部展开")
                    )
                };
            foreach (var node in arr)
            {
                Children.Add(node);
            }

            if (tailScript != null)
            {
                tail.Add(doc.CreateScript(tailScript));
            }
        }

        HtmlNode CreateE(string tag, string cls = null, string text = null)
        {
            var h = doc.CreateElement(tag);
            if (cls != null)
                h.AddClass(cls);
            if (text != null)
                h.ChildNodes.Add(doc.CreateText(text));

            return h;
        }

        HtmlNode CreateTH(string cls = null, string title = null, string text = null, params HtmlNode[] children)
        {
            var h = CreateE("th", cls, children);
            h.Attributes.Add("title", title);
            if (text != null)
                h.ChildNodes.Add(doc.CreateText(text));
            return h;
        }

        HtmlNode CreateTd(int? colspan = null, string cls = null, params HtmlNode[] children)
        {
            var h = CreateE("td", cls, children);
            h.Attributes.Add("colspan", colspan.ToString());
            return h;
        }

        HtmlNode CreateE(string tag, string cls, params HtmlNode[] children)
        {
            var h = CreateE(tag, children);
            if (cls != null)
                h.AddClass(cls);
            return h;
        }

        HtmlNode CreateE(string tag, params HtmlNode[] children)
        {
            var h = doc.CreateElement(tag);
            if (children != null && children.Length != 0)
            {
                foreach (var c in children)
                {
                    h.ChildNodes.Add(c);
                }
            }

            return h;
        }

        /// <summary>
        /// 公开方法，加入列表中，私有方法不加入列表
        /// </summary>
        /// <param name="item"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public void WriteObject(DObject item, string label = null)
        {
            if (label != null)
            {
                var children = new HtmlNode[]
                    {
                        CreateE("h1", "headingpresenter", label),
                        CreateE("div", Format((dynamic)item))
                    }
                    .ToArray();
                var divO = CreateE("div", "headingpresenter", children);
                Children.Add(divO);
                return;
            }

            var o = (HtmlNode[])Format((dynamic)item);
            if (o == null || o.Length == 0) return;
            if (o.Length == 1)
            {
                var e = CreateE("div", "spacer", o[0]);
                Children.Add(e);
                return;
            }

            Children.Add(CreateE("div", "spacer", o));
        }
        // private HtmlNode[] WriteObjectInner(DObject item)
        // {
        //     return Format(item);
        // }

        private HtmlNode[] Format(DObject nullValue)
        {
            throw new InvalidOperationException($"Value of {nullValue.GetType().Name} is not supported, forgot  dispatch?");
        }

        private HtmlNode[] Format(DNull _)
        {
            return [CreateE("span", "null", "null")];
        }

        private HtmlNode[] Format(DValue value)
        {
            if (value.ValueType == typeof(bool))
                return [doc.CreateSpan(value.Value == "True" ? "true" : "false", value.Value)];
            return [doc.CreateText(value.Value)];
        }

        private HtmlNode[] Format(DError error)
        {
            return [CreateE("span", "error", error.Error)];
        }

        private HtmlNode[] Format(CyclicReference reference)
        {
            var tid = tableId++;
            return CreateTable(
                tid,
                2,
                $"Cyclic reference {reference.TypeHeader}",
                CreateTH("string",
                    text: $"To: {reference.Reference}",
                    children: CreateE("span", "meta"))
            );
        }

        private HtmlNode[] Format(DComplex complex)
        {
            var tid = tableId++;
            // WriteTableTag(tid, $" colspan='2'", complex.TypeHeader);
            var arr = complex.Properties.Select(property =>
            {
                var tr = CreateE("tr",
                    CreateTH("member", property.Value is DValue dv ? dv.ValueType.ToString() : property.Value.GetType().ToString(), property.Name, CreateE("span", "meta")),
                    CreateE("td", "value", (HtmlNode[])Format((dynamic)property.Value))
                );
                return tr;
            }).ToArray();
            return CreateTable(
                tid,
                2,
                complex.TypeHeader,
                arr
            );
        }

        private HtmlNode[] Format(DArray array)
        {
            int columnCount = array.Columns.Count;
            // string colspan = columnCount > 1 ? $" colspan='{columnCount}'" : string.Empty;
            var tid = tableId++;
            return CreateTable(tid, columnCount, array.TypeHeader,
                CreateColumnHeaders(array.Columns).Concat(
                    array.Items.Select(item =>
                    {
                        // Write(isNumber ? $"<td{colspan} class=\"n\">" : $"<td{colspan}>");
                        // Format(item);
                        // return Write("</td>");
                        if (item is DComplex dc)
                            return CreateE("tr", FormatArrayItem(dc, array.Columns, columnCount).Select(x => (HtmlNode)x).ToArray());
                        var td = (HtmlNode)CreateE("td", GetCss(item), Format((dynamic)item));
                        td.Attributes.Add("colspan", columnCount.ToString());
                        return CreateE("tr", td);
                    })
                ).ToArray()
            );
        }

        private string GetCss(DObject item)
        {
            var isNumber = IsNumber(item);
            (bool isBoolean, bool True) = isBool(item);
            return isBoolean
                ? (True ? "true" : "false")
                : isNumber
                    ? "n"
                    : "";
        }

        private HtmlNode[] FormatArrayItem(DComplex complex, IReadOnlyDictionary<string, int> columns, int colSpan)
        {
            return columns.OrderBy(c => c.Value).Select(column =>
            {
                var property = complex.Properties.FirstOrDefault(p => p.Name == column.Key);
                if (property != null)
                {
                    return CreateE("td", (GetCss(property.Value)), (HtmlNode[])Format((dynamic)property.Value));
                }

                return CreateE("td"); // add empty column
            }).ToArray();
        }

        private static bool IsNumber(DObject dobj)
        {
            return dobj is DValue dv && Reflector.IsNumber(dv.ValueType);
        }

        private static (bool isBoolean, bool True) isBool(DObject dobj)
        {
            var isBoolean = dobj is DValue dv && dv.ValueType == typeof(Boolean);
            return (isBoolean, isBoolean && dobj is DValue dv1 && dv1.Value == "True");
        }

        private HtmlNode[] CreateTable(int tid, int colspan, string typeHeader, params HtmlNode[] children)
        {
            var mergedChildren = new[]
            {
                CreateE("thead",
                    CreateE("tr",
                        CreateTd(colspan < 1 ? 1 : colspan, ("typeheader"),
                            CreateE("a", "typeheader")
                                .onclick($"return toggle('t{tid}');")
                                .AddChildren(CreateE("span", "arrow-up").AddAttr("id", $"t{tid}ud"))
                                .AddText(typeHeader),
                            CreateMetaSpan()
                        )
                    )
                )
            }.Concat(children).ToArray();
            return
            [
                CreateE("div",
                    CreateE("table")
                        .AddAttr("id", $"t{tid}")
                        .AddStyle("border-bottom: 2px solid;")
                        .AddChildren(mergedChildren)
                )
            ];

//             return Write(
//                 $"""
//                  <thead>
//                      <tr>
//                          <td class="typeheader" {colspan}>
//                          <a class="typeheader" onclick="return toggle('t{tid}');">
//                          <span class="arrow-up" id="t{tid}ud"></span>{typeHeader}</a>
//                                      <span class="meta"></span></td>
//                      </tr>
//                  </thead>
//                  """
//             );
        }

        private HtmlNode CreateMetaSpan()
        {
            return CreateE("span", "meta");
        }

        private HtmlNode[] CreateColumnHeaders(IReadOnlyDictionary<string, int> columns)
        {
            if (columns.Count <= 1)
                return [];

            return
            [
                CreateE("tr",
                    columns
                        .OrderBy(c => c.Value)
                        .Select(column => CreateTH(null, "string", column.Key, CreateMetaSpan()))
                        .ToArray()
                )
            ];
        }

        // private object WriteArrayItem(DObject item, IReadOnlyDictionary<string, int> columns, string colspan)
        // {
        //     var isNumber = item is DValue dv && dv.Value != null && dv.Value.All(c => (c >= '0' && c <= '9') || c == '.');
        //     Write(isNumber ? $"<td{colspan} class=\"n\">" : $"<td{colspan}>");
        //     Format(item);
        //     return Write("</td>");
        // }

        // private HtmlNode[] WriteArrayItem(DComplex complex, IReadOnlyDictionary<string, int> columns, string colspan)
        // {
        //     return columns.OrderBy(c => c.Value).Select(column => { })
        //     foreach (var column in columns.OrderBy(c => c.Value))
        //     {
        //         var property = complex.Properties.FirstOrDefault(p => p.Name == column.Key);
        //         if (property != null)
        //         {
        //             Write("<td>");
        //             Format(property.Value);
        //             Write("</td>");
        //         }
        //         else
        //         {
        //             return Write("<td></td>"); // add empty column
        //         }
        //     }
        // }

        // private object Write(string rawHtml)
        // {
        //     return _writer.WriteLine(rawHtml);
        // }
        //
        // private object WriteString(string str)
        // {
        //     return Write(System.Web.HttpUtility.HtmlEncode(str).Replace("\r\n", "\n").Replace("\n", "<br>"));
        // }


        /// <summary>
        /// 只能调用一次，不能重复调用
        /// </summary>
        /// <returns></returns>
        public string getHtmlString()
        {
            foreach (var node in tail)
            {
                Children.Add(node);
            }

            var sw = new StringWriter();
            doc.DocumentNode.WriteTo(sw);
            return sw.ToString();
        }
    }
}