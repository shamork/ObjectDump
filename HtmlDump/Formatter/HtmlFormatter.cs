﻿using MiP.ObjectDump.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hyperlinq;

namespace MiP.ObjectDump.Formatter
{
    public class HtmlFormatter
    {
        private int tableId = 1;
        private List<object> Children = new();
        public object[] GetChildren() => Children.ToArray();

        public HtmlFormatter()
        {
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
                var children = new[]
                    {
                        H.h1(h => h.css("headingpresenter"), label),
                        H.div((object[])Format((dynamic)item))
                    }
                    .ToArray();
                var divO = H.div(d => d.css("headingpresenter"), children);
                Children.Add(divO);
                return;
            }

            var o = (object[])Format((dynamic)item);
            if (o == null || o.Length == 0) return;
            if (o.Length == 1)
            {
                var e = H.div(d => d.css("spacer"), o[0]);
                Children.Add(e);
                return;
            }

            Children.Add(H.div(d => d.css("spacer"), o));
        }
        // private object[] WriteObjectInner(DObject item)
        // {
        //     return Format(item);
        // }

        private object[] Format(DObject nullValue)
        {
            throw new InvalidOperationException($"Value of {nullValue.GetType().Name} is not supported, forgot  dispatch?");
        }

        private object[] Format(DNull nullValue)
        {
            return [H.span(s => s.css("null"), "null")];
        }

        private object[] Format(DValue value)
        {
            return [value.Value];
        }

        private object[] Format(DError error)
        {
            return [H.span(s => s.css("error"), error.Error)];
        }

        private object[] Format(CyclicReference reference)
        {
            var tid = tableId++;
            return CreateTable(
                tid,
                2,
                $"Cyclic reference {reference.TypeHeader}",
                H.th(th => th.title("string"),
                    $"To: {reference.Reference}",
                    H.span(s => s.css("meta")))
            );
        }

        private object[] Format(DComplex complex)
        {
            var tid = tableId++;
            // WriteTableTag(tid, $" colspan='2'", complex.TypeHeader);
            var arr = complex.Properties.Select(property =>
            {
                var tr = (object)H.tr(
                    H.th(th => th.css("member").title(property.Value is DValue dv ? dv.ValueType : property.Value.GetType()), property.Name, H.span(s => s.css("meta"))),
                    H.td(td => td.css("value"), (object[])Format((dynamic)property.Value))
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

        private object[] Format(DArray array)
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
                            return H.tr(FormatArrayItem(dc, array.Columns, columnCount).Select(x => (object)x).ToArray());
                        return H.tr(
                            H.td(td => td.colspan(columnCount).setCls(GetCss(item)), (object[])Format((dynamic)item))
                        );
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

        private HElement[] FormatArrayItem(DComplex complex, IReadOnlyDictionary<string, int> columns, int colSpan)
        {
            return columns.OrderBy(c => c.Value).Select(column =>
            {
                var property = complex.Properties.FirstOrDefault(p => p.Name == column.Key);
                if (property != null)
                {
                    return H.td(td => td.setCls(GetCss(property.Value)), (object[])Format((dynamic)property.Value));
                }
                else
                {
                    return H.td(); // add empty column
                }
            }).ToArray();
        }

        private static bool IsNumber(DObject dobj)
        {
            return dobj is DValue dv && Reflector.IsNumber(dv.ValueType);
        }
        private static (bool isBoolean,bool True) isBool(DObject dobj)
        {
            var isBoolean= dobj is DValue dv && dv.ValueType==typeof(Boolean);
            return (isBoolean, isBoolean && dobj is DValue dv1 && dv1.Value == "True");
        }

        private object[] CreateTable(int tid, int colspan, string typeHeader, params object[] children)
        {
            var mergedChildren = new[]
            {
                H.thead(
                    H.tr(
                        H.td(td => td.css("typeheader").colspan(colspan < 1 ? 1 : colspan),
                            H.a(a => a.css("typeheader").onclick($"return toggle('t{tid}');"), H.span(s => s.css("arrow-up").id($"t{tid}ud")), typeHeader),
                            CreateMetaSpan()
                        )
                    )
                )
            }.Concat(children).ToArray();
            return
            [
                H.div(
                    H.table(
                        t => t.id($"t{tid}").style("border-bottom: 2px solid;"),
                        mergedChildren
                    )
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

        private static HElement CreateMetaSpan()
        {
            return H.span(s => s.css("meta"));
        }

        private object[] CreateColumnHeaders(IReadOnlyDictionary<string, int> columns)
        {
            if (columns.Count <= 1)
                return [];

            return
            [
                H.tr(
                    columns.OrderBy(c => c.Value).Select(column => H.th(th => th.title("string"), column.Key, CreateMetaSpan()))
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

        // private object[] WriteArrayItem(DComplex complex, IReadOnlyDictionary<string, int> columns, string colspan)
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
    }

    internal static class Ext
    {
        public static IChain<T> onclick<T>(this IChain<T> attributes, object value) where T : HAttribute, new()
        {
            return attributes.Join<T>((T)new T().Create("onclick", value));
        }

        /// <summary>
        /// 如果clsName为空，则无任何副作用
        /// </summary>
        /// <param name="item"></param>
        /// <param name="clsName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IChain<T> setCls<T>(this IChain<T> item, string clsName) where T : HAttribute, new()
        {
            if (!string.IsNullOrEmpty(clsName)) return item.css(clsName);
            return item;
        }
    }
}