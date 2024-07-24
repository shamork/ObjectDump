﻿using System.IO;

namespace MiP.ObjectDump
{
    internal static class Html
    {
        public static string GetDefaultStyles()
        {
            // using (StreamReader reader = new StreamReader(
            //     typeof(Html).Assembly.GetManifestResourceStream(typeof(Html), "styles.css")))
            // {
            //     return reader.ReadToEnd();
            // }
            return
	            """
	            html,body,div,span,iframe,p,pre,a,abbr,acronym,code,del,em,img,ins,q,strong,var,i,fieldset,form,label,legend,table,caption,tbody,tfoot,thead,tr,th,td,article,aside,canvas,details,figure,figcaption,footer,header,hgroup,nav,output,section,summary,time,mark,audio,video{margin:0;padding:0;border:0;vertical-align:baseline;font:inherit;font-size:100%}h1,h2,h3,h4,h5,h6{margin:.2em 0 .05em 0;padding:0;border:0;vertical-align:baseline}i,em{font-style:italic}body{margin:0.5em;font-family:Segoe UI,Verdana,sans-serif;font-size:82%;background:white}pre,code,.fixedfont{font-family:Consolas,monospace;font-size:10pt}a,a:visited{text-decoration:none;font-family:Segoe UI Semibold,sans-serif;font-weight:bold;cursor:pointer}a:hover,a:visited:hover{text-decoration:underline}span.hex{color:rgb(200,30,250);font-family:Consolas,monospace;margin-top:1px}span.hex::before{content:" 0x";color:rgb(200,200,200)}table{border-collapse:collapse;border-spacing:0;border:2px solid #4C74B2;margin:0.3em 0.1em 0.2em 0.1em}table.limit{border-bottom-color:#B56172}table.expandable{border-bottom-style:dashed}td,th{vertical-align:top;border:1px solid #bbb;margin:0}th{position:-webkit-sticky;position:sticky;top:0;z-index:2}th[scope=row]{position:-webkit-sticky;position:sticky;left:0;z-index:2}th{padding:0.05em 0.3em 0.15em 0.3em;text-align:left;background-color:#ddd;border:1px solid #777;font-size:.95em;font-family:Segoe UI Semibold,sans-serif;font-weight:bold}th.private{font-family:Segoe UI;font-weight:normal;font-style:italic}td.private{background:#f4f4ee}td.private table{background:white}td,th.member{padding:0.1em 0.3em 0.2em 0.3em;position:initial}tr.repeat>th{font-size:90%;font-family:Segoe UI Semibold,sans-serif;border:none;background-color:#eee;color:#999;padding:0.0em 0.2em 0.15em 0.3em}td.typeheader{font-size:.95em;background-color:#4C74B2;color:white;padding:0 0.3em 0.25em 0.2em}td.n{text-align:right}a.typeheader,a:link.typeheader,a:visited.typeheader,a:link.extenser,a:visited.extenser{font-family:Segoe UI Semibold,sans-serif;font-size:.95em;font-weight:bold;text-decoration:none;color:white;margin-bottom:-0.1em;float:left}a.difheader,a:link.difheader,a:visited.difheader{color:#ff8}a.extenser,a:link.extenser,a:visited.extenser{margin:0 0 0 0.3em;padding-left:0.5em;padding-right:0.3em}a:hover.extenser{text-decoration:none}span.extenser{font-size:1.1em;line-height:0.8}span.cyclic{padding:0 0.2em 0 0;margin:0;font-family:Arial,sans-serif;font-weight:bold;margin:2px;font-size:1.5em;line-height:0;vertical-align:middle}.arrow-up,.arrow-down{display:inline-block;margin:0 0.3em 0.15em 0.1em;width:0;height:0;cursor:pointer}.arrow-up{border-left:0.35em solid transparent;border-right:0.35em solid transparent;border-bottom:0.35em solid white}.arrow-down{border-left:0.35em solid transparent;border-right:0.35em solid transparent;border-top:0.35em solid white}table.group{border:none;margin:0}td.group{border:none;padding:0 0.1em}div.spacer{margin:0.6em 0}div.headingpresenter{border:none;border-left:0.17em dashed #1a5;margin:.8em 0 1em 0.1em;padding-left:.5em}div.headingcontinuation{border:none;border-left:0.2em dotted #1a5;margin:-0.4em 0 1em 0.1em;padding-left:.5em}h1.headingpresenter{border:none;padding:0 0 0.3em 0;margin:0;font-family:Segoe UI Semibold,Arial;font-weight:bold;background-color:white;color:#209020;font-size:1.1em;line-height:1}td.summary{background-color:#DAEAFA;color:black;font-size:.95em;padding:0.05em 0.3em 0.2em 0.3em}tr.columntotal>td{background-color:#eee;font-family:Segoe UI Semibold;font-weight:bold;font-size:.95em;color:#4C74B2;text-align:right}.error > table{border-color:#B56172}.error > table > thead > tr > td.summary{background-color:#F4DEE3;color:black}.error > table > thead > tr > td.typeheader{background-color:#B56172}span.graphbar{background:#DAEAFA;color:#DAEAFA;padding-bottom:1px;margin-left:-0.2em;margin-right:0.2em}a.graphcolumn,a:link.graphcolumn,a:visited.graphcolumn{color:#4C74B2;text-decoration:none;font-weight:bold;font-family:Arial;font-size:1em;line-height:1;letter-spacing:-0.2em;margin-left:0.15em;margin-right:0.2em;cursor:pointer}a.collection,a:link.collection,a:visited.collection{color:#209020}a.reference,a:link.reference,a:visited.reference{color:#0080D1}span.meta,span.null{color:#209020}span.warning{color:red}span.false{color:#888}span.true{font-weight:bold}.highlight{background:#ff8}code.xml b{color:blue;font-weight:normal}code.xml i{color:brown;font-weight:normal;font-style:normal}code.xml em{color:red;font-weight:normal;font-style:normal}span.cc{background:#666;color:white;margin:0 1.5px;padding:0 1px;font-family:Consolas,monospace;border-radius:3px}ol,ul{margin:0.7em 0.3em;padding-left:2.5em}li{margin:0.3em 0}.difadd{background:#a3f3a3;border:1px solid #88d888}.difremove{background:#ffc8c8;border:1px solid #e8b3b3}.rendering{font-style:italic;color:brown}p.scriptLog{color:#a77;background:#f8f6f6;font-family:Consolas,monospace;font-size:9pt;padding:.1em .3em}::-ms-clear{display:none}input,textarea,button,select{font-family:Segoe UI;font-size:1em;padding:.2em}button{padding:.2em .4em}input,textarea,select{margin:.15em 0}input[type="checkbox"],input[type="radio"]{margin:0 0.4em 0 0;height:0.9em;width:0.9em}input[type="radio"]:focus,input[type="checkbox"]:focus{outline:thin dotted red}.checkbox-label{vertical-align:middle;position:relative;bottom:.07em;margin-right:.5em}fieldset{margin:0 .2em .4em .1em;border:1pt solid #aaa;padding:.1em .6em .4em .6em}legend{padding:.2em .1em}
	            
	            #floating-buttons {
	                position: fixed;
	                top: 20px;
	                right: 20px;
	                display: flex;
	                flex-direction: column;
	                gap: 10px;
	            }
	            
	            #floating-buttons button {
	                background-color: #f0f0f0;
	                border: none;
	                padding: 5px 10px;
	                cursor: pointer;
	            }
	            
	            #floating-buttons button:hover {
	                background-color: #ddd;
	            }
	            
	            """;
        }

        public static void WriteBeginHtml(TextWriter writer, bool docType, string title, string styles)
        {
            if (docType)
                writer.Write("<!DOCTYPE html>");

            writer.Write("<html><head><meta charset='utf-8'><title>");
            writer.Write(title);
            writer.Write("</title><style>");
            writer.Write(styles);
            writer.Write("</style>");
            writer.Write(
                """
                <script language="JavaScript" type="text/javascript">
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
                hideToLevel(1);
                </script>
                """
                );
            writer.Write("</head><body>");
            writer.Write(
	            """
	            <!-- Floating buttons -->
	            <div id="floating-buttons">
	                <button onclick="hideToLevel(1)">全部折叠</button>
	                <button onclick="hideToLevel(2)">展开到1层</button>
	                <button onclick="hideToLevel(3)">展开到2层</button>
	                <button onclick="hideToLevel(4)">展开到3层</button>
	                <button onclick="hideToLevel(1000)">全部展开</button>
	            </div>
	            """
	            );
        }

        public static void WriteEndHtml(TextWriter writer)
        {
            writer.Write("</body></html>");
        }
    }
}
