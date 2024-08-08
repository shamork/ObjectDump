using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace MiP.ObjectDump.Tests
{
    public class Dump_Tests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public Dump_Tests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        private object GetTestObject()
        {
            // initializ a thrown exception
            Exception ex = new InvalidOperationException("Wow bad idea.");
            try
            {
                throw ex;
            }
#pragma warning disable RCS1075 // Avoid empty catch clause that catches System.Exception.
            catch (Exception)
#pragma warning restore RCS1075 // Avoid empty catch clause that catches System.Exception.
            {
            }

            return new
            {
                String = "This is a string",
                Number = 13,
                Date = DateTime.Now,
                SubObject = new { Id = 29, Name = "Sub object 1" },
                Null = (string)null,
                Empty = "",
                Strings = new[] { "One", "Two", "Three" },
                List = new List<string> { "Four", "Five", "Six" },

                ComplexArray = new object[]
                {
                    new{Name="Ich", Id=1},
                    "asoisdas",
                    4,
                    new[]{4,5,6},
                    new{Name="Du", Id=2},
                    new{Name="Er", Id=3},
                    new{Name="Someone", Bla="Blubber"},
                    new{Bla="Blubb"},

                    new List<string> { "Seven","Eight", "between Eight and Nine", "Nine" }
                },
            };
        }

        [Fact]
        public void Dump_ToHtml()
        {
            GetTestObject().DumpToHtml("第1次");
            GetTestObject().DumpToHtml("第2次");

            string filename = Guid.NewGuid() + ".html";
            HtmlDump.SaveToHtmlFile(filename);
            _testOutputHelper.WriteLine(filename);
            Process.Start(filename);
        }

        class data
        {
            public int a { get; set; }
            public string b { get; set; }
            public bool c { get; set; }
            public string n { get; set; }
        }
        [Fact]
        public void Dump_ToHtml2()
        {
            new List<data>()
            { 
                new data{ a = 1, b = "b2", c = true, n = (string)null,},
                new data{ a = 1, b = "b23", c = true, n = (string)null,},
            }.DumpToHtml();
            new List<string>() { "1", "ab" }.DumpToHtml("简单列表");
            new List<object>()
            { 
                new { a = 1, b = 2, c = true, n = (string)null,chr='a'},
                new { a = 1, b = 2, c = true, n = (string)null,chr='A'},
            }.DumpToHtml("复杂列表");
            new
                {
                    a = 1, b = 2, c = true, n = (string)null,
                    d =
                        "sdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf",
                    e = new
                    {
                        a = 1, b = 2, c = true,
                        d =
                            "sdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf"
                    }
                }
                .DumpToHtml("嵌套对象");
            var list=new List<object>() {
                new {a=1,b=2,c=true,n=(string)null,d="sdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf",e=new {a=1,b=2,c=true,d="sdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf"}},
                new {a=1,b=2,c=true,n=(string)null,d="sdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf",e=new {a=1,b=2,c=true,d="sdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf"}},
                new {a=1,b=2,c=true,n=(string)null,d="sdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf",e=new {a=1,b=2,c=true,d="sdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf\r\nsdfasdfalskdfjalsdfadfadfadf"}}
            };
            list.DumpToHtml("第1次-嵌套对象列表");
            "字符串无标签".DumpToHtml();
            "列表无标签".DumpToHtml("下一行信息，字符串Dump");
            list.DumpToHtml();
            list.DumpToHtml("第2次-嵌套对象列表");
            string html = HtmlDump.ToHtml(list, 5);

            string filename = Guid.NewGuid() + ".html";
            HtmlDump.SaveToHtmlFile(filename);
            _testOutputHelper.WriteLine(filename);
            Process.Start(filename);
        }
    }
}
